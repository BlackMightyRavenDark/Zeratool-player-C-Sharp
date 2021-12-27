﻿using System;
using System.Collections.Generic;

namespace Zeratool_player_C_Sharp
{
    public class ZeratoolBookmarks
    {
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
        public BookmarkAddedDelegate BookmarkAdded;
        public BookmarkRemovedDelegate BookmarkRemoved;

        public int Add(TimeSpan time, string shortDescription)
        {
            int id = IndexOf(time);
            if (id == -1)
            {
                BookmarkItem item = new BookmarkItem(time, shortDescription);
                items.Add(item);
                items.Sort(new BookmarkItemComparer());
                BookmarkAdded?.Invoke(this, item);
                id = IndexOf(time);
                return id;
            }
            return id;
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