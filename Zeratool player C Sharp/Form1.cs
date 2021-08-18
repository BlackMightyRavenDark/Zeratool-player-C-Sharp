using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static Zeratool_player_C_Sharp.Utils;
using static Zeratool_player_C_Sharp.ZeratoolPlayerEngine;
using static Zeratool_player_C_Sharp.ZeratoolPlayerGui;
using static Zeratool_player_C_Sharp.DirectShowUtils;
using System.IO;

namespace Zeratool_player_C_Sharp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            player.DropFiles += OnPlayerDropFiles;

            player.MinMax += (object s, ref bool isMaximized) =>
            {
                ZeratoolPlayerGui playerGui = s as ZeratoolPlayerGui;
                if (isMaximized)
                {
                    playerGui.Location = new Point(0, 0);
                    playerGui.Size = new Size(Width - 16, Height - 38);
                }
                else
                {
                    playerGui.Size = new Size(ZeratoolPlayerGui.MIN_WIDTH, ZeratoolPlayerGui.MIN_HEIGHT);
                }
            };

            player.TrackFinished += (s) =>
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

            player.ActionTriggered += (object s, PLAYER_ACTION action, int errorCode) =>
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
                        ofd.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
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
                        string playlist = (s as ZeratoolPlayerGui).Playlist.List.ToText();
                        MessageBox.Show(playlist, "Плейлист", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Видео-файлы не найдены!");
            }
            z.Activate();
        }

        private void ShowError(ZeratoolPlayerGui playerGui, int errorCode)
        {
            switch (errorCode)
            {
                case ERROR_FILE_NAME_NOT_DEFINED:
                    MessageBox.Show("Не указано имя файла!", "Zeratool player",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                case ERROR_FILE_NOT_FOUND:
                    MessageBox.Show($"Файл не найден!\n{playerGui.FileName}", "Zeratool player",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                case ERROR_NOTHING_RENDERED:
                    MessageBox.Show($"Не удалось отрендерить файл!\n{playerGui.FileName}", "Zeratool player",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                default:
                    MessageBox.Show($"Ошибка {ZeratoolPlayerEngine.ErrorCodeToString(errorCode)}", "Zeratool player",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }
    }
}
