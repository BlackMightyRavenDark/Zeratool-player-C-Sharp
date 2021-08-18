using System.Collections.Generic;
using System.Drawing;

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

        public static void FillCircle(this Graphics graphics, Brush brush, int x, int y, int radius)
        {
            int halfRadius = radius / 2;
            graphics.FillEllipse(brush, x - halfRadius, y - halfRadius, radius, radius);
        }
    }
}
