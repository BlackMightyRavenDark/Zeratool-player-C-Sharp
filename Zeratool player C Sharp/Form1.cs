using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using static Zeratool_player_C_Sharp.Utils;
using static Zeratool_player_C_Sharp.ZeratoolPlayerGui;
using static Zeratool_player_C_Sharp.DirectShowUtils;

namespace Zeratool_player_C_Sharp
{
    public partial class Form1 : Form
    {
        private bool firstShown = true;
        private Point oldPos;
        private Size oldSize;

        public Form1()
        {
            InitializeComponent();

            //force to create a window to access its position.
            IntPtr hwnd = Handle;

            OnFormCreate();
        }

        private void OnFormCreate()
        {
            config = new MainConfiguration(Application.StartupPath + "\\zeratool.json");
            config.Saving += (s, json) =>
            {
                if (activePlayer != null && activePlayer.IsMaximized)
                {
                    json["volume"] = activePlayer.Volume;
                    json["titleBarVisible"] = activePlayer.IsTitleBarVisible;
                    json["graphMode"] = activePlayer.PrefferedGraphMode.ToString();
                }
                else
                {
                    json["volume"] = config.lastVolume;
                    json["graphMode"] = config.graphMode.ToString();
                }

                json["cycleCurrentTrack"] = config.playlistCycleCurrentTrack;
              
                if (WindowState == FormWindowState.Normal)
                {
                    json["mainLeft"] = Left;
                    json["mainTop"] = Top;
                    json["mainWidth"] = Width;
                    json["mainHeight"] = Height;
                }
                else
                {
                    json["mainLeft"] = oldPos.X;
                    json["mainTop"] = oldPos.Y;
                    json["mainWidth"] = oldSize.Width;
                    json["mainHeight"] = oldSize.Height;
                }
            };

            config.Loading += (s, json) =>
            {
                JToken jt = json.Value<JToken>("volume");
                if (jt != null)
                {
                    config.lastVolume = Clamp(jt.Value<int>(), 0, 100);
                }

                jt = json.Value<JToken>("titleBarVisible");
                if (jt != null)
                {
                    config.titleBarVisible = jt.Value<bool>();
                }

                jt = json.Value<JToken>("cycleCurrentTrack");
                if (jt != null)
                {
                    config.playlistCycleCurrentTrack = jt.Value<bool>();
                }

                jt = json.Value<JToken>("mainLeft");
                if (jt != null)
                {
                    Left = jt.Value<int>();
                }
                jt = json.Value<JToken>("mainTop");
                if (jt != null)
                {
                    Top = jt.Value<int>();
                }
                jt = json.Value<JToken>("mainWidth");
                if (jt != null)
                {
                    Width = jt.Value<int>();
                }
                jt = json.Value<JToken>("mainHeight");
                if (jt != null)
                {
                    Height = jt.Value<int>();
                }
            };

            if (!string.IsNullOrEmpty(config.keyboardConfigFileName) && !string.IsNullOrWhiteSpace(config.keyboardConfigFileName) &&
                File.Exists(config.keyboardConfigFileName))
            {
                if (!keyBindings.LoadFromJson(config.keyboardConfigFileName))
                {
                    File.Delete(config.keyboardConfigFileName);
                }
            }
            else
            {
                keyBindings.SetDefaults();
            }

            ListAudioRenderers(audioOutputMonikers);

            PlayerCreated += OnPlayerCreated;

            formSettings = new FormSettings();
            formPlaylist = new FormPlaylist();
            formLog = new FormLog();

            config.Load();
            if (!this.IsOnScreen())
            {
                this.Center(Screen.PrimaryScreen.WorkingArea);
            }
            oldPos = new Point(Left, Top);
            oldSize = new Size(Width, Height);

            ZeratoolPlayerGui z = CreatePlayer(this, true);
            z.Volume = config.lastVolume;
            z.IsTitleBarVisible = config.titleBarVisible;
            z.PrefferedGraphMode = config.graphMode;
            z.GraphMode = config.graphMode;
            z.Activate();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.ApplicationExitCall)
            {
                config.Save();
                Hide();
                foreach (ZeratoolPlayerGui z in players)
                {
                    z.Stop();
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (e.CloseReason != CloseReason.ApplicationExitCall)
            {
                if (activePlayer != null && activePlayer.IsMaximized)
                {
                    if (File.Exists(config.playlistFileName))
                    {
                        File.Delete(config.playlistFileName);
                    }
                    if (activePlayer.Playlist.Count > 0)
                    {
                        activePlayer.Playlist.SaveToFile(config.playlistFileName);
                    }
                }

                foreach (ZeratoolPlayerGui z in players)
                {
                    z.Dispose();
                }
                players.Clear();
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (firstShown)
            {
                firstShown = false;
                if (activePlayer != null)
                {
                    string[] args = Environment.GetCommandLineArgs();
                    if (args.Length > 1)
                    {
                        activePlayer.PlayFile(args[1]);
                    }
                    else if (!string.IsNullOrEmpty(config.playlistFileName) && File.Exists(config.playlistFileName))
                    {
                        activePlayer.Playlist.LoadFromFile(config.playlistFileName);
                    }
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                case Keys.Enter:
                    Close();
                    return;
            }

            if (activePlayer != null)
            {
                PlayerHandleKeyboard(activePlayer, e);
            }
        }

        private void PlayerHandleKeyboard(ZeratoolPlayerGui controlledPlayer, KeyEventArgs e)
        {
            KeyboardShortcut keyboardShortcut = keyBindings.FindShortcut(e.KeyData);
            if (keyboardShortcut != null && keyboardShortcut.ShortcutAction != KeyboardShortcutAction.None)
            {
                switch (keyboardShortcut.ShortcutAction)
                {
                    case KeyboardShortcutAction.PlayPauseToggle:
                        controlledPlayer.TogglePlayPause();
                        break;

                    case KeyboardShortcutAction.SeekForward:
                        {
                            double step = 3.0;
                            if (controlledPlayer.TrackDuration - controlledPlayer.TrackPosition > step)
                            {
                                controlledPlayer.Seek(step);
                            }
                            break;
                        }

                    case KeyboardShortcutAction.SeekBackward:
                        controlledPlayer.Seek(-3.0);
                        break;

                    case KeyboardShortcutAction.JumpForward:
                        {
                            double step = 10.0;
                            if (controlledPlayer.TrackDuration - controlledPlayer.TrackPosition > step)
                            {
                                controlledPlayer.Seek(step);
                            }
                            break;
                        }

                    case KeyboardShortcutAction.JumpBackward:
                        controlledPlayer.Seek(-10.0);
                        break;

                    case KeyboardShortcutAction.SeekToBeginning:
                        controlledPlayer.TrackPosition = 0.0;
                        break;

                    case KeyboardShortcutAction.VolumeUp:
                        controlledPlayer.Volume += 5;
                        break;

                    case KeyboardShortcutAction.VolumeDown:
                        controlledPlayer.Volume -= 5;
                        break;

                    case KeyboardShortcutAction.FullscreenToggle:
                        controlledPlayer.ToggleFullscreenMode();
                        break;

                    case KeyboardShortcutAction.ControlPanelVisibilityToggle:
                        controlledPlayer.IsControlsVisible = !controlledPlayer.IsControlsVisible;
                        controlledPlayer.ResizeOutputWindow();
                        break;

                    case KeyboardShortcutAction.RebuildGraph:
                        {
                            double pos = controlledPlayer.TrackPosition;
                            controlledPlayer.Clear();
                            if (controlledPlayer.Play() == S_OK && pos > 0.0)
                            {
                                controlledPlayer.TrackPosition = pos;
                            }
                            break;
                        }
                }
            }
        }

        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int x = Clamp(e.X, 0, Width - ZeratoolPlayerGui.MIN_WIDTH - 16);
            int y = Clamp(e.Y, 0, Height - ZeratoolPlayerGui.MIN_HEIGHT - 36);
            ZeratoolPlayerGui z = CreatePlayer(this, x, y, ZeratoolPlayerGui.MIN_WIDTH, ZeratoolPlayerGui.MIN_HEIGHT, false);
            z.Activate();
        }

        private void OnPlayerCreated(ZeratoolPlayerGui z, bool isMaximizedToParent)
        {
            z.FilesDropped += OnPlayerFilesDropped;

            z.Activated += (s) =>
            {
                ZeratoolPlayerGui playerGui = s as ZeratoolPlayerGui;
                if (activePlayer != playerGui)
                {
                    activePlayer = playerGui;
                    foreach (ZeratoolPlayerGui zeratoolPlayerGui in players)
                    {
                        if (zeratoolPlayerGui != activePlayer)
                        {
                            zeratoolPlayerGui.SetTitleBarBackColor(COLOR_INACTIVE);
                        }
                    }
                    activePlayer.SetTitleBarBackColor(COLOR_ACTIVE);
                    activePlayer.BringToFront();
                    Text = $"{activePlayer.Title} | {APP_TITLE}";
                }
            };

            z.Closing += (s) =>
            {
                ZeratoolPlayerGui playerGui = s as ZeratoolPlayerGui;
                playerGui.Clear();
                players.Remove(playerGui);
                if (playerGui == activePlayer)
                {
                    if (players.Count > 0)
                    {
                        players[0].Activate();
                    }
                    else
                    {
                        activePlayer = null;
                    }
                }
                System.Diagnostics.Debug.WriteLine($"Player {playerGui.Title} closed");
                if (playerGui.IsMaximized)
                {
                    Close();
                }
            };

            z.MinMax += (object sender, ref bool isMaximized) =>
            {
                ZeratoolPlayerGui playerGui = sender as ZeratoolPlayerGui;
                if (isMaximized)
                {
                    playerGui.Location = new Point(0, 0);
                    playerGui.Size = new Size(Width - 16, Height - 36);
                }
                else
                {
                    playerGui.Size = new Size(ZeratoolPlayerGui.MIN_WIDTH, ZeratoolPlayerGui.MIN_HEIGHT);
                }
            };

            z.TrackFinished += (s) =>
            {
                ZeratoolPlayerGui playerGui = s as ZeratoolPlayerGui;
                if (config.playlistCycleCurrentTrack)
                {
                    playerGui.TrackPosition = 0;
                    return;
                }

                if (playerGui.Playlist.PlayingIndex < playerGui.Playlist.Count - 1)
                {
                    playerGui.Play(playerGui.Playlist.PlayingIndex + 1);
                }
                else
                {
                    playerGui.Pause();
                    System.Diagnostics.Debug.WriteLine("Playlist: No more files.");
                }
            };

            z.ActionTriggered += (object s, PlayerAction action, int errorCode) =>
            {
                System.Diagnostics.Debug.WriteLine($"Main form received player action: {action}");
                ZeratoolPlayerGui playerGui = s as ZeratoolPlayerGui;
                switch (action)
                {
                    case PlayerAction.Play:
                        if (errorCode != S_OK)
                        {
                            ShowErrorMessage(playerGui, errorCode);
                        }
                        break;

                    case PlayerAction.OpenFile:
                        OpenFileDialog ofd = new OpenFileDialog();
                        ofd.Title = "Select a file to play with it";
                        ofd.InitialDirectory = Path.GetDirectoryName(Application.StartupPath);
                        ofd.Filter = "Video files|*.avi;*.mpg;*.mpeg;*.ts;*.mp4;*.mkv;*.webm";
                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            playerGui.PlayFile(ofd.FileName);
                        }
                        ofd.Dispose();
                        break;

                    case PlayerAction.OpenSettings:
                        if (!formSettings.Visible)
                        {
                            formSettings.Show();
                        }
                        formSettings.BringToFront();
                        break;

                    case PlayerAction.OpenPlaylist:
                        if (!formPlaylist.Visible)
                        {
                            formPlaylist.Show();
                        }
                        formPlaylist.BringToFront();                         
                        break;

                    case PlayerAction.OpenLog:
                        if (!formLog.Visible)
                        {
                            formLog.Show();
                        }
                        formLog.BringToFront();
                        break;
                    
                    case PlayerAction.Fullscreen:
                        if (playerGui.IsMaximized)
                        {
                            if (playerGui.IsFullscreen)
                            {
                                FormBorderStyle = FormBorderStyle.None;
                                WindowState = FormWindowState.Maximized;
                                playerGui.IsTitleBarVisible = false;
                                playerGui.IsControlsVisible = false;
                            }
                            else
                            {
                                FormBorderStyle = FormBorderStyle.Sizable;
                                WindowState = FormWindowState.Normal;
                                playerGui.IsTitleBarVisible = true;
                                playerGui.IsControlsVisible = true;
                            }
                            playerGui.ResizeOutputWindow();
                        }
                        break;
                }
            };

            z.TitleChanged += (s, title) =>
            {
                ZeratoolPlayerGui playerGui = s as ZeratoolPlayerGui;
                if (playerGui == activePlayer)
                {
                    Text = $"{title} | {APP_TITLE}";
                }
            };

            z.VolumeChanged += (s) =>
            {
                ZeratoolPlayerGui playerGui = s as ZeratoolPlayerGui;
                if (playerGui.IsMaximized)
                {
                    config.lastVolume = playerGui.Volume;
                }
            };

            z.TrackRendered += (s, errorCode) =>
            {
                ZeratoolPlayerGui playerGui = s as ZeratoolPlayerGui;
                if (playerGui == activePlayer)
                {
                    Text = $"{playerGui.Title} | {APP_TITLE}";
                }
            };

            z.PlayerEngine.filters.audioRenderers.Clear();
            foreach (MonikerItem monikerItem in audioOutputMonikers)
            {
                z.PlayerEngine.filters.audioRenderers.Add(monikerItem);
            }
            z.PlayerEngine.filters.LoadFromJsonFile(config.filtersConfigFileName);

            if (isMaximizedToParent)
            {
                z.Maximize();
            }

            players.Add(z);
        }

        private void OnPlayerFilesDropped(object sender, List<string> droppedFiles)
        {
            ZeratoolPlayerGui z = sender as ZeratoolPlayerGui;
            List<string> playableFiles = GetPlayableFiles(droppedFiles);
            if (playableFiles.Count > 0)
            {
                z.Stop();
                z.Playlist.Clear();
                z.Playlist.AddRange(playableFiles);
                z.Play(0);
            }
            else
            {
                MessageBox.Show("Видео-файлы не найдены!", APP_TITLE,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            z.Activate();
        }
    }
}
