using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using Shell32;
using Newtonsoft.Json.Linq;
using static Zeratool_player_C_Sharp.ZeratoolPlayerEngine;

namespace Zeratool_player_C_Sharp
{
    public static class Utils
    {
        public delegate void PlayerCreatedDelegate(ZeratoolPlayerGui playerGui, bool isMaximized);
        public static PlayerCreatedDelegate PlayerCreated;

        public static FormSettings formSettings;
        public static FormPlaylist formPlaylist;
        public static FormLog formLog;
        public static FormBookmarks formBookmarks;

        public static readonly List<ZeratoolPlayerGui> players = new List<ZeratoolPlayerGui>();
        public static ZeratoolPlayerGui activePlayer = null;
        public static List<MonikerItem> audioOutputMonikers = new List<MonikerItem>();
        public static List<string> videoFileTypes = new List<string>() { ".avi", ".mpg", ".mpeg", ".ts", ".mp4", ".mkv", ".webm" };
        public static readonly KeyBindings keyBindings = new KeyBindings();
        public static Timestamps timestamps = null;

        public static MainConfiguration config;

        public const string APP_TITLE = "Zeratool player";

        public static ZeratoolPlayerGui CreatePlayer(Control parentControl, bool maximized)
        {
            return CreatePlayer(parentControl, 0, 0, ZeratoolPlayerGui.MIN_WIDTH, ZeratoolPlayerGui.MIN_HEIGHT, maximized);
        }

        public static ZeratoolPlayerGui CreatePlayer(Control parentControl, int x, int y, int w, int h, bool maximized)
        {
            ZeratoolPlayerGui z = new ZeratoolPlayerGui();
            z.Location = new Point(x, y);
            z.Size = new Size(w, h);
            z.Parent = parentControl;

            PlayerCreated?.Invoke(z, maximized);

            return z;
        }

        public static Rectangle ResizeRect(Rectangle source, Size newSize)
        {
            float aspectSource = source.Height / (float)source.Width;
            float aspectDest = newSize.Height / (float)newSize.Width;
            int w = newSize.Width;
            int h = newSize.Height;
            if (aspectSource > aspectDest)
            {
                w = (int)(newSize.Height / aspectSource);
            }
            else if (aspectSource < aspectDest)
            {
                h = (int)(newSize.Width * aspectSource);
            }
            return new Rectangle(0, 0, w, h);
        }

        public static Rectangle CenterRect(Rectangle source, Rectangle dest)
        {
            int x = dest.Width / 2 - source.Width / 2;
            int y = dest.Height / 2 - source.Height / 2;
            return new Rectangle(x, y, source.Width, source.Height);
        }

        public static int Clamp(int x, int min, int max)
        {
            if (x < min)
            {
                x = min;
            }
            else if (x > max)
            {
                x = max;
            }
            return x;
        }

        public static double Clamp(double x, double min, double max)
        {
            if (x < min)
            {
                x = min;
            }
            else if (x > max)
            {
                x = max;
            }
            return x;
        }

        public static void SetDoubleBuffered(Control control, bool enabled)
        {
            typeof(Control).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, control, new object[] { enabled });
        }

        public static int FindPlayerInComboBox(ComboBox comboBox, ZeratoolPlayerGui z)
        {
            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                if ((comboBox.Items[i] as PlayerListItem).Player == z)
                {
                    return i;
                }
            }
            return -1;
        }

        public static string GetShortcutTarget(string shortcutFileName)
        {
            if (string.IsNullOrEmpty(shortcutFileName) || string.IsNullOrWhiteSpace(shortcutFileName) ||
                !shortcutFileName.ToLower().EndsWith(".lnk"))
            {
                return null;
            }

            //using Shell32
            try
            {
                Shell shell = new Shell();

                string shortcut_path = shortcutFileName.Substring(0, shortcutFileName.LastIndexOf("\\"));
                string shortcut_name = shortcutFileName.Substring(shortcutFileName.LastIndexOf("\\") + 1);

                Folder shortcut_folder = shell.NameSpace(shortcut_path);
                FolderItem folder_item = shortcut_folder.Items().Item(shortcut_name);
                ShellLinkObject lnk = (ShellLinkObject)folder_item.GetLink;

                string target = lnk.Path;
                return target;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public static List<string> GetPlayableFiles(IEnumerable<string> collection)
        {
            List<string> files = new List<string>();
            foreach (string str in collection)
            {
                if (File.Exists(str))
                {
                    files.Add(str);
                }
                else if (Directory.Exists(str))
                {
                    string[] dirFiles = Directory.GetFiles(str);
                    if (dirFiles.Length > 0)
                    {
                        files.AddRange(dirFiles);
                    }
                }
            }

            List<string> resList = new List<string>();
            foreach (string fileName in files)
            {
                string fn = fileName;
                string ext = Path.GetExtension(fn);
                if (!string.IsNullOrEmpty(ext))
                {
                    if (ext.ToLower() == ".lnk")
                    {
                        fn = GetShortcutTarget(fn);
                    }
                    if (!string.IsNullOrEmpty(fn) && IsPlayableFile(fn) && File.Exists(fn))
                    {
                        resList.Add(fn);
                    }
                }
            }
            return resList;
        }

        public static bool IsPlayableFile(string fn)
        {
            string ext = Path.GetExtension(fn);
            return !string.IsNullOrEmpty(ext) && videoFileTypes.Contains(ext.ToLower());
        }

        public static ZeratoolPlayerGui GetPlayerFromComboBox(ComboBox comboBox)
        {
            if (comboBox.SelectedIndex < 0)
            {
                return null;
            }
            PlayerListItem playerListItem = (PlayerListItem)comboBox.Items[comboBox.SelectedIndex];
            if (playerListItem == null)
            {
                return null;
            }
            return playerListItem.Player;
        }

        public static void ShowErrorMessage(ZeratoolPlayerGui playerGui, int errorCode)
        {
            switch (errorCode)
            {
                case ERROR_FILE_NAME_NOT_DEFINED:
                    MessageBox.Show("Не указано имя файла!", APP_TITLE,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                case ERROR_FILE_NOT_FOUND:
                    MessageBox.Show($"Файл не найден!\n{playerGui.FileName}", APP_TITLE,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                case ERROR_NOTHING_RENDERED:
                    MessageBox.Show($"Не удалось отрендерить файл!\n{playerGui.FileName}", APP_TITLE,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                default:
                    MessageBox.Show($"Ошибка {ZeratoolPlayerEngine.ErrorCodeToString(errorCode)}", APP_TITLE,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }
    }

    public sealed class MainConfiguration
    {
        public string configFileName;
        public string keyboardConfigFileName;
        public string selfPath;
        public string filtersConfigFileName;
        public int lastVolume;
        public bool titleBarVisible;
        public string playlistFileName;
        public string bookmarksFileName;
        public string timestampsFileName;
        public bool playlistCycleCurrentTrack;
        public DirectShowGraphMode graphMode;

        public delegate void SavingDelegate(object sender, JObject root);
        public delegate void LoadingDelegate(object sender, JObject root);
        public SavingDelegate Saving;
        public LoadingDelegate Loading;

        public MainConfiguration(string fileName)
        {
            configFileName = fileName;
            selfPath = Path.GetDirectoryName(Application.ExecutablePath);

            LoadDefaults();
        }

        public void Save()
        {
            if (File.Exists(configFileName))
            {
                File.Delete(configFileName);
            }
            JObject json = new JObject();
            Saving?.Invoke(this, json);
            File.WriteAllText(configFileName, json.ToString());
        }

        public void LoadDefaults()
        {
            lastVolume = 25;
            titleBarVisible = true;
            playlistCycleCurrentTrack = false;
            graphMode = DirectShowGraphMode.Manual;
            playlistFileName = selfPath + "\\LastPlaylist.json";
            filtersConfigFileName = selfPath + "\\filters.json";
            keyboardConfigFileName = selfPath + "\\keyboard.json";
            bookmarksFileName = selfPath + "\\bookmarks.json";
            timestampsFileName = selfPath + "\\timestamps.json";
        }

        public void Load()
        {
            if (File.Exists(configFileName))
            {
                JObject json = JObject.Parse(File.ReadAllText(configFileName));
                if (json != null)
                {
                    Loading?.Invoke(this, json);
                }
            }
        }
    }

    public sealed class PlayerListItem
    {
        public string DisplayName { get; private set; }
        public ZeratoolPlayerGui Player { get; private set; }

        public PlayerListItem(ZeratoolPlayerGui playerGuiObject, string displayName)
        {
            Player = playerGuiObject;
            DisplayName = displayName;
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
