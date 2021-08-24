using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using static Zeratool_player_C_Sharp.DirectShowUtils;
using Newtonsoft.Json.Linq;

namespace Zeratool_player_C_Sharp
{
    public static class Utils
    {
        public class MyConfiguration
        {
            public string configFileName;
            public string selfPath;
            public int lastVolume;

            public delegate void SavingDelegate(object sender, JObject root, ref bool canceled);
            public delegate void LoadingDelegate(object sender, JObject root, ref bool canceled);
            public SavingDelegate Saving;
            public LoadingDelegate Loading;

            public MyConfiguration(string fileName)
            {
                configFileName = fileName;
                selfPath = Path.GetDirectoryName(Application.ExecutablePath);
                LoadDefault();
            }

            public void Save()
            {
                if (File.Exists(configFileName))
                {
                    File.Delete(configFileName);
                }
                JObject json = new JObject();
                bool canceled = false;
                Saving?.Invoke(this, json, ref canceled);
                if (!canceled)
                {
                    File.WriteAllText(configFileName, json.ToString());
                }
            }

            public void LoadDefault()
            {
                lastVolume = 25;
            }

            public void Load()
            {
                if (File.Exists(configFileName))
                {
                    JObject json = JObject.Parse(File.ReadAllText(configFileName));
                    if (json != null)
                    {
                        bool canceled = false;
                        Loading?.Invoke(this, json, ref canceled);
                    }
                }
            }
        }

        public class PlayerListItem
        {
            private string _displayName;
            private ZeratoolPlayerGui _playerObject;

            public string DisplayName => _displayName;
            public ZeratoolPlayerGui Player => _playerObject;

            public PlayerListItem(ZeratoolPlayerGui playerGuiObject, string displayName)
            {
                _playerObject = playerGuiObject;
                _displayName = displayName;
            }

            public override string ToString()
            {
                return DisplayName;
            }
        }


        public delegate void PlayerCreatedDelegate(ZeratoolPlayerGui playerGui, bool isMaximized);
        public static PlayerCreatedDelegate PlayerCreated;

        public static FormSettings formSettings;
        public static FormPlaylist formPlaylist;

        public static readonly List<ZeratoolPlayerGui> players = new List<ZeratoolPlayerGui>();
        public static ZeratoolPlayerGui activePlayer = null;
        public static List<MonikerItem> audioOutputMonikers = new List<MonikerItem>();
        public static List<string> videoFileTypes = new List<string>() { ".avi", ".mpg", ".mpeg", ".ts", ".mp4", ".mkv", ".webm" };

        public static string playlistFileName;

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
            string ext = Path.GetExtension(fn).ToLower();
            return videoFileTypes.Contains(ext);
        }

    }
}
