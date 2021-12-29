using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Zeratool_player_C_Sharp
{
    public class ZeratoolBookmarks
    {
        public ZeratoolPlayerGui Player { get; private set; }

        private readonly List<BookmarkItem> items = new List<BookmarkItem>();

        public int Count => items.Count;
        public BookmarkItem this[int number]
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

        public delegate void BookmarkAddedDelegate(object sender, BookmarkItem bookmarkItem);
        public delegate void BookmarkRemovedDelegate(object sender, int index);
        public delegate void BookmarkListClearedDelegate(object sender);
        public BookmarkAddedDelegate BookmarkAdded;
        public BookmarkRemovedDelegate BookmarkRemoved;
        public BookmarkListClearedDelegate BookmarkListCleared;

        public ZeratoolBookmarks(ZeratoolPlayerGui playerGui)
        {
            Player = playerGui;
        }

        public int Add(TimeSpan time, string shortDescription)
        {
            BookmarkItem bookmarkItem = new BookmarkItem(time, shortDescription);
            return Add(bookmarkItem);
        }

        public int Add(BookmarkItem bookmarkItem)
        {
            int id = IndexOf(bookmarkItem.TimeCode);
            if (id == -1)
            {
                items.Add(bookmarkItem);
                items.Sort(new BookmarkItemComparer());
                BookmarkAdded?.Invoke(this, bookmarkItem);
                id = IndexOf(bookmarkItem.TimeCode);
                return id;
            }
            return id;
        }

        public void AddRange(IEnumerable<BookmarkItem> bookmarkItems)
        {
            foreach (BookmarkItem item in bookmarkItems)
            {
                _ = Add(item);
            }
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new IndexOutOfRangeException();
            }

            items.RemoveAt(index);
            BookmarkRemoved?.Invoke(this, index);
        }

        public void Clear()
        {
            if (Count > 0)
            {
                items.Clear();
                BookmarkListCleared?.Invoke(this);
            }
        }

        public int IndexOf(TimeSpan time)
        {
            for (int i = 0; i < Count; i++)
            {
                if (items[i].TimeCode == time)
                {
                    return i;
                }
            }
            return -1;
        }

        public JArray ToJsonArray()
        {
            JArray jsonArray = new JArray();
            ToJsonArray(jsonArray);
            return jsonArray;
        }

        public void ToJsonArray(JArray jsonArray)
        {
            foreach (BookmarkItem item in items)
            {
                JObject j = new JObject();
                j["timeCode"] = TimeToString(new DateTime(item.TimeCode.Ticks));
                j["shortDescription"] = item.ShortDescription;
                jsonArray.Add(j);
            }
        }

        public bool SaveToJsonFile(string fileName)
        {
            bool fileExists = File.Exists(fileName);
            JObject globalBookmarkList;
            if (fileExists)
            {
                globalBookmarkList = LoadGlobalBookmarkListFromJsonFile(fileName);
            }
            else
            {
                globalBookmarkList = new JObject();
                JArray jArray = new JArray();
                globalBookmarkList.Add(new JProperty("items", jArray));
            }
            if (globalBookmarkList == null)
            {
                return false;
            }

            JToken jt = globalBookmarkList.Value<JToken>("items");
            if (jt == null)
            {
                return false;
            }

            JArray jItems = jt.Value<JArray>();
            _ = FindOrCreateTrackBookmarks(jItems, Player.FileName, out JArray jBookmarksArray);
            if (jBookmarksArray.Count > 0)
            {
                jBookmarksArray.Clear();
            }
            ToJsonArray(jBookmarksArray);

            if (fileExists)
            {
                File.Delete(fileName);
            }
            File.WriteAllText(fileName, globalBookmarkList.ToString());

            return true;
        }

        public static JObject LoadGlobalBookmarkListFromJsonFile(string fileName)
        {
            return JObject.Parse(File.ReadAllText(fileName));
        }

        public static JObject FindOrCreateTrackBookmarks(JArray itemsArray, string trackFullFilePath, out JArray jBookmarksArray)
        {
            foreach (JObject j in itemsArray)
            {
                JToken jt = j.Value<JToken>(trackFullFilePath);
                if (jt != null)
                {
                    jBookmarksArray = jt.Value<JArray>();
                    return j;
                }
            }

            JObject jItem = new JObject();
            jBookmarksArray = new JArray();
            jItem.Add(new JProperty(trackFullFilePath, jBookmarksArray));
            itemsArray.Add(jItem);
            return jItem;
        }

        public static JArray FindOrCreateTrackBookmarks(string bookmarksFileName, string trackFullFilePath)
        {
            JObject globalBookmarkList = LoadGlobalBookmarkListFromJsonFile(bookmarksFileName);
            if (globalBookmarkList == null)
            {
                return null;
            }
            JToken jt = globalBookmarkList.Value<JToken>("items");
            if (jt == null)
            {
                return null;
            }

            JArray jArray = jt.Value<JArray>();
            _ = FindOrCreateTrackBookmarks(jArray, trackFullFilePath, out JArray jBookmarksArray);
            return jBookmarksArray;
        }

        public static TimeSpan ParseTimeCode(string timeCode)
        {
            string fmt = timeCode.Contains(".") ? "HH:mm:ss.f" : "HH:mm:ss";
            if (DateTime.TryParseExact(timeCode, fmt, null, System.Globalization.DateTimeStyles.None, out DateTime dt))
            {
                return new TimeSpan(dt.Ticks);
            }
            return new TimeSpan(long.MaxValue);
        }

        public static string TimeToString(DateTime time, string format = "HH:mm:ss.f")
        {
            return time.ToString(format);
        }
    }

    public sealed class BookmarkItem
    {
        public TimeSpan TimeCode { get; private set; }
        public string ShortDescription { get; private set; }

        public BookmarkItem(TimeSpan timeCode, string shortDescription)
        {
            TimeCode = timeCode;
            ShortDescription = shortDescription;
        }
    }

    public sealed class BookmarkItemComparer : IComparer<BookmarkItem>
    {
        public int Compare(BookmarkItem x, BookmarkItem y)
        {
            if (x.TimeCode > y.TimeCode)
            {
                return 1;
            }
            else
            {
                return x.TimeCode == y.TimeCode ? 0 : -1;
            }
        }
    }
}
