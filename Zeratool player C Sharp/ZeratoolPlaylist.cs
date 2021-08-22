using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Zeratool_player_C_Sharp
{
    public class ZeratoolPlaylist
    {
        private readonly List<string> items = new List<string>();
        private int _playingIndex = -1;
        private ZeratoolPlayerEngine _playerEngine;
        public ZeratoolPlayerEngine PlayerEngine => _playerEngine;

        public int Count => items.Count;
        public int PlayingIndex { get { return _playingIndex; } }

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
            _playerEngine = playerEngine;
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
            _playingIndex = -1;
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
        
        public void NextTrack()
        {
            int id = PlayingIndex + 1;
            if (id < Count)
            {
                PlayFile(id);

                _playingIndex = id;
                IndexChanged?.Invoke(this, id);
            }
        }

        public void PreviousTrack()
        {
            int id = PlayingIndex - 1;
            if (id >= 0)
            {
                PlayFile(id);
                _playingIndex = id;
                IndexChanged?.Invoke(this, id);
            }
        }

        public void PlayFile(int index)
        {
            _playerEngine.Clear();
            _playingIndex = index;
            IndexChanged?.Invoke(this, _playingIndex);
            _playerEngine.FileName = items[index];
            _playerEngine.Play();
        }

        public void SetIndex(string fileName)
        {
            int id = items.IndexOf(fileName);
            if (_playingIndex != id)
            {
                _playingIndex = id;
                IndexChanged?.Invoke(this, _playingIndex);
            }
        }

        public void SetIndex(int index)
        {
            if (_playingIndex != index)
            {
                _playingIndex = index;
                IndexChanged?.Invoke(this, _playingIndex);
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
            json["list"] = jArray;
            json["index"] = PlayingIndex;
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
                        _playingIndex = (id >= Count || id < 0) ? 0 : id;

                    }
                }
                else
                {
                    _playingIndex = -1;
                }
                
            }
            IndexChanged?.Invoke(this, _playingIndex);
        }

        public override string ToString()
        {
            return Count == 0 ? "<Пусто>" : items.ToText();
        }
    }
}
