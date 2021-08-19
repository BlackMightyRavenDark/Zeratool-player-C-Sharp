using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static Zeratool_player_C_Sharp.Utils;
using static Zeratool_player_C_Sharp.ZeratoolPlayerEngine;
using static Zeratool_player_C_Sharp.ZeratoolPlayerGui;
using static Zeratool_player_C_Sharp.DirectShowUtils;

namespace Zeratool_player_C_Sharp
{
    public partial class Form1 : Form
    {
        public const string TITLE = "Zeratool player";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PlayerAdd += OnPlayerAdd;

            ZeratoolPlayerGui z = AddPlayer(this, true);
            z.Activate();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            foreach (ZeratoolPlayerGui z in players)
            {
                z.Stop();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (ZeratoolPlayerGui z in players)
            {
                z.Dispose();
            }
            players.Clear();
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
        }

        private void OnPlayerAdd(ZeratoolPlayerGui z, bool isMaximizedToParent)
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

            z.ActionTriggered += (object s, PLAYER_ACTION action, int errorCode) =>
            {
                System.Diagnostics.Debug.WriteLine($"Main form received player action: {action}");
                ZeratoolPlayerGui playerGui = s as ZeratoolPlayerGui;
                switch (action)
                {
                    case PLAYER_ACTION.Play:
                        if (errorCode != S_OK)
                        {
                            ShowError(playerGui, errorCode);
                        }
                        break;

                    case PLAYER_ACTION.OpenFile:
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

                    case PLAYER_ACTION.OpenSettings:
                        MessageBox.Show("Настроек пока не существует!", "Ошибка!",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;

                    case PLAYER_ACTION.OpenPlaylist:
                        MessageBox.Show(playerGui.Playlist.ToString(), "Плейлист", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    
                    case PLAYER_ACTION.Fullscreen:
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

        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int x = Clamp(e.X, 0, Width - ZeratoolPlayerGui.MIN_WIDTH - 16);
            int y = Clamp(e.Y, 0, Height - ZeratoolPlayerGui.MIN_HEIGHT - 36);
            ZeratoolPlayerGui z = AddPlayer(this, x, y, ZeratoolPlayerGui.MIN_WIDTH, ZeratoolPlayerGui.MIN_HEIGHT, false);
            z.Activate();
        }

    }
}
