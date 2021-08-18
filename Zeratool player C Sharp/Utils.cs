using System.Drawing;

namespace Zeratool_player_C_Sharp
{
    public static class Utils
    {
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

    }
}
