using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace Zeratool_player_C_Sharp
{
    public static class Utils
    {
        public static List<string> videoFileTypes = new List<string>() { ".avi", ".mpg", ".mpeg", ".ts", ".mp4", ".mkv", ".webm" };

        public static Rectangle ResizeRect(Rectangle source, Size newSize)
        {
            float aspectSource = source.Height / (float)source.Width;
            float aspectDest = newSize.Height / (float)newSize.Width;
            int w = newSize.Width;
            int h = newSize.Height;
            if (aspectSource > aspectDest)
                w = (int)(newSize.Height / aspectSource);
            else if (aspectSource < aspectDest)
                h = (int)(newSize.Width * aspectSource);
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
