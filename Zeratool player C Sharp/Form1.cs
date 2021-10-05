using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using static Zeratool_player_C_Sharp.Utils;
using static Zeratool_player_C_Sharp.ZeratoolPlayerEngine;
using static Zeratool_player_C_Sharp.ZeratoolPlayerGui;
using static Zeratool_player_C_Sharp.DirectShowUtils;

namespace Zeratool_player_C_Sharp
{
    public partial class Form1 : Form
    {
        public const string TITLE = "Zeratool player";
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
            ListAudioRenderers(audioOutputMonikers);

            PlayerCreated += OnPlayerCreated;

            config = new MainConfiguration(Application.StartupPath + "\\zeratool.json");
            config.Saving += (s, json) =>
            {
                if (activePlayer != null && activePlayer.IsMaximized)
                {
                    json["volume"] = activePlayer.Volume;
                    json["titleBarVisible"] = activePlayer.IsTitleBarVisible;
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
            z.Activate();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            config.Save();
            Hide();
            foreach (ZeratoolPlayerGui z in players)
            {
                z.Stop();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
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

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (firstShown)
            {
                firstShown = false;
                if (activePlayer != null)
                {
                    if (Environment.GetCommandLineArgs().Length > 1)
                    {
                        string fn = Environment.GetCommandLineArgs()[1];
                        activePlayer.PlayFile(fn);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(config.playlistFileName) && File.Exists(config.playlistFileName))
                        {
                            activePlayer.Playlist.LoadFromFile(config.playlistFileName);
                        }
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
            switch (e.KeyCode)
            {
                case Keys.Space:
                case Keys.Insert:
                    PlayerPlayPause(controlledPlayer);
                    break;

                case Keys.Left:
                    controlledPlayer.Seek(e.Modifiers == Keys.Shift ? -10.0 : -3.0);
                    break;

                case Keys.Right:
                    double step = e.Modifiers == Keys.Shift ? 10.0 : 3.0;
                    if (controlledPlayer.TrackDuration - controlledPlayer.TrackPosition > step)
                    {
                        controlledPlayer.Seek(step);
                    }
                    break;

                case Keys.Up:
                    controlledPlayer.Volume += 5;
                    break;

                case Keys.Down:
                    controlledPlayer.Volume -= 5;
                    break;

                case Keys.R:
                    double pos = controlledPlayer.TrackPosition;
                    controlledPlayer.Clear();
                    if (controlledPlayer.Play() == S_OK && pos > 0.0)
                    {
                        controlledPlayer.TrackPosition = pos;
                    }
                    break;

                case Keys.Delete:
                    controlledPlayer.IsControlsVisible = !controlledPlayer.IsControlsVisible;
                    controlledPlayer.ResizeOutputWindow();
                    break;

                case Keys.PageDown:
                    controlledPlayer.ToggleFullscreenMode();
                    break;
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
            z.DropFiles += OnPlayerDropFiles;

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
                    Text = $"{activePlayer.Title} | {TITLE}";
                }
            };

            z.Closing += (s) =>
            {
                ZeratoolPlayerGui playerGui = s as ZeratoolPlayerGui;
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
                if (playerGui.IsFullscreen)
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
                            ShowError(playerGui, errorCode);
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
                    Text = $"{title} | {TITLE}";
                }
            };

            z.TrackRendered += (s, errorCode) =>
            {
                ZeratoolPlayerGui playerGui = s as ZeratoolPlayerGui;
                if (playerGui == activePlayer)
                {
                    Text = $"{playerGui.Title} | {TITLE}";
                }
            };

            z.PlayerEngine.filters.audioRenderers.Clear();
            foreach (MonikerItem monikerItem in audioOutputMonikers)
            {
                z.PlayerEngine.filters.audioRenderers.Add(monikerItem);
            }
            z.PlayerEngine.filters.prefferedAudioRendererId = audioOutputMonikers.Count == 0 ? -1 : 0;
            z.PlayerEngine.filters.audioRendererId = z.PlayerEngine.filters.prefferedAudioRendererId;

            if (isMaximizedToParent)
            {
                z.Maximize();
            }

            players.Add(z);
        }

        private void OnPlayerDropFiles(object sender, List<string> droppedFiles)
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
                MessageBox.Show("Видео-файлы не найдены!", TITLE,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            z.Activate();
        }

        private int PlayerPlayPause(ZeratoolPlayerGui playerGui)
        {
            switch (playerGui.State)
            {
                case PlayerState.Null:
                case PlayerState.Paused:
                case PlayerState.Stopped:
                    return playerGui.Play();

                case PlayerState.Playing:
                    return playerGui.Pause() ? S_OK : S_FALSE;

                default:
                    return S_OK;
            }
        }

        private void ShowError(ZeratoolPlayerGui playerGui, int errorCode)
        {
            switch (errorCode)
            {
                case ERROR_FILE_NAME_NOT_DEFINED:
                    MessageBox.Show("Не указано имя файла!", TITLE,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                case ERROR_FILE_NOT_FOUND:
                    MessageBox.Show($"Файл не найден!\n{playerGui.FileName}", TITLE,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                case ERROR_NOTHING_RENDERED:
                    MessageBox.Show($"Не удалось отрендерить файл!\n{playerGui.FileName}", TITLE,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                default:
                    MessageBox.Show($"Ошибка {ZeratoolPlayerEngine.ErrorCodeToString(errorCode)}", TITLE,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }
    }
}
