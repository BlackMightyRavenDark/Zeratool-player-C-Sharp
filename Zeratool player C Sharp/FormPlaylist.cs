using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using static Zeratool_player_C_Sharp.Utils;

namespace Zeratool_player_C_Sharp
{
    public partial class FormPlaylist : Form
    {
        private Point oldPos;
        private Size oldSize;

        public FormPlaylist()
        {
            InitializeComponent();

            //force to create a window to access its position.
            IntPtr hwnd = Handle;

            OnFormCreate();
        }

        private void OnFormCreate()
        {
            PlayerCreated += OnPlayerCreated;

            config.Saving += (object s, JObject json) =>
            {
                if (WindowState == FormWindowState.Normal)
                {
                    json["playlistLeft"] = Left;
                    json["playlistTop"] = Top;
                    json["playlistWidth"] = Width;
                    json["playlistHeight"] = Height;
                }
                else
                {
                    json["playlistLeft"] = oldPos.X;
                    json["playlistTop"] = oldPos.Y;
                    json["playlistWidth"] = oldSize.Width;
                    json["playlistHeight"] = oldSize.Height;
                }
            };

            config.Loading += (object s, JObject json) =>
            {
                JToken jt = json.Value<JToken>("playlistLeft");
                if (jt != null)
                {
                    Left = jt.Value<int>();
                }
                jt = json.Value<JToken>("playlistTop");
                if (jt != null)
                {
                    Top = jt.Value<int>();
                }
                jt = json.Value<JToken>("playlistWidth");
                if (jt != null)
                {
                    Width = jt.Value<int>();
                }
                jt = json.Value<JToken>("playlistHeight");
                if (jt != null)
                {
                    Height = jt.Value<int>();
                }

                chkCycleCurrentTrack.Checked = config.playlistCycleCurrentTrack;

                if (!this.IsOnScreen())
                {
                    this.Center(Screen.PrimaryScreen.WorkingArea);
                }

                oldPos = new Point(Left, Top);
                oldSize = new Size(Width, Height);
            };
        }

        //To proper ListBox repaint
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams result = base.CreateParams;
                const int WS_EX_COMPOSITED = 0x02000000;
                result.ExStyle |= WS_EX_COMPOSITED;
                return result;
            }
        }

        private void FormPlaylist_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void lbPlaylist_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void lbPlaylist_DragDrop(object sender, DragEventArgs e)
        {
            if (comboBoxPlayers.SelectedIndex >= 0 && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] strings = (string[])e.Data.GetData(DataFormats.FileDrop);
                List<string> playableFiles = GetPlayableFiles(strings);
                if (playableFiles.Count > 0)
                {
                    ZeratoolPlayerGui z = (comboBoxPlayers.Items[comboBoxPlayers.SelectedIndex] as PlayerListItem).Player;
                    z.Playlist.AddRange(playableFiles);
                }
            }
        }

        private void lbPlaylist_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = lbPlaylist.Font.Height + 2;
        }

        private void lbPlaylist_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                bool isPlaying = e.Index == activePlayer.Playlist.PlayingIndex;
                Brush brush = isPlaying ? Brushes.Yellow : Brushes.Lime;
                if (isPlaying)
                {
                    Rectangle r = new Rectangle(1, e.Bounds.Y + 1, e.Bounds.Height - 2, e.Bounds.Height - 2);
                    e.Graphics.DrawIcon(Properties.Resources.play_active, r);
                }
                string t = Path.GetFileName(lbPlaylist.Items[e.Index].ToString());
                float x = isPlaying ? lbPlaylist.Font.Size + 6.0f : 0.0f;
                e.Graphics.DrawString(t, lbPlaylist.Font, brush, new PointF(x, e.Bounds.Y + 1.0f));

                bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
                if (isSelected)
                {
                    e.Bounds.Inflate(0, -1);
                    e.Graphics.DrawRectangle(Pens.White, e.Bounds);
                }
            }
        }

        private void lbPlaylist_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lbPlaylist.SelectedIndex >= 0 && activePlayer != null)
            {
                int errorCode = activePlayer.Playlist.PlayFile(lbPlaylist.SelectedIndex);
                if (errorCode != DirectShowUtils.S_OK)
                {
                    ShowErrorMessage(activePlayer, errorCode);
                }
            }
        }

        private void chkCycleCurrentTrack_CheckedChanged(object sender, EventArgs e)
        {
            config.playlistCycleCurrentTrack = chkCycleCurrentTrack.Checked;
        }

        private void OnPlayerCreated(ZeratoolPlayerGui z, bool maximized)
        {
            z.Activated += OnPlayerActivated;
            z.TitleChanged += OnPlayerTitleChanged;
            z.Closing += OnPlayerClosing;
            z.Playlist.ItemAdded += OnPlayerPlaylistItemAdded;
            z.Playlist.ItemRemoved += OnPlayerPlaylistItemRemoved;
            z.Playlist.Cleared += OnPlayerPlaylistCleared;
            z.TrackRendered += (s, errorCode) =>
            {
                ZeratoolPlayerGui playerGui = GetPlayerFromComboBox(comboBoxPlayers);

                if (playerGui != null && (s as ZeratoolPlayerEngine) == playerGui.PlayerEngine)
                {
                    lbPlaylist.SelectedIndex = playerGui.Playlist.PlayingIndex;
                    lbPlaylist.Refresh();
                }
            };

            z.Playlist.IndexChanged += (s, index) =>
            {
                ZeratoolPlayerGui playerGui = GetPlayerFromComboBox(comboBoxPlayers);

                if (playerGui != null && (s as ZeratoolPlaylist) == playerGui.Playlist)
                {
                    lbPlaylist.SelectedIndex = playerGui.Playlist.PlayingIndex;
                    lbPlaylist.Refresh();
                }
            };

            string t = $"Player [{players.Count - 1}]: {z.Title}";
            comboBoxPlayers.Items.Add(new PlayerListItem(z, t));
        }

        private void OnPlayerClosing(object sender)
        {
            ListPlayers();
        }

        private void OnPlayerActivated(object sender)
        {
            ZeratoolPlayerGui z = sender as ZeratoolPlayerGui;
            int id = FindPlayerInComboBox(comboBoxPlayers, z);
            comboBoxPlayers.SelectedIndex = id;
            lbPlaylist.Items.Clear();
            if (z.Playlist.Count > 0)
            {
                lbPlaylist.Items.AddRange(z.Playlist.ToArray());
            }
        }

        private void OnPlayerTitleChanged(object sender, string title)
        {
            ZeratoolPlayerGui z = sender as ZeratoolPlayerGui;
            int id = FindPlayerInComboBox(comboBoxPlayers, z);
            string t = $"Player [{id}]: {title}";
            comboBoxPlayers.Items[id] = new PlayerListItem(z, t);
        }

        private void OnPlayerPlaylistCleared(object sender)
        {
            ZeratoolPlayerGui z = GetPlayerFromComboBox(comboBoxPlayers);

            if (z != null && (sender as ZeratoolPlaylist) == z.Playlist)
            {
                lbPlaylist.Items.Clear();
            }
        }

        private void OnPlayerPlaylistItemAdded(object sender, string fileName)
        {
            ZeratoolPlayerGui z = GetPlayerFromComboBox(comboBoxPlayers);

            if (z != null && (sender as ZeratoolPlaylist) == z.Playlist)
            {
                lbPlaylist.Items.Add(fileName);
            }
        }

        private void OnPlayerPlaylistItemRemoved(object sender, int index, string fileName)
        {
            ZeratoolPlayerGui z = GetPlayerFromComboBox(comboBoxPlayers);

            if (z != null && (sender as ZeratoolPlaylist) == z.Playlist)
            {
                lbPlaylist.Items.RemoveAt(index);
            }
        }

        private void ListPlayers()
        {
            comboBoxPlayers.Items.Clear();
            if (players.Count > 0)
            {
                for (int i = 0; i < players.Count; i++)
                {
                    ZeratoolPlayerGui z = players[i];
                    string t = $"Player [{i}]: {z.Title}";
                    comboBoxPlayers.Items.Add(new PlayerListItem(z, t));
                }
                if (activePlayer != null)
                {
                    int id = FindPlayerInComboBox(comboBoxPlayers, activePlayer);
                    comboBoxPlayers.SelectedIndex = id;
                }
            }
        }

        private void comboBoxPlayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            ZeratoolPlayerGui z = GetPlayerFromComboBox(comboBoxPlayers);

            if (z != null && z != activePlayer)
            {
                z.Activate();
            }
        }
    }
}
