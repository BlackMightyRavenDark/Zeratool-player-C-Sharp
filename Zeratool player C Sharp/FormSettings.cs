﻿using System;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using static Zeratool_player_C_Sharp.Utils;
using static Zeratool_player_C_Sharp.ZeratoolPlayerEngine;
using static Zeratool_player_C_Sharp.DirectShowUtils;

namespace Zeratool_player_C_Sharp
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();

            //force to create a window to access its position.
            IntPtr hwnd = Handle;

            OnFormCreate();
        }

        private void OnFormCreate()
        {
            comboBoxAudioRenderers.Items.Clear();
            foreach (MonikerItem item in audioOutputMonikers)
            {
                comboBoxAudioRenderers.Items.Add(item.DisplayName);
            }
            comboBoxAudioRenderers.SelectedIndex = audioOutputMonikers.Count == 0 ? -1 : 0;

            PlayerCreated += OnPlayerCreated;

            config.Saving += (object s, JObject json) =>
            {
                if (WindowState == FormWindowState.Normal)
                {
                    json["settingsLeft"] = Left;
                    json["settingsTop"] = Top;
                }
            };

            config.Loading += (object s, JObject json) =>
            {
                JToken jt = json.Value<JToken>("settingsLeft");
                if (jt != null)
                {
                    Left = jt.Value<int>();
                }
                jt = json.Value<JToken>("settingsTop");
                if (jt != null)
                {
                    Top = jt.Value<int>();
                }

                jt = json.Value<JToken>("graphMode");
                if (jt == null || !Enum.TryParse<DirectShowGraphMode>(jt.Value<string>(), out config.graphMode))
                {
                    config.graphMode = DirectShowGraphMode.Manual;
                }

                if (!this.IsOnScreen())
                {
                    this.Center(Screen.PrimaryScreen.WorkingArea);
                }
            };
        }

        private void FormSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
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

        private void ListFilters(ZeratoolPlayerEngine playerEngine)
        {
            comboBoxSplittersAVI.Items.Clear();
            comboBoxSplittersAVI.Items.Add("Автоматически (перебор всех вариантов)");
            foreach (FilterItem item in playerEngine.filters.mediaSplittersAVI)
            {
                comboBoxSplittersAVI.Items.Add(item.DisplayName);
            }
            comboBoxSplittersAVI.SelectedIndex = playerEngine.filters.mediaSplitterAviId + 1;

            comboBoxSplittersMPG.Items.Clear();
            comboBoxSplittersMPG.Items.Add("Автоматически (перебор всех вариантов)");
            foreach (FilterItem item in playerEngine.filters.mediaSplittersMPG)
            {
                comboBoxSplittersMPG.Items.Add(item.DisplayName);
            }
            comboBoxSplittersMPG.SelectedIndex = playerEngine.filters.mediaSplitterMpgId + 1;

            comboBoxSplittersTS.Items.Clear();
            comboBoxSplittersTS.Items.Add("Автоматически (перебор всех вариантов)");
            foreach (FilterItem item in playerEngine.filters.mediaSplittersTS)
            {
                comboBoxSplittersTS.Items.Add(item.DisplayName);
            }
            comboBoxSplittersTS.SelectedIndex = playerEngine.filters.mediaSplitterTsId + 1;

            comboBoxSplittersMP4.Items.Clear();
            comboBoxSplittersMP4.Items.Add("Автоматически (перебор всех вариантов)");
            foreach (FilterItem item in playerEngine.filters.mediaSplittersMP4)
            {
                comboBoxSplittersMP4.Items.Add(item.DisplayName);
            }
            comboBoxSplittersMP4.SelectedIndex = playerEngine.filters.mediaSplitterMp4Id + 1;

            comboBoxSplittersMKV.Items.Clear();
            comboBoxSplittersMKV.Items.Add("Автоматически (перебор всех вариантов)");
            foreach (FilterItem item in playerEngine.filters.mediaSplittersMKV)
            {
                comboBoxSplittersMKV.Items.Add(item.DisplayName);
            }
            comboBoxSplittersMKV.SelectedIndex = playerEngine.filters.mediaSplitterMkvId + 1;

            comboBoxSplittersOther.Items.Clear();
            comboBoxSplittersOther.Items.Add("Автоматически (перебор всех вариантов)");
            foreach (FilterItem item in playerEngine.filters.mediaSplittersOther)
            {
                comboBoxSplittersOther.Items.Add(item.DisplayName);
            }
            comboBoxSplittersOther.SelectedIndex = playerEngine.filters.mediaSplitterOtherId + 1;

            comboBoxVideoDecoders.Items.Clear();
            comboBoxVideoDecoders.Items.Add("Автоматически (перебор всех вариантов)");
            foreach (FilterItem item in playerEngine.filters.videoDecoders)
            {
                comboBoxVideoDecoders.Items.Add(item.DisplayName);
            }
            comboBoxVideoDecoders.SelectedIndex = playerEngine.filters.prefferedVideoDecoderId + 1;

            comboBoxVideoRenderers.Items.Clear();
            foreach (FilterItem item in playerEngine.filters.videoRenderers)
            {
                comboBoxVideoRenderers.Items.Add(item.DisplayName);
            }
            comboBoxVideoRenderers.SelectedIndex = playerEngine.filters.prefferedVideoRendererId;

            comboBoxAudioDecoders.Items.Clear();
            comboBoxAudioDecoders.Items.Add("Автоматически (перебор всех вариантов)");
            foreach (FilterItem item in playerEngine.filters.audioDecoders)
            {
                comboBoxAudioDecoders.Items.Add(item.DisplayName);
            }
            comboBoxAudioDecoders.SelectedIndex = playerEngine.filters.prefferedAudioDecoderId + 1;

            comboBoxAudioRenderers.Items.Clear();
            foreach (MonikerItem item in playerEngine.filters.audioRenderers)
            {
                comboBoxAudioRenderers.Items.Add(item.DisplayName);
            }
            comboBoxAudioRenderers.SelectedIndex = playerEngine.filters.prefferedAudioRendererId;
        }

        private void OnPlayerCreated(ZeratoolPlayerGui z, bool maximized)
        {
            z.Activated += OnPlayerActivated;
            z.Closing += OnPlayerClosing;
            z.TitleChanged += OnPlayerTitleChanged;
            z.TrackRendered += OnPlayerTrackRendered;

            string t = $"Player [{players.Count - 1}]: {z.Title}";
            comboBoxPlayers.Items.Add(new PlayerListItem(z, t));
        }

        private void OnPlayerClosing(object sender)
        {
            ListPlayers();
        }

        private void OnPlayerActivated(object sender)
        {
            System.Diagnostics.Debug.WriteLine("Player activated in form settings");
            ZeratoolPlayerGui z = sender as ZeratoolPlayerGui;
            int id = FindPlayerInComboBox(comboBoxPlayers, z);
            if (comboBoxPlayers.SelectedIndex != id)
            {
                comboBoxPlayers.SelectedIndex = id;

                ListFilters(z.PlayerEngine);
                RefreshParameters(z);
            }
        }

        private void OnPlayerTitleChanged(object sender, string title)
        {
            ZeratoolPlayerGui z = sender as ZeratoolPlayerGui;
            int id = FindPlayerInComboBox(comboBoxPlayers, z);
            string t = $"Player [{id}]: {title}";
            comboBoxPlayers.Items[id] = new PlayerListItem(z, t);
        }

        private void OnPlayerTrackRendered(object sender, int errorCode)
        {
            if (comboBoxPlayers.Items.Count > 0)
            {
                ZeratoolPlayerGui z = (comboBoxPlayers.Items[comboBoxPlayers.SelectedIndex] as PlayerListItem).Player;
                if (sender as ZeratoolPlayerGui == z)
                {
                    RefreshParameters(z);
                }
            }
        }

        private void rbGraphModeAutomatic_CheckedChanged(object sender, EventArgs e)
        {
            if (comboBoxPlayers.Items.Count > 0)
            {
                ZeratoolPlayerGui z = (comboBoxPlayers.Items[comboBoxPlayers.SelectedIndex] as PlayerListItem).Player;
                z.PrefferedGraphMode = ZeratoolPlayerEngine.DirectShowGraphMode.Automatic;
            }
        }

        private void rbGraphModeIntellectual_CheckedChanged(object sender, EventArgs e)
        {
            if (comboBoxPlayers.Items.Count > 0)
            {
                ZeratoolPlayerGui z = (comboBoxPlayers.Items[comboBoxPlayers.SelectedIndex] as PlayerListItem).Player;
                z.PrefferedGraphMode = ZeratoolPlayerEngine.DirectShowGraphMode.Intellectual;
            }
        }

        private void rbGraphModeManual_CheckedChanged(object sender, EventArgs e)
        {
            if (comboBoxPlayers.Items.Count > 0)
            {
                ZeratoolPlayerGui z = (comboBoxPlayers.Items[comboBoxPlayers.SelectedIndex] as PlayerListItem).Player;
                z.PrefferedGraphMode = ZeratoolPlayerEngine.DirectShowGraphMode.Manual;
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

        private void RefreshParameters(ZeratoolPlayerGui z)
        {
            switch (z.GraphMode)
            {
                case ZeratoolPlayerEngine.DirectShowGraphMode.Automatic:
                    rbGraphModeAutomatic.ForeColor = Color.Red;
                    rbGraphModeIntellectual.ForeColor = Color.Black;
                    rbGraphModeManual.ForeColor = Color.Black;
                    break;

                case ZeratoolPlayerEngine.DirectShowGraphMode.Intellectual:
                    rbGraphModeAutomatic.ForeColor = Color.Black;
                    rbGraphModeIntellectual.ForeColor = Color.Red;
                    rbGraphModeManual.ForeColor = Color.Black;
                    break;

                case ZeratoolPlayerEngine.DirectShowGraphMode.Manual:
                    rbGraphModeAutomatic.ForeColor = Color.Black;
                    rbGraphModeIntellectual.ForeColor = Color.Black;
                    rbGraphModeManual.ForeColor = Color.Red;
                    break;
            }

            switch (z.PrefferedGraphMode)
            {
                case ZeratoolPlayerEngine.DirectShowGraphMode.Automatic:
                    rbGraphModeAutomatic.Checked = true;
                    break;

                case ZeratoolPlayerEngine.DirectShowGraphMode.Intellectual:
                    rbGraphModeIntellectual.Checked = true;
                    break;

                case ZeratoolPlayerEngine.DirectShowGraphMode.Manual:
                    rbGraphModeManual.Checked = true;
                    break;
            }
        }

        private void btnRebuildGraph_Click(object sender, EventArgs e)
        {
            if (comboBoxPlayers.Items.Count > 0)
            {
                btnRebuildGraph.Enabled = false;

                ZeratoolPlayerGui z = (comboBoxPlayers.Items[comboBoxPlayers.SelectedIndex] as PlayerListItem).Player;
                double pos = z.TrackPosition;
                z.Clear();
                if (z.Play() == S_OK)
                {
                    z.TrackPosition = pos;
                }

                btnRebuildGraph.Enabled = true;
            }
        }

        private void comboBoxSplittersAVI_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxPlayers.SelectedIndex >= 0)
            {
                ZeratoolPlayerGui z = (comboBoxPlayers.Items[comboBoxPlayers.SelectedIndex] as PlayerListItem).Player;
                z.PlayerEngine.filters.mediaSplitterAviId = comboBoxSplittersAVI.SelectedIndex - 1;
            }
        }

        private void comboBoxSplittersMPG_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxPlayers.SelectedIndex >= 0)
            {
                ZeratoolPlayerGui z = (comboBoxPlayers.Items[comboBoxPlayers.SelectedIndex] as PlayerListItem).Player;
                z.PlayerEngine.filters.mediaSplitterMpgId = comboBoxSplittersMPG.SelectedIndex - 1;
            }
        }

        private void comboBoxSplittersTS_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxPlayers.SelectedIndex >= 0)
            {
                ZeratoolPlayerGui z = (comboBoxPlayers.Items[comboBoxPlayers.SelectedIndex] as PlayerListItem).Player;
                z.PlayerEngine.filters.mediaSplitterTsId = comboBoxSplittersTS.SelectedIndex - 1;
            }
        }

        private void comboBoxSplittersMP4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxPlayers.SelectedIndex >= 0)
            {
                ZeratoolPlayerGui z = (comboBoxPlayers.Items[comboBoxPlayers.SelectedIndex] as PlayerListItem).Player;
                z.PlayerEngine.filters.mediaSplitterMp4Id = comboBoxSplittersMP4.SelectedIndex - 1;
            }
        }

        private void comboBoxSplittersMKV_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxPlayers.SelectedIndex >= 0)
            {
                ZeratoolPlayerGui z = (comboBoxPlayers.Items[comboBoxPlayers.SelectedIndex] as PlayerListItem).Player;
                z.PlayerEngine.filters.mediaSplitterMkvId = comboBoxSplittersMKV.SelectedIndex - 1;
            }
        }

        private void comboBoxSplittersOther_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxPlayers.SelectedIndex >= 0)
            {
                ZeratoolPlayerGui z = (comboBoxPlayers.Items[comboBoxPlayers.SelectedIndex] as PlayerListItem).Player;
                z.PlayerEngine.filters.mediaSplitterOtherId = comboBoxSplittersOther.SelectedIndex - 1;
            }
        }

        private void comboBoxVideoDecoders_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxPlayers.SelectedIndex >= 0)
            {
                ZeratoolPlayerGui z = (comboBoxPlayers.Items[comboBoxPlayers.SelectedIndex] as PlayerListItem).Player;
                z.PlayerEngine.filters.prefferedVideoDecoderId = comboBoxVideoDecoders.SelectedIndex - 1;
            }
        }

        private void comboBoxVideoRenderers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxPlayers.SelectedIndex >= 0)
            {
                ZeratoolPlayerGui z = (comboBoxPlayers.Items[comboBoxPlayers.SelectedIndex] as PlayerListItem).Player;
                z.PlayerEngine.filters.prefferedVideoRendererId = comboBoxVideoRenderers.SelectedIndex;
            }
        }

        private void comboBoxAudioDecoders_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxPlayers.SelectedIndex >= 0)
            {
                ZeratoolPlayerGui z = (comboBoxPlayers.Items[comboBoxPlayers.SelectedIndex] as PlayerListItem).Player;
                z.PlayerEngine.filters.prefferedAudioDecoderId = comboBoxAudioDecoders.SelectedIndex - 1;
            }
        }

        private void comboBoxAudioRenderers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxPlayers.SelectedIndex >= 0)
            {
                ZeratoolPlayerGui z = (comboBoxPlayers.Items[comboBoxPlayers.SelectedIndex] as PlayerListItem).Player;
                z.PlayerEngine.filters.prefferedAudioRendererId = comboBoxAudioRenderers.SelectedIndex;
            }
        }

        private void btnSaveFilters_Click(object sender, EventArgs e)
        {
            if (comboBoxPlayers.SelectedIndex >= 0)
            {
                ZeratoolPlayerGui z = (comboBoxPlayers.Items[comboBoxPlayers.SelectedIndex] as PlayerListItem).Player;
                if (z != null)
                {
                    if (z.IsMaximized)
                    {
                        z.PlayerEngine.filters.SaveToJsonFile(config.filtersConfigFileName);
                        MessageBox.Show("Настройки сохранены!", "Zeratool player",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Можно сохранить настройки только главного плеера!", "Ошибка!",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return;
                }
            }
            MessageBox.Show("Не выбран плеер!", "Ошибка!",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
