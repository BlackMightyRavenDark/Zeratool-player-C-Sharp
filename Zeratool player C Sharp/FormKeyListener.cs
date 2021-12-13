using System.Windows.Forms;

namespace Zeratool_player_C_Sharp
{
    public partial class FormKeyListener : Form
    {
        public Keys ResultKeys { get; private set; } = Keys.None;

        public FormKeyListener(KeyboardShortcut keyboardShortcut)
        {
            InitializeComponent();

            lblKeyActionTitle.Text = $"Действие: {keyboardShortcut.Title}";
            KeysConverter keysConverter = new KeysConverter();
            lblKeyCombination.Text = $"Текущая клавиша: {keysConverter.ConvertToString(keyboardShortcut.Keys)}";
        }

        private void FormKeyListener_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }
            bool modifierKeyPressed = e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.ShiftKey ||
                e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin;
            if (modifierKeyPressed || e.KeyCode == Keys.Return || e.KeyCode == Keys.Menu ||
                e.KeyCode == Keys.Capital || e.KeyCode == Keys.Scroll || e.KeyCode == Keys.NumLock)
            {
                return;
            }

            if (!modifierKeyPressed)
            {
                ResultKeys = e.KeyData;
                KeysConverter keysConverter = new KeysConverter();
                lblKeyCombination.Text = keysConverter.ConvertToString(e.KeyData);

                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
