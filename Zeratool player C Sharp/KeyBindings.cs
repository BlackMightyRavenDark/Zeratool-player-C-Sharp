using System.Collections.Generic;
using System.Windows.Forms;

namespace Zeratool_player_C_Sharp
{
    public sealed class KeyBindings
    {
        public static readonly KeysConverter keysConverter = new KeysConverter();
        public readonly List<KeyboardShortcut> keyboardShortcuts = new List<KeyboardShortcut>();

        public KeyBindings()
        {
            SetDefaults();
        }

        public void SetDefaults()
        {
            keyboardShortcuts.Clear();

            keyboardShortcuts.Add(new KeyboardShortcut(Keys.Left, KeyboardShortcutAction.SeekBackward, "Перемотка назад"));
            keyboardShortcuts.Add(new KeyboardShortcut(Keys.Right, KeyboardShortcutAction.SeekForward, "Перемотка вперёд"));
        }

        public KeyboardShortcutAction FindShortcutAction(Keys keys)
        {
            foreach (KeyboardShortcut ks in keyboardShortcuts)
            {
                if (ks.Keys == keys)
                {
                    return ks.ShortcutAction;
                }
            }
            return KeyboardShortcutAction.None;
        }
    }

    public enum KeyboardShortcutAction
    {
        None, //no action
        SeekForward,
        SeekBackward
    }

    public sealed class KeyboardShortcut
    {
        public Keys Keys { get; private set; }
        public string Title { get; private set; }
        public KeyboardShortcutAction ShortcutAction { get; private set; }

        public KeyboardShortcut(Keys keys, KeyboardShortcutAction action, string title)
        {
            Keys = keys;
            ShortcutAction = action;
            Title = title;
        }

        public override string ToString()
        {
            return KeyBindings.keysConverter.ConvertToString(Keys);
        }
    }
}
