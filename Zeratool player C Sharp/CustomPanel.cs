using System;
using System.Windows.Forms;

namespace Zeratool_player_C_Sharp
{
    public partial class CustomPanel : Panel
    {
        private DateTime lastDown = DateTime.MinValue;

        public delegate void MouseSingleDownDelegate(object sender, MouseEventArgs e);
        public delegate void MouseDoubleDownDelegate(object sender, MouseEventArgs e);
        public event MouseSingleDownDelegate MouseSingleDown;
        public event MouseDoubleDownDelegate MouseDoubleDown;

        public CustomPanel()
        {
            MouseDown += (s, e) =>
            {
                DateTime now = DateTime.Now;

                MouseSingleDown?.Invoke(this, e);
                if (MouseDoubleDown != null && (now - lastDown).Ticks < TimeSpan.FromMilliseconds(600).Ticks)
                {
                    //System.Diagnostics.Debug.WriteLine("double down");
                    MouseDoubleDown.Invoke(this, e);
                }
                lastDown = now;
            };
        }
    }
}
