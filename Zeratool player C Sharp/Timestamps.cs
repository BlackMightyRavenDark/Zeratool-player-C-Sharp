using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Zeratool_player_C_Sharp
{
    public sealed class Timestamps
    {
        public string FileName { get; private set; }
        public JObject JsonBase { get; private set; } = null;

        public Timestamps(string fileName)
        {
            FileName = fileName;
            LoadOrCreateBase(fileName);
        }

        private JObject CreateNewEmptyBase()
        {
            JsonBase = new JObject();
            JsonBase.Add(new JProperty("items", new JArray()));
            return JsonBase;
        }

        private JObject LoadOrCreateBase(string fileName)
        {
            if (File.Exists(fileName))
            {
                JsonBase = JObject.Parse(File.ReadAllText(fileName));
                if (JsonBase == null)
                {
                    JsonBase = CreateNewEmptyBase();
                }
                return JsonBase;
            }
            return CreateNewEmptyBase();
        }

        public void SaveToJsonFile()
        {
            if (File.Exists(FileName))
            {
                File.Delete(FileName);
            }
            File.WriteAllText(FileName, JsonBase.ToString());
        }

        public JArray GetOrCreateTimestampsArray(JObject jBase)
        {
            JToken jt = jBase.Value<JToken>("items");
            JArray jArray = jt != null ? jt.Value<JArray>() : new JArray();
            return jArray;
        }

        public JObject FindTrackTimestamp(JArray items, string trackFilePath)
        {
            foreach (JObject j in items)
            {
                string t = j.Value<string>("filePath");
                if (t == trackFilePath)
                {
                    return j;
                }
            }
            return null;
        }

        public void SaveTimestamp(string trackFilePath, TimeSpan timeSpan)
        {
            JArray items = GetOrCreateTimestampsArray(JsonBase);
            JObject jTimestamp = FindTrackTimestamp(items, trackFilePath);
            if (jTimestamp == null)
            {
                jTimestamp = new JObject();
                items.Add(jTimestamp);
            }
            jTimestamp["filePath"] = trackFilePath;
            jTimestamp["timeCode"] = ZeratoolBookmarks.TimeToString(new DateTime(timeSpan.Ticks), "HH:mm:ss");
        }

        public TimeSpan GetTimestamp(string trackFilePath)
        {
            JArray items = GetOrCreateTimestampsArray(JsonBase);
            JObject jTimestamp = FindTrackTimestamp(items, trackFilePath);
            if (jTimestamp != null)
            {
                JToken jt = jTimestamp.Value<JToken>("timeCode");
                if (jt != null)
                {
                    TimeSpan timeSpan = ZeratoolBookmarks.ParseTimeCode(jt.Value<string>());
                    return timeSpan;
                }
            }

            return TimeSpan.MaxValue;
        }
    }
}
