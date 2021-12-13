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

            keyboardShortcuts.Add(new KeyboardShortcut(Keys.Space, KeyboardShortcutAction.PlayPauseToggle, "Плей / Пауза"));
            keyboardShortcuts.Add(new KeyboardShortcut(Keys.Left, KeyboardShortcutAction.SeekBackward, "Перемотка назад"));
            keyboardShortcuts.Add(new KeyboardShortcut(Keys.Right, KeyboardShortcutAction.SeekForward, "Перемотка вперёд"));
            keyboardShortcuts.Add(new KeyboardShortcut(Keys.Shift | Keys.Left, KeyboardShortcutAction.JumpBackward, "Прыжок назад"));
            keyboardShortcuts.Add(new KeyboardShortcut(Keys.Shift | Keys.Right, KeyboardShortcutAction.JumpForward, "Прыжок вперёд"));
            keyboardShortcuts.Add(new KeyboardShortcut(Keys.Up, KeyboardShortcutAction.VolumeUp, "Прибавить громкость"));
            keyboardShortcuts.Add(new KeyboardShortcut(Keys.Down, KeyboardShortcutAction.VolumeDown, "Убавить громкость"));
            keyboardShortcuts.Add(new KeyboardShortcut(Keys.PageDown, KeyboardShortcutAction.FullscreenToggle, "Полный экран"));
            keyboardShortcuts.Add(new KeyboardShortcut(Keys.Delete, KeyboardShortcutAction.ControlPanelVisibilityToggle, "Показать / скрыть панель управления"));
            keyboardShortcuts.Add(new KeyboardShortcut(Keys.R, KeyboardShortcutAction.RebuildGraph, "Перерендерить"));
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
        PlayPauseToggle,
        SeekForward,
        SeekBackward,
        JumpForward,
        JumpBackward,
        VolumeUp,
        VolumeDown,
        FullscreenToggle,
        ControlPanelVisibilityToggle,
        RebuildGraph
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
