using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace Zeratool_player_C_Sharp
{
    public sealed class KeyBindings
    {
        public static readonly KeysConverter keysConverter = new KeysConverter();
        public readonly List<KeyboardShortcut> keyboardShortcuts = new List<KeyboardShortcut>();

        public void SetDefaults()
        {
            keyboardShortcuts.Clear();

            keyboardShortcuts.Add(new KeyboardShortcut(Keys.Space, KeyboardShortcutAction.PlayPauseToggle, "Плей / Пауза"));
            keyboardShortcuts.Add(new KeyboardShortcut(Keys.Left, KeyboardShortcutAction.SeekBackward, "Перемотка назад"));
            keyboardShortcuts.Add(new KeyboardShortcut(Keys.Right, KeyboardShortcutAction.SeekForward, "Перемотка вперёд"));
            keyboardShortcuts.Add(new KeyboardShortcut(Keys.Shift | Keys.Left, KeyboardShortcutAction.JumpBackward, "Прыжок назад"));
            keyboardShortcuts.Add(new KeyboardShortcut(Keys.Shift | Keys.Right, KeyboardShortcutAction.JumpForward, "Прыжок вперёд"));
            keyboardShortcuts.Add(new KeyboardShortcut(Keys.Back, KeyboardShortcutAction.SeekToBeginning, "Перемотать в начало"));
            keyboardShortcuts.Add(new KeyboardShortcut(Keys.Up, KeyboardShortcutAction.VolumeUp, "Прибавить громкость"));
            keyboardShortcuts.Add(new KeyboardShortcut(Keys.Down, KeyboardShortcutAction.VolumeDown, "Убавить громкость"));
            keyboardShortcuts.Add(new KeyboardShortcut(Keys.PageDown, KeyboardShortcutAction.FullscreenToggle, "Полный экран"));
            keyboardShortcuts.Add(new KeyboardShortcut(Keys.Delete, KeyboardShortcutAction.ControlPanelVisibilityToggle, "Показать / скрыть панель управления"));
            keyboardShortcuts.Add(new KeyboardShortcut(Keys.R, KeyboardShortcutAction.RebuildGraph, "Перерендерить"));
        }


        public KeyboardShortcut FindShortcut(Keys keys)
        {
            foreach (KeyboardShortcut ks in keyboardShortcuts)
            {
                if (ks.Keys == keys)
                {
                    return ks;
                }
            }
            return null;
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

        public void SaveToJson(string fileName)
        {
            JArray jArray = new JArray();
            foreach (KeyboardShortcut ks in keyboardShortcuts)
            {
                JObject j = new JObject();
                j["title"] = ks.Title;
                j["action"] = ks.ShortcutAction.ToString();
                j["key"] = keysConverter.ConvertToString(ks);
                jArray.Add(j);
            }
            JObject json = new JObject();
            json.Add(new JProperty("keyList", jArray));
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            File.WriteAllText(fileName, json.ToString());
        }

        public bool LoadFromJson(string fileName)
        {
            keyboardShortcuts.Clear();
            try
            {
                JObject json = JObject.Parse(File.ReadAllText(fileName));
                JArray jArray = json.Value<JArray>("keyList");
                foreach (JObject j in jArray)
                {
                    string action = j.Value<string>("action");
                    if (Enum.TryParse(action, out KeyboardShortcutAction keyboardShortcutAction))
                    {
                        string title = j.Value<string>("title");
                        string key = j.Value<string>("key");
                        Keys keys = (Keys)keysConverter.ConvertFromString(key);
                        keyboardShortcuts.Add(new KeyboardShortcut(keys, keyboardShortcutAction, title));
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                SetDefaults();
                return false;
            }
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
        SeekToBeginning,
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
