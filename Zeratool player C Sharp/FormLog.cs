using System;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using static Zeratool_player_C_Sharp.Utils;

namespace Zeratool_player_C_Sharp
{
    public partial class FormLog : Form
    {
        private Point oldPos;
        private Size oldSize;

        public FormLog()
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
                    json["logLeft"] = Left;
                    json["logTop"] = Top;
                    json["logWidth"] = Width;
                    json["logHeight"] = Height;
                }
                else
                {
                    json["logLeft"] = oldPos.X;
                    json["logTop"] = oldPos.Y;
                    json["logWidth"] = oldSize.Width;
                    json["logHeight"] = oldSize.Height;
                }
            };

            config.Loading += (object s, JObject json) =>
            {
                JToken jt = json.Value<JToken>("logLeft");
                if (jt != null)
                {
                    Left = jt.Value<int>();
                }
                jt = json.Value<JToken>("logTop");
                if (jt != null)
                {
                    Top = jt.Value<int>();
                }
                jt = json.Value<JToken>("logWidth");
                if (jt != null)
                {
                    Width = jt.Value<int>();
                }
                jt = json.Value<JToken>("logHeight");
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

        private void FormLog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
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

        private void OnPlayerCreated(ZeratoolPlayerGui z, bool maximized)
        {
            z.Activated += OnPlayerActivated;
            z.Closing += OnPlayerClosing;
            z.TitleChanged += OnPlayerTitleChanged;
            z.PlayerEngine.LogAdded += OnPlayerLogItemAdded;
            z.PlayerEngine.LogCleared += OnPlayerLogCleared;

            string t = $"Player [{players.Count - 1}]: {z.Title}";
            comboBoxPlayers.Items.Add(new PlayerListItem(z, t));
        }

        private void OnPlayerClosing(object sender)
        {
            ListPlayers();
        }

        private void OnPlayerActivated(object sender)
        {
            System.Diagnostics.Debug.WriteLine("Player activated in form log");
            ZeratoolPlayerGui z = sender as ZeratoolPlayerGui;
            int id = FindPlayerInComboBox(comboBoxPlayers, z);
            if (comboBoxPlayers.SelectedIndex != id)
            {
                comboBoxPlayers.SelectedIndex = id;

                ListLog(z.PlayerEngine);
            }
        }

        private void OnPlayerTitleChanged(object sender, string title)
        {
            ZeratoolPlayerGui z = sender as ZeratoolPlayerGui;
            int id = FindPlayerInComboBox(comboBoxPlayers, z);
            string t = $"Player [{id}]: {title}";
            comboBoxPlayers.Items[id] = new PlayerListItem(z, t);
        }

        private void OnPlayerLogCleared(object sender)
        {
            ZeratoolPlayerGui z = GetPlayerFromComboBox(comboBoxPlayers);

            if (z != null && (sender as ZeratoolPlayerEngine) == z.PlayerEngine)
            {
                ListLog(z.PlayerEngine);
            }
        }

        private void OnPlayerLogItemAdded(object sender, ZeratoolLogItem logItem)
        {
            ZeratoolPlayerGui z = GetPlayerFromComboBox(comboBoxPlayers);

            if (z != null && (sender as ZeratoolPlayerEngine) == z.PlayerEngine)
            {
                ListViewItem item = new ListViewItem(logItem.DateTime.ToString("yyyy.MM.dd, HH:mm:ss"));
                item.SubItems.Add(logItem.Event);
                item.SubItems.Add(logItem.ShortResultDescription);
                lvLog.Items.Add(item);
            }
        }

        private void ListLog(ZeratoolPlayerEngine playerEngine)
        {
            lvLog.Items.Clear();

            foreach (ZeratoolLogItem logItem in playerEngine.Log)
            {
                ListViewItem item = new ListViewItem(logItem.DateTime.ToString("yyyy.MM.dd, HH:mm:ss"));
                item.SubItems.Add(logItem.Event);
                item.SubItems.Add(logItem.ShortResultDescription);
                lvLog.Items.Add(item);
            }
        }
    }
}
