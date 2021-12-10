using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Zeratool_player_C_Sharp
{
    public class ZeratoolPlaylist
    {
        private readonly List<string> items = new List<string>();

        public ZeratoolPlayerEngine PlayerEngine { get; private set; }
        public int Count => items.Count;
        public int PlayingIndex { get; private set; } = -1;

        public delegate void ItemAddedDelegate(object sender, string itemString);
        public delegate void ItemRemovedDelegate(object sender, int index, string itemString);
        public delegate void ClearedDelegate(object sender);
        public delegate void IndexChangedDelegate(object sender, int index);
        public ItemAddedDelegate ItemAdded;
        public ItemRemovedDelegate ItemRemoved;
        public ClearedDelegate Cleared;
        public IndexChangedDelegate IndexChanged; 

        public string this[int number]
        {
            get
            {
                if (Count == 0 || number < 0 || number >= Count)
                {
                    throw new ArgumentOutOfRangeException();
                }
                return items[number];
            }
        }

        public ZeratoolPlaylist(ZeratoolPlayerEngine playerEngine)
        {
            PlayerEngine = playerEngine;
        }

        public int Add(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException();
            }
            items.Add(fileName);
            ItemAdded?.Invoke(this, fileName);
            return Count - 1;
        }

        public int AddRange(IEnumerable<string> collection)
        {
            items.AddRange(collection);
            if (ItemAdded != null)
            {
                foreach (string str in collection)
                {
                    ItemAdded.Invoke(this, str);
                }
            }
            return Count - 1;
        }

        public void RemoveAt(int index)
        {
            if (Count == 0 || index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException();
            }
            string t = items[index];
            items.RemoveAt(index);
            ItemRemoved?.Invoke(this, index, t);
        }

        public void Clear()
        {
            items.Clear();
            PlayingIndex = -1;
            Cleared?.Invoke(this);
            IndexChanged?.Invoke(this, -1);
        }

        public string[] ToArray()
        {
            return items.ToArray();
        }

        public bool Contains(string t)
        {
            return items.Contains(t);
        }
        
        public int NextTrack()
        {
            int res = DirectShowUtils.S_OK;
            if (Count > 0 && PlayingIndex < Count - 1)
            {
                PlayingIndex++;
                if (PlayingIndex < 0)
                {
                    PlayingIndex = 0;
                }
                res = PlayFile(PlayingIndex);
                IndexChanged?.Invoke(this, PlayingIndex);
            }
            return res;
        }

        public int PreviousTrack()
        {
            int res = DirectShowUtils.S_OK;
            if (Count > 0 && PlayingIndex > 0)
            {
                PlayingIndex--;
                if (PlayingIndex >= Count)
                {
                    PlayingIndex = Count - 1;
                }
                res = PlayFile(PlayingIndex);
                IndexChanged?.Invoke(this, PlayingIndex);
            }
            return res;
        }

        public int PlayFile(int index)
        {
            PlayerEngine.Clear();
            PlayingIndex = index;
            IndexChanged?.Invoke(this, PlayingIndex);
            PlayerEngine.FileName = items[index];
            return PlayerEngine.Play();
        }

        public void SetIndex(string fileName)
        {
            int id = items.IndexOf(fileName);
            SetIndex(id);
        }

        public void SetIndex(int index)
        {
            if (PlayingIndex != index)
            {
                PlayingIndex = index;
                IndexChanged?.Invoke(this, PlayingIndex);
            }
        }

        public void SaveToFile(string fileName)
        {
            JArray jArray = new JArray();
            foreach (string str in items)
            {
                jArray.Add(str);
            }
            JObject json = new JObject();
            json["index"] = PlayingIndex;
            json["list"] = jArray;
            File.WriteAllText(fileName, json.ToString());
        }

        public void LoadFromFile(string fileName)
        {
            JObject json = JObject.Parse(File.ReadAllText(fileName));
            if (json != null)
            {
                JToken jt = json.Value<JToken>("list");
                if (jt != null)
                {
                    JArray jsonArr = jt.Value<JArray>();
                    if (jsonArr.Count > 0)
                    {
                        List<string> list = new List<string>();
                        foreach (JToken jToken in jsonArr)
                        {
                            list.Add(jToken.Value<string>());
                        }
                        if (list.Count > 0)
                        {
                            AddRange(list);
                        }
                    }
                }
                if (Count > 0)
                {
                    jt = json.Value<JToken>("index");
                    if (jt != null)
                    {
                        int id = jt.Value<int>();
                        PlayingIndex = (id >= Count || id < 0) ? 0 : id;
                    }
                }
                else
                {
                    PlayingIndex = -1;
                }
            }
            IndexChanged?.Invoke(this, PlayingIndex);
        }

        public override string ToString()
        {
            return Count == 0 ? "<Пусто>" : items.ToText();
        }
    }
}
