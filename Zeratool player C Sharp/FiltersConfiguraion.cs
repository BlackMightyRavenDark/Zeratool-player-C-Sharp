using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Newtonsoft.Json.Linq;
using DirectShowLib;
using static Zeratool_player_C_Sharp.DirectShowUtils;

namespace Zeratool_player_C_Sharp
{
    public class FiltersConfiguraion
    {
        public List<FilterItem> mediaSplittersAVI = new List<FilterItem>();
        public List<FilterItem> mediaSplittersMPG = new List<FilterItem>();
        public List<FilterItem> mediaSplittersTS = new List<FilterItem>();
        public List<FilterItem> mediaSplittersMP4 = new List<FilterItem>();
        public List<FilterItem> mediaSplittersMKV = new List<FilterItem>();
        public List<FilterItem> mediaSplittersOther = new List<FilterItem>();
        public List<FilterItem> videoDecoders = new List<FilterItem>();
        public List<FilterItem> videoRenderers = new List<FilterItem>();
        public List<FilterItem> audioDecoders = new List<FilterItem>();
        public List<MonikerItem> audioRenderers = new List<MonikerItem>();

        public int mediaSplitterAviId;
        public int mediaSplitterMpgId;
        public int mediaSplitterTsId;
        public int mediaSplitterMp4Id;
        public int mediaSplitterMkvId;
        public int mediaSplitterOtherId;
        public int videoDecoderId;
        public int prefferedVideoDecoderId;
        public int videoRendererId;
        public int prefferedVideoRendererId;
        public int audioDecoderId;
        public int prefferedAudioDecoderId;
        public int audioRendererId;
        public int prefferedAudioRendererId;

        public FiltersConfiguraion()
        {
            Clear();
        }

        public void Clear()
        {
            mediaSplittersAVI.Clear();
            mediaSplitterAviId = -1;

            mediaSplittersMPG.Clear();
            mediaSplitterMpgId = -1;

            mediaSplittersTS.Clear();
            mediaSplitterTsId = -1;

            mediaSplittersMP4.Clear();
            mediaSplitterMp4Id = -1;

            mediaSplittersMKV.Clear();
            mediaSplitterMkvId = -1;

            mediaSplittersOther.Clear();
            mediaSplitterOtherId = -1;

            videoDecoders.Clear();
            videoDecoderId = -1;
            prefferedVideoDecoderId = -1;

            videoRenderers.Clear();
            videoRendererId = -1;
            prefferedVideoRendererId = 2;

            audioDecoders.Clear();
            audioDecoderId = -1;
            prefferedAudioDecoderId = -1;

            prefferedAudioRendererId = -1;
            audioRendererId = prefferedAudioRendererId;
        }

        private void SetDefaultMediaSplittersAvi()
        {
            mediaSplittersAVI.Clear();
            mediaSplittersAVI.Add(new FilterItem(CLSID_HaaliMediaSplitter, "Haali media splitter"));
            mediaSplittersAVI.Add(new FilterItem(CLSID_LAV_Splitter, "LAV splitter"));
            mediaSplitterAviId = -1;
        }

        private void SetDefaultMediaSplittersMpg()
        {
            mediaSplittersMPG.Clear();
            mediaSplittersMPG.Add(new FilterItem(CLSID_HaaliMediaSplitter, "Haali media splitter"));
            mediaSplittersMPG.Add(new FilterItem(CLSID_LAV_Splitter, "LAV splitter"));
            mediaSplittersMPG.Add(new FilterItem(CLSID_MPEG2Splitter, "MPEG-2 splitter"));
            mediaSplitterMpgId = -1;
        }

        private void SetDefaultMediaSplittersTs()
        {
            mediaSplittersTS.Clear();
            mediaSplittersTS.Add(new FilterItem(CLSID_HaaliMediaSplitter, "Haali media splitter"));
            mediaSplittersTS.Add(new FilterItem(CLSID_LAV_Splitter, "LAV splitter"));
            mediaSplitterTsId = -1;
        }

        private void SetDefaultMediaSplittersMp4()
        {
            mediaSplittersMP4.Clear();
            mediaSplittersMP4.Add(new FilterItem(CLSID_HaaliMediaSplitter, "Haali media splitter"));
            mediaSplittersMP4.Add(new FilterItem(CLSID_LAV_Splitter, "LAV splitter"));
            mediaSplitterMp4Id = -1;
        }

        private void SetDefaultMediaSplittersMkv()
        {
            mediaSplittersMKV.Clear();
            mediaSplittersMKV.Add(new FilterItem(CLSID_HaaliMediaSplitter, "Haali media splitter"));
            mediaSplittersMKV.Add(new FilterItem(CLSID_LAV_Splitter, "LAV splitter"));
            mediaSplitterMkvId = -1;
        }

        private void SetDefaultMediaSplittersOther()
        {
            mediaSplittersOther.Clear();
            mediaSplittersOther.Add(new FilterItem(CLSID_HaaliMediaSplitter, "Haali media splitter"));
            mediaSplittersOther.Add(new FilterItem(CLSID_LAV_Splitter, "LAV splitter"));
            mediaSplitterOtherId = -1;
        }

        private void SetDefaultAudioDecoders()
        {
            audioDecoders.Clear();
            audioDecoders.Add(new FilterItem(CLSID_FFDShowAudioDecoder, "FFDShow audio decoder"));
            audioDecoders.Add(new FilterItem(CLSID_LAV_AudioDecoder, "LAV audio decoder"));
            audioDecoders.Add(new FilterItem(CLSID_AC3Filter, "AC3Filter"));
            audioDecoderId = -1;
            prefferedAudioDecoderId = -1;
        }

        private void SetDefaultVideoDecoders()
        {
            videoDecoders.Clear();
            videoDecoders.Add(new FilterItem(CLSID_FFDShowVideoDecoder, "FFDShow video decoder"));
            videoDecoders.Add(new FilterItem(CLSID_LAV_VideoDecoder, "LAV video decoder"));
            videoDecoderId = -1;
            prefferedVideoDecoderId = -1;
        }

        private void SetDefaultVideoRenderers()
        {
            videoRenderers.Clear();
            videoRenderers.Add(new FilterItem(CLSID_DefaultVideoRenderer, "Default video renderer"));
            videoRenderers.Add(new FilterItem(CLSID_VideoRenderer, "Video renderer"));
            videoRenderers.Add(new FilterItem(CLSID_VideoMixingRenderer9, "Video mixing renderer 9 (VMR9)"));
            videoRenderers.Add(new FilterItem(CLSID_HaaliVideoRenderer, "Haali video renderer"));
            videoRenderers.Add(new FilterItem(CLSID_EnhancedVideoRenderer, "Enhanced video renderer"));
            videoRendererId = 2;
            prefferedVideoRendererId = 2;
        }

        public void SetDefaults()
        {
            SetDefaultMediaSplittersAvi();
            SetDefaultMediaSplittersMpg();
            SetDefaultMediaSplittersTs();
            SetDefaultMediaSplittersMp4();
            SetDefaultMediaSplittersMkv();
            SetDefaultMediaSplittersOther();
            SetDefaultAudioDecoders();
            SetDefaultVideoDecoders();
            SetDefaultVideoRenderers();

            prefferedAudioRendererId = audioRenderers.Count > 0 ? 0 : -1;
            audioRendererId = prefferedAudioRendererId;
        }

        public int FindAudioRendererByName(string audioRendererDeviceName)
        {
            for (int i = 0; i < audioRenderers.Count; i++)
            {
                if (audioRenderers[i].DisplayName == audioRendererDeviceName)
                {
                    return i;
                }
            }
            return -1;
        }

        public void SaveToJsonFile(string fileName)
        {
            JObject json = new JObject();
            SaveGroup(mediaSplittersAVI, "splittersAvi", mediaSplitterAviId, json);
            SaveGroup(mediaSplittersMPG, "splittersMpg", mediaSplitterMpgId, json);
            SaveGroup(mediaSplittersTS, "splittersTs", mediaSplitterTsId, json);
            SaveGroup(mediaSplittersMP4, "splittersMp4", mediaSplitterMp4Id, json);
            SaveGroup(mediaSplittersMKV, "splittersMkv", mediaSplitterMkvId, json);
            SaveGroup(mediaSplittersOther, "splittersOther", mediaSplitterOtherId, json);
            SaveGroup(audioDecoders, "audioDecoders", prefferedAudioDecoderId, json);
            SaveGroup(videoDecoders, "videoDecoders", prefferedVideoDecoderId, json);
            SaveGroup(videoRenderers, "videoRenderers", prefferedVideoRendererId, json);
            string audioRendererName = prefferedAudioRendererId >= 0 && audioRenderers.Count > 0 ?
                audioRenderers[prefferedAudioRendererId].DisplayName : null;
            if (!string.IsNullOrEmpty(audioRendererName) && !string.IsNullOrWhiteSpace(audioRendererName))
            {
                json["prefferedAudioRendererName"] = audioRendererName;
            }

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            File.WriteAllText(fileName, json.ToString());

            void SaveGroup(IEnumerable<FilterItem> group, string groupName, int selectedIndex, JObject root)
            {
                JArray jArray = new JArray();
                foreach (FilterItem filter in group)
                {
                    JObject jFilter = new JObject();
                    jFilter["displayName"] = filter.DisplayName;
                    jFilter["guid"] = filter.Guid;
                    jArray.Add(jFilter);
                }
                JObject jSplittersGroup = new JObject();
                jSplittersGroup["selectedIndex"] = selectedIndex;
                jSplittersGroup["list"] = jArray;
                root.Add(new JProperty(groupName, jSplittersGroup));
            }
        }

        public void LoadFromJsonFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName))
            {
                prefferedAudioRendererId = audioRenderers.Count > 0 ? 0 : -1;
                return;
            }

            JObject json = JObject.Parse(File.ReadAllText(fileName));
            if (json != null)
            {
                if (!LoadGroup("splittersAvi", mediaSplittersAVI, ref mediaSplitterAviId, json))
                {
                    SetDefaultMediaSplittersAvi();
                }
                if (!LoadGroup("splittersMpg", mediaSplittersMPG, ref mediaSplitterMpgId, json))
                {
                    SetDefaultMediaSplittersMpg();
                }
                if (!LoadGroup("splittersTs", mediaSplittersTS, ref mediaSplitterTsId, json))
                {
                    SetDefaultMediaSplittersTs();
                }
                if (!LoadGroup("splittersMp4", mediaSplittersMP4, ref mediaSplitterMp4Id, json))
                {
                    SetDefaultMediaSplittersMp4();
                }
                if (!LoadGroup("splittersMkv", mediaSplittersMKV, ref mediaSplitterMkvId, json))
                {
                    SetDefaultMediaSplittersMkv();
                }
                if (!LoadGroup("splittersOther", mediaSplittersOther, ref mediaSplitterOtherId, json))
                {
                    SetDefaultMediaSplittersOther();
                }
                if (LoadGroup("audioDecoders", audioDecoders, ref prefferedAudioDecoderId, json))
                {
                    audioDecoderId = prefferedAudioDecoderId;
                }
                else
                {
                    SetDefaultAudioDecoders();
                }
                if (LoadGroup("videoDecoders", videoDecoders, ref prefferedVideoDecoderId, json))
                {
                    videoDecoderId = prefferedVideoDecoderId;
                }
                else
                {
                    SetDefaultVideoDecoders();
                }
                if (LoadGroup("videoRenderers", videoRenderers, ref prefferedVideoRendererId, json))
                {
                    if (prefferedVideoRendererId < 0)
                    {
                        prefferedVideoRendererId = videoRenderers.Count > 0 ? 0 : -1;
                    }
                    videoRendererId = prefferedVideoRendererId;
                }
                else
                {
                    SetDefaultVideoRenderers();
                }

                JToken jt = json.Value<JToken>("prefferedAudioRendererName");
                string prefferedAudioRendererName = jt?.Value<string>();
                if (string.IsNullOrEmpty(prefferedAudioRendererName) || string.IsNullOrWhiteSpace(prefferedAudioRendererName))
                {
                    prefferedAudioRendererId = audioRenderers.Count > 0 ? 0 : -1;
                }
                else
                {
                    prefferedAudioRendererId = FindAudioRendererByName(prefferedAudioRendererName);
                    if (prefferedAudioRendererId < 0)
                    {
                        prefferedAudioRendererId = audioRenderers.Count > 0 ? 0 : -1;
                    }
                }
            }

            bool LoadGroup(string groupName, List<FilterItem> collection, ref int selected, JObject root)
            {
                collection.Clear();
                selected = -1;

                JToken jToken = root.Value<JToken>(groupName);
                if (jToken != null)
                {
                    JObject jObject = jToken.Value<JObject>();
                    jToken = jObject.Value<JToken>("selectedIndex");
                    if (jToken != null)
                    {
                        selected = jToken.Value<int>();
                    }
                    jToken = jObject.Value<JToken>("list");
                    if (jToken != null)
                    {
                        JArray jsonArr = jToken.Value<JArray>();
                        foreach (JObject j in jsonArr)
                        {
                            string filterName = j?.Value<string>("displayName");
                            if (string.IsNullOrEmpty(filterName) || string.IsNullOrWhiteSpace(filterName))
                            {
                                continue;
                            }
                            string guidString = j?.Value<string>("guid");
                            if (string.IsNullOrEmpty(guidString) || string.IsNullOrWhiteSpace(guidString))
                            {
                                continue;
                            }
                            if (Guid.TryParse(guidString, out Guid guid))
                            {
                                collection.Add(new FilterItem(guid, filterName));
                            }
                        }
                    }
                    if (selected >= collection.Count)
                    {
                        selected = collection.Count - 1;
                    }
                    return true;
                }
                return false;
            }
        }
    }

    public class FilterItem
    {
        public Guid Guid { get; private set; }
        public string DisplayName { get; private set; }

        public FilterItem(Guid guid, string displayName)
        {
            Guid = guid;
            DisplayName = displayName;
        }
    }

    public class MonikerItem
    {
        public IMoniker Moniker { get; private set; }
        public IPropertyBag PropertyBag { get; private set; }
        public string DisplayName { get; private set; }

        public MonikerItem(IMoniker moniker, IPropertyBag propertyBag, string displayName)
        {
            Moniker = moniker;
            PropertyBag = propertyBag;
            DisplayName = displayName;
        }
    }
}
