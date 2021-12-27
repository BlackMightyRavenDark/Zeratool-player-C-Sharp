using System;
using System.Windows.Forms;
using static Zeratool_player_C_Sharp.ZeratoolPlayerEngine;
using static Zeratool_player_C_Sharp.Utils;

namespace Zeratool_player_C_Sharp
{
    public partial class FormBookmarks : Form
    {
        public FormBookmarks()
        {
            InitializeComponent();
            OnFormCreated();
        }

        private void OnFormCreated()
        {
            PlayerCreated += OnPlayerCreated;
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

            ZeratoolPlayerGui z = (comboBoxPlayers.Items[comboBoxPlayers.SelectedIndex] as PlayerListItem).Player;
            if (z.State == PlayerState.Playing || z.State == PlayerState.Paused)
            {
                TimeSpan timeSpan = TimeSpan.FromSeconds(z.TrackPosition);
                string timeCode = new DateTime(timeSpan.Ticks).ToString("HH:mm:ss.f");
                int id = z.Bookmarks.Add(timeSpan, timeCode);
                ListBookmarks(z);
                if (id >= 0 && listViewBookmarks.Items.Count > 0)
                {
                    listViewBookmarks.SelectedIndices.Add(id);
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
            if (comboBoxPlayers.SelectedIndex < 0)
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
            ZeratoolPlayerGui z = (comboBoxPlayers.Items[comboBoxPlayers.SelectedIndex] as PlayerListItem).Player;
            int id = listViewBookmarks.SelectedIndices[0];
            z.Bookmarks.RemoveAt(id);
            ListBookmarks(z);
            if (listViewBookmarks.Items.Count > 0)
            {
                int selectedId = id < listViewBookmarks.Items.Count ? id : listViewBookmarks.Items.Count - 1;
                listViewBookmarks.SelectedIndices.Add(selectedId);
            }
        }

        private void comboBoxPlayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxPlayers.SelectedIndex >= 0)
            {
                ZeratoolPlayerGui z = (comboBoxPlayers.Items[comboBoxPlayers.SelectedIndex] as PlayerListItem).Player;

                if (activePlayer != z)
                {
                    z.Activate();
                }
            }
        }

        private void OnPlayerCreated(ZeratoolPlayerGui z, bool maximized)
        {
            z.Activated += OnPlayerActivated;
            z.TitleChanged += OnPlayerTitleChanged;
            z.Closing += OnPlayerClosing;

            string t = $"Player [{players.Count - 1}]: {z.Title}";
            comboBoxPlayers.Items.Add(new PlayerListItem(z, t));
        }

        private void OnPlayerClosing(object sender)
        {
            ListPlayers();
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
        }

        private void OnPlayerTitleChanged(object sender, string title)
        {
            ZeratoolPlayerGui z = sender as ZeratoolPlayerGui;
            int id = FindPlayerInComboBox(comboBoxPlayers, z);
            string t = $"Player [{id}]: {title}";
            comboBoxPlayers.Items[id] = new PlayerListItem(z, t);
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

        private void ListBookmarks(ZeratoolPlayerGui playerGui)
        {
            listViewBookmarks.Items.Clear();
            for (int i = 0; i < playerGui.Bookmarks.Count; i++)
            {
                BookmarkItem bookmarkItem = playerGui.Bookmarks[i];
                string timeCode = ZeratoolBookmarks.TimeToString(new DateTime(bookmarkItem.TimeCode.Ticks));
                ListViewItem listViewItem = new ListViewItem(timeCode);
                listViewItem.SubItems.Add(bookmarkItem.ShortDescription);
                listViewBookmarks.Items.Add(listViewItem);
            }
        }

        private void listViewBookmarks_Resize(object sender, EventArgs e)
        {
            columnHeaderBookmarkTitle.Width = listViewBookmarks.Width - columnHeaderBookmarkTimecode.Width - 30;
        }
    }
}
