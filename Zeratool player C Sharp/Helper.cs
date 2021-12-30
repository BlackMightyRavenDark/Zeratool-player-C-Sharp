using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Zeratool_player_C_Sharp
{
    public static class Helper
    {
        public static string ToText(this IEnumerable<string> list)
        {
            string s = string.Empty;
            foreach (string t in list)
            {
                s += t + "\n";
            }
            return s;
        }

        public static void DrawCircle(this Graphics graphics, Pen pen, int x, int y, int radius)
        {
            int halfRadius = radius / 2;
            graphics.DrawEllipse(pen, x - halfRadius, y - halfRadius, radius, radius);
        }

        public static void FillCircle(this Graphics graphics, Brush brush, int x, int y, int radius)
        {
            int halfRadius = radius / 2;
            graphics.FillEllipse(brush, x - halfRadius, y - halfRadius, radius, radius);
        }

        public static bool IsOnScreen(this Form form)
        {
            Screen[] screens = Screen.AllScreens;

            foreach (Screen screen in screens)
            {
                Rectangle formRectangle = new Rectangle(form.Left, form.Top, form.Width, form.Height);
                if (screen.WorkingArea.IntersectsWith(formRectangle))
                {
                    return true;
                }
            }

            return false;
        }

        public static void Center(this Form form, Rectangle rectangle)
        {
            int x = rectangle.Width / 2 - form.Width / 2;
            int y = rectangle.Height / 2 - form.Height / 2;
            form.Location = new Point(x, y);
        }
    }
}
