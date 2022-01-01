using System;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using static Zeratool_player_C_Sharp.ZeratoolPlayerEngine;
using static Zeratool_player_C_Sharp.Utils;

namespace Zeratool_player_C_Sharp
{
    public partial class FormBookmarks : Form
    {
        private Point oldPos;
        private Size oldSize;

        public FormBookmarks()
        {
            InitializeComponent();

            //force to create a window to access its position.
            IntPtr hwnd = Handle;

            OnFormCreated();
        }

        private void OnFormCreated()
        {
            PlayerCreated += OnPlayerCreated;

            config.Saving += (object s, JObject json) =>
            {
                if (WindowState == FormWindowState.Normal)
                {
                    json["bookmarksLeft"] = Left;
                    json["bookmarksTop"] = Top;
                    json["bookmarksWidth"] = Width;
                    json["bookmarksHeight"] = Height;
                }
                else
                {
                    json["bookmarksLeft"] = oldPos.X;
                    json["bookmarksTop"] = oldPos.Y;
                    json["bookmarksWidth"] = oldSize.Width;
                    json["bookmarksHeight"] = oldSize.Height;
                }
            };

            config.Loading += (object s, JObject json) =>
            {
                JToken jt = json.Value<JToken>("bookmarksLeft");
                if (jt != null)
                {
                    Left = jt.Value<int>();
                }
                jt = json.Value<JToken>("bookmarksTop");
                if (jt != null)
                {
                    Top = jt.Value<int>();
                }
                jt = json.Value<JToken>("bookmarksWidth");
                if (jt != null)
                {
                    Width = jt.Value<int>();
                }
                jt = json.Value<JToken>("bookmarksHeight");
                if (jt != null)
                {
                    Height = jt.Value<int>();
                }

                if (!this.IsOnScreen())
                {
                    this.Center(Screen.PrimaryScreen.WorkingArea);
                }

                oldPos = new Point(Left, Top);
                oldSize = new Size(Width, Height);
            };
        }

        private void FormBookmarks_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Hide();
                e.Cancel = true;
            }
        }

        private void btnAddBookmark_Click(object sender, EventArgs e)
        {
            if (comboBoxPlayers.SelectedIndex < 0)
            {
                MessageBox.Show("Не выбран плеер!", "Ошибка!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ZeratoolPlayerGui z = GetPlayerFromComboBox(comboBoxPlayers);
            if (z != null && (z.State == PlayerState.Playing || z.State == PlayerState.Paused))
            {
                z.PutCurrentMomentToBookmarks();
                z.RefreshSeekBar();

                if (!z.Bookmarks.SaveToJsonFile(config.bookmarksFileName))
                {
                    MessageBox.Show("Не удалось сохранить список отметин!\nВозможно, что файл повреждён!", "Ошибка!",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Выбранный плеер неактивен!", "Ошибка!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRemoveBookmark_Click(object sender, EventArgs e)
        {
            ZeratoolPlayerGui z = GetPlayerFromComboBox(comboBoxPlayers);
            if (z == null)
            {
                MessageBox.Show("Не выбран плеер!", "Ошибка!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (listViewBookmarks.SelectedIndices.Count == 0)
            {
                MessageBox.Show("Не выбрана отметина!", "Ошибка!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int id = listViewBookmarks.SelectedIndices[0];
            z.Bookmarks.RemoveAt(id);
            z.RefreshSeekBar();
            ListBookmarks(z);
            if (listViewBookmarks.Items.Count > 0)
            {
                int selectedId = id < listViewBookmarks.Items.Count ? id : listViewBookmarks.Items.Count - 1;
                listViewBookmarks.SelectedIndices.Add(selectedId);
            }

            if (!z.Bookmarks.SaveToJsonFile(config.bookmarksFileName))
            {
                MessageBox.Show("Не удалось сохранить список отметин!\nВозможно, что файл повреждён!", "Ошибка!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listViewBookmarks_Resize(object sender, EventArgs e)
        {
            columnHeaderBookmarkTitle.Width = listViewBookmarks.Width - columnHeaderBookmarkTimecode.Width - 30;
        }

        private void listViewBookmarks_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (comboBoxPlayers.SelectedIndex >= 0 && listViewBookmarks.SelectedIndices.Count > 0)
                {
                    ZeratoolPlayerGui z = GetPlayerFromComboBox(comboBoxPlayers);
                    if (z != null && z.State == PlayerState.Playing || z.State == PlayerState.Paused)
                    {
                        int selectedIndex = listViewBookmarks.SelectedIndices[0];
                        string timeCodeString = listViewBookmarks.Items[selectedIndex].Text;
                        TimeSpan timeSpan = ZeratoolBookmarks.ParseTimeCode(timeCodeString);
                        if (timeSpan == TimeSpan.MaxValue)
                        {
                            MessageBox.Show("Ошибка!", "Ошибка!",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (timeSpan.TotalSeconds > z.TrackDuration)
                        {
                            MessageBox.Show($"Не удалось найти позицию {timeCodeString}!", "Ошибка!",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        z.TrackPosition = timeSpan.TotalSeconds;
                    }
                }
            }
        }

        private void comboBoxPlayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            listViewBookmarks.Items.Clear();
            ZeratoolPlayerGui z = GetPlayerFromComboBox(comboBoxPlayers);

            if (z != null && z != activePlayer)
            {
                z.Activate();
            }
        }

        private void OnPlayerCreated(ZeratoolPlayerGui z, bool maximized)
        {
            z.Activated += OnPlayerActivated;
            z.TitleChanged += OnPlayerTitleChanged;
            z.FileNameChanged += (s, fileName) =>
            {
                ListBookmarks(s as ZeratoolPlayerGui);
            };
            z.Closing += OnPlayerClosing;
            z.BookmarkAdded += OnPlayerBookmarkAdded;

            string t = $"Player [{players.Count - 1}]: {z.Title}";
            comboBoxPlayers.Items.Add(new PlayerListItem(z, t));
        }

        private void OnPlayerClosing(object sender)
        {
            ListPlayers();
            listViewBookmarks.Items.Clear();
            if (activePlayer != null)
            {
                activePlayer.Activate();
            }
        }

        private void OnPlayerActivated(object sender)
        {
            System.Diagnostics.Debug.WriteLine("Player activated in form bookmarks");
            ZeratoolPlayerGui z = sender as ZeratoolPlayerGui;
            int id = FindPlayerInComboBox(comboBoxPlayers, z);
            if (comboBoxPlayers.SelectedIndex != id)
            {
                comboBoxPlayers.SelectedIndex = id;
            }
            ListBookmarks(z);
        }

        private void OnPlayerTitleChanged(object sender, string title)
        {
            ZeratoolPlayerGui z = sender as ZeratoolPlayerGui;
            int id = FindPlayerInComboBox(comboBoxPlayers, z);
            string t = $"Player [{id}]: {title}";
            comboBoxPlayers.Items[id] = new PlayerListItem(z, t);
        }

        private void OnPlayerBookmarkAdded(object sender, BookmarkItem bookmarkItem, int positionIndex)
        {
            ZeratoolPlayerGui z = GetPlayerFromComboBox(comboBoxPlayers);

            if (z != null && z == (ZeratoolPlayerGui)sender)
            {
                string timeCode = ZeratoolBookmarks.TimeToString(new DateTime(bookmarkItem.TimeCode.Ticks));
                ListViewItem listViewItem = new ListViewItem(timeCode);
                listViewItem.SubItems.Add(bookmarkItem.ShortDescription);

                listViewBookmarks.Items.Insert(positionIndex, listViewItem);
                listViewBookmarks.SelectedIndices.Clear();
                listViewBookmarks.SelectedIndices.Add(positionIndex);

                listViewBookmarks.EnsureVisible(positionIndex);
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

        private int ListBookmarks(ZeratoolPlayerGui playerGui)
        {
            listViewBookmarks.Items.Clear();
            int bookmarkCount = playerGui.Bookmarks.Count;
            if (bookmarkCount > 0)
            {
                for (int i = 0; i < bookmarkCount; i++)
                {
                    BookmarkItem bookmarkItem = playerGui.Bookmarks[i];
                    string timeCode = ZeratoolBookmarks.TimeToString(new DateTime(bookmarkItem.TimeCode.Ticks));
                    ListViewItem listViewItem = new ListViewItem(timeCode);
                    listViewItem.SubItems.Add(bookmarkItem.ShortDescription);
                    listViewBookmarks.Items.Add(listViewItem);
                }
            }
            return bookmarkCount;
        }
    }
}
