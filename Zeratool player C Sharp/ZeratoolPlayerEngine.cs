using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DirectShowLib;
using static Zeratool_player_C_Sharp.DirectShowUtils;
using static Zeratool_player_C_Sharp.Utils;

namespace Zeratool_player_C_Sharp
{
    public class ZeratoolPlayerEngine : Control
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

                audioRendererId = 0;
                prefferedAudioRendererId = 0;
            }

            public void SetDefaults()
            {
                Clear();

                mediaSplittersAVI.Add(new FilterItem(CLSID_HaaliMediaSplitter, "Haali media splitter"));
                mediaSplittersAVI.Add(new FilterItem(CLSID_LAV_Splitter, "LAV splitter"));

                mediaSplittersMPG.Add(new FilterItem(CLSID_HaaliMediaSplitter, "Haali media splitter"));
                mediaSplittersMPG.Add(new FilterItem(CLSID_LAV_Splitter, "LAV splitter"));
                mediaSplittersMPG.Add(new FilterItem(CLSID_MPEG2Splitter, "MPEG-2 splitter"));

                mediaSplittersTS.Add(new FilterItem(CLSID_HaaliMediaSplitter, "Haali media splitter"));
                mediaSplittersTS.Add(new FilterItem(CLSID_LAV_Splitter, "LAV splitter"));

                mediaSplittersMP4.Add(new FilterItem(CLSID_HaaliMediaSplitter, "Haali media splitter"));
                mediaSplittersMP4.Add(new FilterItem(CLSID_LAV_Splitter, "LAV splitter"));

                mediaSplittersMKV.Add(new FilterItem(CLSID_HaaliMediaSplitter, "Haali media splitter"));
                mediaSplittersMKV.Add(new FilterItem(CLSID_LAV_Splitter, "LAV splitter"));

                mediaSplittersOther.Add(new FilterItem(CLSID_HaaliMediaSplitter, "Haali media splitter"));
                mediaSplittersOther.Add(new FilterItem(CLSID_LAV_Splitter, "LAV splitter"));
                
                videoDecoders.Add(new FilterItem(CLSID_FFDShowVideoDecoder, "FFDShow video decoder"));
                videoDecoders.Add(new FilterItem(CLSID_LAV_VideoDecoder, "LAV video decoder"));

                audioDecoders.Add(new FilterItem(CLSID_FFDShowAudioDecoder, "FFDShow audio decoder"));
                audioDecoders.Add(new FilterItem(CLSID_AC3Filter, "AC3Filter"));
                audioDecoders.Add(new FilterItem(CLSID_LAV_AudioDecoder, "LAV audio decoder"));

                videoRenderers.Add(new FilterItem(CLSID_DefaultVideoRenderer, "Default video renderer"));
                videoRenderers.Add(new FilterItem(CLSID_VideoRenderer, "Video renderer"));
                videoRenderers.Add(new FilterItem(CLSID_VideoMixingRenderer9, "Video mixing renderer 9 (VMR9)"));
                videoRenderers.Add(new FilterItem(CLSID_HaaliVideoRenderer, "Haali video renderer"));
                videoRenderers.Add(new FilterItem(CLSID_EnhancedVideoRenderer, "Enhanced video renderer"));
            }
        }

        private IGraphBuilder graphBuilder = null;
        private ICaptureGraphBuilder2 captureGraphBuilder2 = null;
        private IVideoWindow videoWindow = null;
        private IBasicVideo basicVideo = null;
        private IBasicAudio basicAudio = null;
        private IMediaControl mediaControl = null;
        private IMediaPosition mediaPosition = null;
        private IMediaEventEx mediaEventEx = null;

        private IFileSourceFilter fileSource = null;
        private IBaseFilter fileSourceFilter = null;
        private IBaseFilter mediaSplitter = null;
        private IBaseFilter videoDecoder = null;
        private IBaseFilter audioDecoder = null;
        private IBaseFilter videoRenderer = null;
        private IBaseFilter audioRenderer = null;
        
        public const int DIRECTSHOW_EVENTS_MESSAGE = 1025;

        public const int ERROR_FILE_NOT_FOUND = -100;
        public const int ERROR_FILE_NAME_NOT_DEFINED = -101;
        public const int ERROR_NOTHING_RENDERED = -102;


        public enum GRAPH_MODE { Automatic, Intellectual, Manual };
        public enum PLAYER_STATE { Null, Playing, Paused, Stopped };

        private GRAPH_MODE _graphMode = GRAPH_MODE.Manual;
        private PLAYER_STATE _state = PLAYER_STATE.Null;
        private int _videoWidth = 0;
        private int _videoHeight = 0;
        private int _volume = 25;
        private Rectangle _outputScreenRect = new Rectangle(0, 0, 0, 0);

        public string FileName { get; set; }
        public GRAPH_MODE GraphMode 
        {
            get
            {
                return _graphMode;
            }
            set
            {
                if (State == PLAYER_STATE.Null)
                {
                    _graphMode = value;
                }
            } 
        }
        public PLAYER_STATE State { get { return _state; } }
        public Control OutputWindow { get; set; }
        public Rectangle OutputScreenRect { get { return _outputScreenRect; } set { SetScreenRect(value); } }
        public Size VideoSize { get { return new Size(_videoWidth, _videoHeight); } }
        public double Duration
        {
            get
            {
                if (mediaPosition != null)
                {
                    int errorCode = mediaPosition.get_Duration(out double dur);
                    return errorCode == S_OK ? dur : 0.0;
                }
                return 0.0;
            }
        }
        public double Position
        { 
            get
            {
                if (mediaPosition != null)
                {
                    int errorCode = mediaPosition.get_CurrentPosition(out double pos);
                    return errorCode == S_OK ? pos : 0.0;
                }
                return 0.0;
            }
            set
            {
                if (mediaPosition != null)
                {
                    double newPos = value;
                    if (newPos < 0.0)
                        newPos = 0.0;
                    else if (newPos > Duration)
                        newPos = Duration;
                    mediaPosition.put_CurrentPosition(newPos);
                }
            }         
        }
        public int Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                if (value < 0)
                    value = 0;
                else if (value > 100)
                    value = 100;
                if (_volume != value)
                {
                    _volume = value;
                    if (AudioRendered)
                    {
                        int db = GetDecibelsVolume(_volume);
                        basicAudio.put_Volume(db);
                    }
                }
            }
        }
        public bool VideoRendered => basicVideo != null;
        public bool AudioRendered => basicAudio != null;

        public readonly FiltersConfiguraion filters = new FiltersConfiguraion();

        public delegate void ClearedDelegate(object sender);
        public delegate void TrackRenderedDelegate(object sender, int errorCode);
        public delegate void TrackFinishedDelegate(object sender);
        public ClearedDelegate Cleared;
        public TrackRenderedDelegate TrackRendered;
        public TrackFinishedDelegate TrackFinished;


        public ZeratoolPlayerEngine()
        {
            filters.SetDefaults();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == DIRECTSHOW_EVENTS_MESSAGE)
            {
                while (mediaEventEx.GetEvent(
                    out EventCode eventCode, out IntPtr param1, out IntPtr param2, 1) == S_OK)
                {
                    mediaEventEx.FreeEventParams(eventCode, param1, param2);
                    if (eventCode == EventCode.Complete && State == PLAYER_STATE.Playing)
                    {
                        System.Diagnostics.Debug.WriteLine("Player engine event: Track finished");

                        if (TrackFinished != null)
                        {
                            TrackFinished.Invoke(this);
                        }
                        else
                        {
                            Pause();
                        }
                    }
                }
            }
            base.WndProc(ref m);
        }


        private int BuildGraph()
        {
            if (string.IsNullOrEmpty(FileName) || string.IsNullOrWhiteSpace(FileName))
            {
                return ERROR_FILE_NAME_NOT_DEFINED;
            }

            if (!File.Exists(FileName))
            {
                return ERROR_FILE_NOT_FOUND;
            }

            filters.videoDecoderId = filters.prefferedVideoDecoderId; 
            filters.videoRendererId = filters.prefferedVideoRendererId;
            filters.audioDecoderId = filters.prefferedAudioDecoderId;
            filters.audioRendererId = filters.prefferedAudioRendererId;

            System.Diagnostics.Debug.WriteLine($"File name: {FileName}");

            int errorCode;

            switch (GraphMode)
            {
                case GRAPH_MODE.Automatic:
                    System.Diagnostics.Debug.WriteLine("Building graph in automatic mode.");
                    errorCode = CreateComObject<FilterGraph, IGraphBuilder>(out graphBuilder); 
                    if (errorCode != S_OK)
                    {
                        return errorCode;
                    }
                    errorCode = graphBuilder.RenderFile(FileName, null);
                    if (errorCode != S_OK || !GetComInterface<IMediaControl>(graphBuilder, out mediaControl) || 
                        !GetVideoInterfaces() || !ConfigureVideoOutput())
                    {
                        Clear();
                        return errorCode;
                    }
                    if (GetComInterface<IBasicAudio>(graphBuilder, out basicAudio))
                    {
                        basicAudio.put_Volume(GetDecibelsVolume(Volume));
                    }

                    mediaPosition = (IMediaPosition)graphBuilder;

                    if (GetComInterface<IMediaEventEx>(graphBuilder, out mediaEventEx))
                    {
                        mediaEventEx.SetNotifyWindow(Handle, DIRECTSHOW_EVENTS_MESSAGE, new IntPtr(0));
                    }

                    _state = PLAYER_STATE.Stopped;
                    TrackRendered?.Invoke(this, errorCode);

                    return errorCode;

                case GRAPH_MODE.Intellectual:
                    System.Diagnostics.Debug.WriteLine("Building graph in intellectual mode.");
                    errorCode = BuildGraphIntellectual();
                    TrackRendered?.Invoke(this, errorCode);
                    return errorCode;

                case GRAPH_MODE.Manual:
                    System.Diagnostics.Debug.WriteLine("Building graph in manual mode.");
                    errorCode = BuildGraphManual();
                    TrackRendered?.Invoke(this, errorCode);
                    return errorCode;
            }

            return S_FALSE;
        }

        private int BuildGraphManual()
        {
            int errorCode = CreateComObject<FilterGraph, IGraphBuilder>(out graphBuilder);
            if (errorCode != S_OK)
            {
                Clear();
                return errorCode;
            }

            errorCode = CreateDirectShowFilter(CLSID_FileSourceAsync, out fileSourceFilter);
            if (errorCode != S_OK)
            {
                Clear();
                return errorCode;
            }
            graphBuilder.AddFilter(fileSourceFilter, "Source filter");

            fileSource = (IFileSourceFilter)fileSourceFilter;
            if (fileSource == null)
            {
                graphBuilder.RemoveFilter(fileSourceFilter);
                Clear();
                return E_POINTER;
            }

            errorCode = fileSource.Load(FileName, null);
            if (errorCode != S_OK)
            {
                graphBuilder.RemoveFilter(fileSourceFilter);
                Clear();
                return errorCode;
            }

            if (FindPin(fileSourceFilter, 0, PinDirection.Output, out IPin pinOut) != S_OK)
            {
                System.Diagnostics.Debug.WriteLine("Source filter's output pin not found!");
                Clear();
                return E_POINTER;
            }

            errorCode = ConnectMediaSplitter_Manual(pinOut);
            if (errorCode != S_OK)
            {
                graphBuilder.RemoveFilter(fileSourceFilter);
                Marshal.ReleaseComObject(pinOut);
                Clear();
                return errorCode;
            }
            Marshal.ReleaseComObject(pinOut);

            //render video chain.
            int errorCodeVideo = FindPin(mediaSplitter, "ideo", PinDirection.Output, out pinOut);
            if (errorCodeVideo == S_OK)
            {
                errorCodeVideo = RenderVideoStream_Manual(pinOut);
                Marshal.ReleaseComObject(pinOut);
                if (errorCodeVideo != S_OK || !GetVideoInterfaces() || !ConfigureVideoOutput())
                {
                    ClearVideoChain();
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Media splitter can't find video output pin!");
            }

            //render audio chain.
            int errorCodeAudio = FindPin(mediaSplitter, "udio", PinDirection.Output, out pinOut);
            if (errorCodeAudio == S_OK)
            {
                errorCodeAudio = RenderAudioStream_Manual(pinOut);
                Marshal.ReleaseComObject(pinOut);
                if (errorCodeAudio == S_OK && GetComInterface<IBasicAudio>(graphBuilder, out basicAudio))
                {
                    basicAudio.put_Volume(GetDecibelsVolume(Volume));
                }
                else
                {
                    ClearAudioChain();
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Media splitter can't find audio output pin!");
            }


            if (errorCodeAudio != S_OK && errorCodeVideo != S_OK)
            {
                Clear();
                System.Diagnostics.Debug.WriteLine("Error: ERROR_NOTHING_RENDERED");
                return ERROR_NOTHING_RENDERED;
            }

            if (!GetComInterface<IMediaControl>(graphBuilder, out mediaControl))
            {
                Clear();
                return E_POINTER;
            }

            if (GetComInterface<IMediaEventEx>(graphBuilder, out mediaEventEx))
            {
                mediaEventEx.SetNotifyWindow(Handle, DIRECTSHOW_EVENTS_MESSAGE, new IntPtr(0));
            }

            mediaPosition = (IMediaPosition)graphBuilder;

            _state = PLAYER_STATE.Stopped;

            return S_OK;
        }


        private int ConnectMediaSplitter_Manual(IPin sourcePinOut)
        {
            int errorCode;
            FilterItem filterItem;
            List<FilterItem> splitters;
            int splitterId;
            string ext = Path.GetExtension(FileName).ToLower();
            if (ext == ".avi")
            {
                splitters = filters.mediaSplittersAVI;
                splitterId = filters.mediaSplitterAviId;
            }
            else if (ext == ".mpg" || ext == ".mpeg")
            {
                splitters = filters.mediaSplittersMPG;
                splitterId = filters.mediaSplitterMpgId;
            }
            else if (ext == ".ts")
            {
                splitters = filters.mediaSplittersTS;
                splitterId = filters.mediaSplitterTsId;
            }
            else if (ext == ".mp4")
            {
                splitters = filters.mediaSplittersMP4;
                splitterId = filters.mediaSplitterMp4Id;
            }
            else if (ext == ".mkv")
            {
                splitters = filters.mediaSplittersMKV;
                splitterId = filters.mediaSplitterMkvId;
            }
            else
            {
                splitters = filters.mediaSplittersOther;
                splitterId = filters.mediaSplitterOtherId;
            }

            //если сплиттер выбран вручную.
            if (splitterId >= 0)
            {
                filterItem = splitters[splitterId];
                errorCode = CreateDirectShowFilter(filterItem.GetGuid(), out mediaSplitter);
                if (errorCode != S_OK)
                {
                    System.Diagnostics.Debug.WriteLine($"Loading {filterItem.DisplayName}: {ErrorCodeToString(errorCode)}");
                    return errorCode;
                }
                System.Diagnostics.Debug.WriteLine($"Loading {filterItem.DisplayName}: S_OK");
                graphBuilder.AddFilter(mediaSplitter, filterItem.DisplayName);

                if (FindPin(mediaSplitter, 0, PinDirection.Input, out IPin pinIn) != S_OK)
                {
                    System.Diagnostics.Debug.WriteLine($"{filterItem.DisplayName}: input pin not found!");
                    graphBuilder.RemoveFilter(mediaSplitter);
                    return E_POINTER;
                }

                errorCode = graphBuilder.Connect(sourcePinOut, pinIn);
                if (errorCode != S_OK)
                {
                    System.Diagnostics.Debug.WriteLine($"Connecting {filterItem.DisplayName}: {ErrorCodeToString(errorCode)}");
                    graphBuilder.RemoveFilter(mediaSplitter);
                    Marshal.ReleaseComObject(pinIn);
                    Marshal.ReleaseComObject(mediaSplitter);
                    return errorCode;
                }

                System.Diagnostics.Debug.WriteLine($"Connecting {filterItem.DisplayName}: S_OK");
                return errorCode;
            }

            //автоматический перебор сплиттеров.
            System.Diagnostics.Debug.WriteLine("Automatic media splitter selection mode.");
            return FindAndConnectMediaSplitter_Manual(splitters, sourcePinOut);
        }

        private int FindAndConnectMediaSplitter_Manual(IEnumerable<FilterItem> splitters, IPin sourcePinOut)
        {
            foreach (FilterItem item in splitters)
            {
                Guid splitterGuid = item.GetGuid();
                string splitterName = item.DisplayName;
                if (splitterGuid.Equals(CLSID_HaaliMediaSplitter))
                {
                    splitterGuid = CLSID_HaaliMediaSplitterAR;
                    splitterName = "Haali media splitter (AR)";
                }

                int errorCode = CreateDirectShowFilter(splitterGuid, out IBaseFilter filter);
                if (errorCode != S_OK)
                {
                    System.Diagnostics.Debug.WriteLine($"Loading {splitterName}: {ErrorCodeToString(errorCode)}");                    
                    continue;
                }
                System.Diagnostics.Debug.WriteLine($"Loading {splitterName}: S_OK");
                graphBuilder.AddFilter(filter, splitterName);

                if (FindPin(filter, 0, PinDirection.Input, out IPin pinIn) != S_OK)
                {
                    System.Diagnostics.Debug.WriteLine($"{splitterName}: input pin not found!");
                    graphBuilder.RemoveFilter(filter);
                    Marshal.ReleaseComObject(filter);
                    continue;
                }

                errorCode = graphBuilder.Connect(sourcePinOut, pinIn);
                if (errorCode != S_OK)
                {
                    System.Diagnostics.Debug.WriteLine($"Connecting {splitterName}: {ErrorCodeToString(errorCode)}");
                    graphBuilder.RemoveFilter(filter);
                    Marshal.ReleaseComObject(pinIn);
                    Marshal.ReleaseComObject(filter);
                    continue;
                }
                System.Diagnostics.Debug.WriteLine($"Connecting {splitterName}: S_OK");
                Marshal.ReleaseComObject(pinIn);

                FindPin(filter, "ideo", PinDirection.Output, out IPin splitterPinOut);
                if (splitterPinOut == null)
                {
                    System.Diagnostics.Debug.WriteLine("Video output pin not found! Skipping this bad bad filter.");
                    graphBuilder.RemoveFilter(filter);
                    Marshal.ReleaseComObject(filter);
                    continue;
                }
                Marshal.ReleaseComObject(splitterPinOut);

                mediaSplitter = filter;

                return errorCode;
            }

            System.Diagnostics.Debug.WriteLine("No one valid media splitter found!");
            return S_FALSE;
        }

        private int RenderVideoStream_Manual(IPin pinOut)
        {
            FilterItem filterItem;
            IPin pinIn;
            int errorCode;

            //если декодер видео выбран вручную.
            if (filters.videoDecoderId >= 0)
            {
                filterItem = filters.videoDecoders[filters.videoDecoderId];
                errorCode = CreateDirectShowFilter(filterItem.GetGuid(), out videoDecoder);
                if (errorCode != S_OK)
                {
                    System.Diagnostics.Debug.WriteLine($"Loading {filterItem.DisplayName}: {ErrorCodeToString(errorCode)}");
                    return errorCode;
                }
                System.Diagnostics.Debug.WriteLine($"Loading {filterItem.DisplayName}: S_OK");
                graphBuilder.AddFilter(videoDecoder, filterItem.DisplayName);

                if (FindPin(videoDecoder, 0, PinDirection.Input, out pinIn) != S_OK)
                {
                    System.Diagnostics.Debug.WriteLine($"{filterItem.DisplayName}: input pin not found!");
                    graphBuilder.RemoveFilter(videoDecoder);
                    return E_POINTER;
                }

                errorCode = graphBuilder.Connect(pinOut, pinIn);
                if (errorCode != S_OK)
                {
                    System.Diagnostics.Debug.WriteLine($"Connecting {filterItem.DisplayName}: {ErrorCodeToString(errorCode)}");
                    graphBuilder.RemoveFilter(videoDecoder);
                    return errorCode;
                }
                System.Diagnostics.Debug.WriteLine($"Connecting {filterItem.DisplayName}: S_OK");

            }
            else
            {
                //автоматический перебор декодеров видео.
                System.Diagnostics.Debug.WriteLine("Automatic video decoder selection mode.");
                errorCode = FindAndConnectVideoDecoder_Manual(pinOut);

                if (errorCode != S_OK)
                {
                    return errorCode;
                }
            }

            //processing video renderer.
            filterItem = filters.videoRenderers[filters.videoRendererId];
            errorCode = CreateDirectShowFilter(filterItem.GetGuid(), out videoRenderer);
            if (errorCode != S_OK)
            {
                System.Diagnostics.Debug.WriteLine($"Loading {filterItem.DisplayName}: {ErrorCodeToString(errorCode)}");
                graphBuilder.RemoveFilter(videoRenderer);
                return errorCode;
            }
            System.Diagnostics.Debug.WriteLine($"Loading {filterItem.DisplayName}: S_OK");
            graphBuilder.AddFilter(videoRenderer, filterItem.DisplayName);

            if (FindPin(videoDecoder, 0, PinDirection.Output, out pinOut) != S_OK)
            {
                graphBuilder.RemoveFilter(videoRenderer);
                graphBuilder.RemoveFilter(videoDecoder);
                return E_POINTER;
            }
            if (FindPin(videoRenderer, 0, PinDirection.Input, out pinIn) != S_OK)
            {
                Marshal.ReleaseComObject(pinOut);
                graphBuilder.RemoveFilter(videoRenderer);
                graphBuilder.RemoveFilter(videoDecoder);
                return E_POINTER;
            }

            errorCode = graphBuilder.Connect(pinOut, pinIn);
            if (errorCode != S_OK)
            {
                System.Diagnostics.Debug.WriteLine($"Connecting {filterItem.DisplayName}: {ErrorCodeToString(errorCode)}");
                graphBuilder.RemoveFilter(videoDecoder);
                graphBuilder.RemoveFilter(videoRenderer);
            }
            System.Diagnostics.Debug.WriteLine($"Connecting {filterItem.DisplayName}: S_OK");
            Marshal.ReleaseComObject(pinOut);
            Marshal.ReleaseComObject(pinIn);

            return errorCode;
        }

        private int FindAndConnectVideoDecoder_Manual(IPin pinOut)
        {
            foreach (FilterItem item in filters.videoDecoders)
            {
                int errorCode = CreateDirectShowFilter(item.GetGuid(), out IBaseFilter filter);
                if (errorCode != S_OK)
                {
                    System.Diagnostics.Debug.WriteLine($"Loading {item.DisplayName}: {ErrorCodeToString(errorCode)}");
                    continue;
                }
                System.Diagnostics.Debug.WriteLine($"Loading {item.DisplayName}: S_OK");
                graphBuilder.AddFilter(filter, item.DisplayName);
                
                if (FindPin(filter, 0, PinDirection.Input, out IPin pinIn) != S_OK)
                {
                    System.Diagnostics.Debug.WriteLine($"{item.DisplayName}: input pin not found!");
                    graphBuilder.RemoveFilter(filter);
                    continue;
                }

                errorCode = graphBuilder.Connect(pinOut, pinIn);
                if (errorCode != S_OK)
                {
                    System.Diagnostics.Debug.WriteLine($"Connecting {item.DisplayName}: {ErrorCodeToString(errorCode)}");
                    graphBuilder.RemoveFilter(filter);
                    Marshal.ReleaseComObject(pinIn);
                    Marshal.ReleaseComObject(filter);
                    continue;
                }
                System.Diagnostics.Debug.WriteLine($"Connecting {item.DisplayName}: S_OK");

                Marshal.ReleaseComObject(pinIn);
                videoDecoder = filter;
                return errorCode;
            }

            System.Diagnostics.Debug.WriteLine("Failed to find video decoder!");
            return S_FALSE;
        }

        private int RenderAudioStream_Manual(IPin pinOut)
        {
            int errorCode;
            IPin pinIn;

            //если декодер аудио выбран вручную.
            if (filters.audioDecoderId >= 0)
            {
                FilterItem filterItemAudioDecoder;
                filterItemAudioDecoder = filters.audioDecoders[filters.audioDecoderId];
                errorCode = CreateDirectShowFilter(filterItemAudioDecoder.GetGuid(), out audioDecoder);
                if (errorCode != S_OK)
                {
                    System.Diagnostics.Debug.WriteLine($"Loading {filterItemAudioDecoder.DisplayName}: {ErrorCodeToString(errorCode)}");
                    return errorCode;
                }
                System.Diagnostics.Debug.WriteLine($"Loading {filterItemAudioDecoder.DisplayName}: S_OK");
                graphBuilder.AddFilter(audioDecoder, filterItemAudioDecoder.DisplayName);

                if (FindPin(audioDecoder, 0, PinDirection.Input, out pinIn) != S_OK)
                {
                    System.Diagnostics.Debug.WriteLine($"{filterItemAudioDecoder.DisplayName}: input pin not found!");
                    graphBuilder.RemoveFilter(audioDecoder);
                    return E_POINTER;
                }

                errorCode = graphBuilder.Connect(pinOut, pinIn);
                Marshal.ReleaseComObject(pinIn);
                if (errorCode != S_OK)
                {
                    System.Diagnostics.Debug.WriteLine($"Connecting {filterItemAudioDecoder.DisplayName}: {ErrorCodeToString(errorCode)}");
                    graphBuilder.RemoveFilter(audioDecoder);

                    return errorCode;
                }
                System.Diagnostics.Debug.WriteLine($"Connecting {filterItemAudioDecoder.DisplayName}: {ErrorCodeToString(errorCode)}");
              
            }
            else
            {
                //автоматический перебор декодеров аудио.
                System.Diagnostics.Debug.WriteLine("Automatic audio decoder selection mode.");
                errorCode = FindAndConnectAudioDecoder_Manual(pinOut);

                if (errorCode != S_OK)
                {
                    return errorCode;
                }
            }

            //processing audio renderer.        
            string audioRendererName;
            if (filters.audioRenderers.Count > 0)
            {
                MonikerItem audioRendererMoniker = filters.audioRenderers[filters.audioRendererId];
                audioRendererName = audioRendererMoniker.DisplayName;
                errorCode = CreateDirectShowFilter(audioRendererMoniker.Moniker, out IBaseFilter filter);
                if (errorCode == S_OK)
                {
                    audioRenderer = filter;
                    graphBuilder.AddFilter(audioRenderer, audioRendererName);
                    
                    System.Diagnostics.Debug.WriteLine($"Loading {audioRendererName}: S_OK");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Loading {audioRendererName}: {ErrorCodeToString(errorCode)}");
                }
            }
            else
            {
                errorCode = CreateDirectShowFilter(CLSID_DirectSoundAudioRenderer, out audioRenderer);
                audioRendererName = "DirectSound audio renderer";
                if (errorCode == S_OK)
                {
                    System.Diagnostics.Debug.WriteLine($"Loading {audioRendererName}: S_OK");
                    
                    graphBuilder.AddFilter(audioRenderer, audioRendererName);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Loading {audioRendererName}: {ErrorCodeToString(errorCode)}");
                }
            }
            if (errorCode != S_OK)
            {
                graphBuilder.RemoveFilter(audioDecoder);
                return errorCode;
            }
            
            if (FindPin(audioDecoder, 0, PinDirection.Output, out pinOut) != S_OK)
            {
                System.Diagnostics.Debug.WriteLine("Audio decoder: Output pin not found!");
                graphBuilder.RemoveFilter(audioRenderer);
                graphBuilder.RemoveFilter(audioDecoder);
                return E_POINTER;
            }
            if (FindPin(audioRenderer, 0, PinDirection.Input, out pinIn) != S_OK)
            {
                System.Diagnostics.Debug.WriteLine($"{audioRendererName}: Input pin not found!");
                graphBuilder.RemoveFilter(audioRenderer);
                graphBuilder.RemoveFilter(audioDecoder);
                Marshal.ReleaseComObject(pinOut);
                return E_POINTER;
            }

            errorCode = graphBuilder.Connect(pinOut, pinIn);
            Marshal.ReleaseComObject(pinIn);
            Marshal.ReleaseComObject(pinOut);
            if (errorCode != S_OK)
            {
                System.Diagnostics.Debug.WriteLine($"Connecting {audioRendererName}: {ErrorCodeToString(errorCode)}"); 
                graphBuilder.RemoveFilter(audioDecoder);
                graphBuilder.RemoveFilter(audioRenderer);
                return errorCode;
            }
            System.Diagnostics.Debug.WriteLine($"Connecting {audioRendererName}: S_OK");

            return S_OK;
        }

        private int FindAndConnectAudioDecoder_Manual(IPin pinOut)
        {
            foreach (FilterItem item in filters.audioDecoders)
            {
                int errorCode = CreateDirectShowFilter(item.GetGuid(), out IBaseFilter filter);
                if (errorCode != S_OK)
                {
                    System.Diagnostics.Debug.WriteLine($"Loading {item.DisplayName}: {ErrorCodeToString(errorCode)}");
                    continue;
                }
                System.Diagnostics.Debug.WriteLine($"Loading {item.DisplayName}: S_OK");
                graphBuilder.AddFilter(filter, item.DisplayName);

                if (FindPin(filter, 0, PinDirection.Input, out IPin pinIn) != S_OK)
                {
                    System.Diagnostics.Debug.WriteLine($"{item.DisplayName}: Input pin not found! Skip it.");
                    graphBuilder.RemoveFilter(filter);
                    Marshal.ReleaseComObject(filter);
                    continue;
                }

                errorCode = graphBuilder.Connect(pinOut, pinIn);
                Marshal.ReleaseComObject(pinIn);
                if (errorCode != S_OK)
                {
                    System.Diagnostics.Debug.WriteLine($"Connecting {item.DisplayName}: {ErrorCodeToString(errorCode)}");
                    graphBuilder.RemoveFilter(filter);
                    Marshal.ReleaseComObject(filter);
                    continue;
                }
                System.Diagnostics.Debug.WriteLine($"Connecting {item.DisplayName}: S_OK");

                audioDecoder = filter;
                return errorCode;
            }

            System.Diagnostics.Debug.WriteLine("No valid audio decoder found!");
            return S_FALSE;
        }

        private int BuildGraphIntellectual()
        {
            int errorCode = CreateComObject<FilterGraph, IGraphBuilder>(out graphBuilder);
            if (errorCode != S_OK)
            {
                Clear();
                return errorCode;
            }

            errorCode = CreateComObject<CaptureGraphBuilder2, ICaptureGraphBuilder2>(out captureGraphBuilder2);
            if (errorCode != S_OK)
            {
                Clear();
                return errorCode;
            }

            errorCode = captureGraphBuilder2.SetFiltergraph(graphBuilder);
            if (errorCode != S_OK)
            {
                Clear();
                return errorCode;
            }
            graphBuilder.AddSourceFilter(FileName, "Source filter", out fileSourceFilter);

            //render video chain
            int errorCodeVideo = RenderVideoStream_Intellectual();
            if (errorCodeVideo != S_OK || !GetVideoInterfaces() || !ConfigureVideoOutput())
            {
                System.Diagnostics.Debug.WriteLine($"Video rendering error: {ErrorCodeToString(errorCodeVideo)}");
                ClearVideoChain();
            }

            //render audio chain
            int errorCodeAudio = RenderAudioStream_Intellectual();
            if (errorCodeAudio == S_OK)
            {
                if (GetComInterface<IBasicAudio>(graphBuilder, out basicAudio))
                {
                    basicAudio.put_Volume(GetDecibelsVolume(Volume));
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Audio rendering error: {ErrorCodeToString(errorCodeAudio)}");
                ClearAudioChain();
            }

            if (errorCodeAudio != S_OK && errorCodeVideo != S_OK)
            {
                Clear();
                return ERROR_NOTHING_RENDERED;
            }

            if (!GetComInterface<IMediaControl>(graphBuilder, out mediaControl))
            {
                Clear();
                return E_POINTER;
            }

            if (GetComInterface<IMediaEventEx>(graphBuilder, out mediaEventEx))
            {
                mediaEventEx.SetNotifyWindow(Handle, DIRECTSHOW_EVENTS_MESSAGE, new IntPtr(0));
            }

            mediaPosition = (IMediaPosition)graphBuilder;

            _state = PLAYER_STATE.Stopped;

            return S_OK;
        }

        private int RenderVideoStream_Intellectual_Enumeration()
        {
            foreach (FilterItem item in filters.videoDecoders)
            {
                int errorCode = CreateDirectShowFilter(item.GetGuid(), out IBaseFilter filter);
                if (errorCode != S_OK)
                {
                    System.Diagnostics.Debug.WriteLine($"Loading {item.DisplayName}: {ErrorCodeToString(errorCode)}");
                    continue;
                }
                System.Diagnostics.Debug.WriteLine($"Loading {item.DisplayName}: S_OK");
                graphBuilder.AddFilter(filter, item.DisplayName);

                errorCode = captureGraphBuilder2.RenderStream(null, MediaType.Video, fileSourceFilter, filter, videoRenderer);
                if (errorCode != S_OK)
                {
                    System.Diagnostics.Debug.WriteLine($"Rendering with {item.DisplayName}: {ErrorCodeToString(errorCode)}");
                    graphBuilder.RemoveFilter(filter);
                    Marshal.ReleaseComObject(filter);
                    continue;
                }

                System.Diagnostics.Debug.WriteLine($"{item.DisplayName}: Successfully rendered video stream!");
                videoDecoder = filter;
                return S_OK;
            }

            System.Diagnostics.Debug.WriteLine("No valid video decoder found!");
            return S_FALSE;
        }

        private int RenderVideoStream_Intellectual()
        { 
            FilterItem filterItem = filters.videoRenderers[filters.videoRendererId];
            int errorCode = CreateDirectShowFilter(filterItem.GetGuid(), out videoRenderer);
            if (errorCode != S_OK)
            {
                System.Diagnostics.Debug.WriteLine($"Loading {filterItem.DisplayName}: {ErrorCodeToString(errorCode)}");
                return errorCode;
            }
            System.Diagnostics.Debug.WriteLine($"Loading {filterItem.DisplayName}: S_OK");
            graphBuilder.AddFilter(videoRenderer, filterItem.DisplayName);

            if (filters.videoDecoderId < 0)
            {
                System.Diagnostics.Debug.WriteLine("Automatic video decoder selection mode.");
                errorCode = RenderVideoStream_Intellectual_Enumeration();
                if (errorCode != S_OK)
                {
                    graphBuilder.RemoveFilter(videoRenderer);
                }
                return errorCode;
            }

            filterItem = filters.videoDecoders[filters.videoDecoderId];
            errorCode = CreateDirectShowFilter(filterItem.GetGuid(), out videoDecoder);
            if (errorCode != S_OK)
            {
                System.Diagnostics.Debug.WriteLine($"Loading {filterItem.DisplayName}: {ErrorCodeToString(errorCode)}");
                graphBuilder.RemoveFilter(videoRenderer);
                return errorCode;
            }
            System.Diagnostics.Debug.WriteLine($"Loading {filterItem.DisplayName}: S_OK");

            graphBuilder.AddFilter(videoDecoder, filterItem.DisplayName);

            errorCode = captureGraphBuilder2.RenderStream(null, MediaType.Video, fileSourceFilter, videoDecoder, videoRenderer);
            if (errorCode != S_OK)
            {
                System.Diagnostics.Debug.WriteLine($"Rendering with {filterItem.DisplayName} failed! {ErrorCodeToString(errorCode)}");

                graphBuilder.RemoveFilter(videoDecoder);
                graphBuilder.RemoveFilter(videoRenderer);
                return errorCode;
            }

            System.Diagnostics.Debug.WriteLine($"{filterItem.DisplayName}: Successfully rendered video stream!");

            return S_OK;
        }

        private int RenderAudioStream_Intellectual_Enumeration()
        {
            foreach (FilterItem item in filters.audioDecoders)
            {
                int errorCode = CreateDirectShowFilter(item.GetGuid(), out IBaseFilter filter);
                if (errorCode != S_OK)
                {
                    System.Diagnostics.Debug.WriteLine($"Loading {item.DisplayName}: {ErrorCodeToString(errorCode)}");
                    continue;
                }
                System.Diagnostics.Debug.WriteLine($"Loading {item.DisplayName}: S_OK");
                graphBuilder.AddFilter(filter, item.DisplayName);

                errorCode = captureGraphBuilder2.RenderStream(null, MediaType.Audio, fileSourceFilter, filter, audioRenderer);
                if (errorCode != S_OK)
                {
                    System.Diagnostics.Debug.WriteLine($"{item.DisplayName}: Audio stream rendering failed! {ErrorCodeToString(errorCode)}");
                    graphBuilder.RemoveFilter(filter);
                    continue;
                }

                System.Diagnostics.Debug.WriteLine($"{item.DisplayName}: Successfully rendered audio stream!");
                audioRenderer = filter;
                return S_OK;
            }

            System.Diagnostics.Debug.WriteLine("No valid audio decoder found!");
            return S_FALSE;
        }

        private int RenderAudioStream_Intellectual()
        {
            MonikerItem audioRendererMoniker = filters.audioRenderers[filters.audioRendererId];
            int errorCode = CreateDirectShowFilter(audioRendererMoniker.Moniker, out audioRenderer);
            if (errorCode != S_OK)
            {
                System.Diagnostics.Debug.WriteLine($"Loading {audioRendererMoniker.DisplayName}: {ErrorCodeToString(errorCode)}");
                return errorCode;
            }
            System.Diagnostics.Debug.WriteLine($"Loading {audioRendererMoniker.DisplayName}: S_OK");
            graphBuilder.AddFilter(audioRenderer, audioRendererMoniker.DisplayName);

            if (filters.audioDecoderId < 0)
            {
                System.Diagnostics.Debug.WriteLine("Automatic audio decoder selection mode.");
                errorCode = RenderAudioStream_Intellectual_Enumeration();
                if (errorCode != S_OK)
                {
                    graphBuilder.RemoveFilter(audioRenderer);
                }
                return errorCode;
            }

            FilterItem filterItem = filters.audioDecoders[filters.audioDecoderId];
            errorCode = CreateDirectShowFilter(filterItem.GetGuid(), out audioDecoder);
            if (errorCode != S_OK)
            {
                System.Diagnostics.Debug.WriteLine($"Loading {filterItem.DisplayName}: {ErrorCodeToString(errorCode)}");
                graphBuilder.RemoveFilter(audioRenderer);
                return errorCode;
            }
            System.Diagnostics.Debug.WriteLine($"Loading {filterItem.DisplayName}: S_OK");
            graphBuilder.AddFilter(audioDecoder, filterItem.DisplayName);

            errorCode = captureGraphBuilder2.RenderStream(null, MediaType.Audio, fileSourceFilter, audioDecoder, audioRenderer);
            if (errorCode != S_OK)
            {
                System.Diagnostics.Debug.WriteLine($"Rendering with {filterItem.DisplayName} failed! {ErrorCodeToString(errorCode)}");
                graphBuilder.RemoveFilter(audioDecoder);
                graphBuilder.RemoveFilter(audioRenderer);
                return errorCode;
            }

            System.Diagnostics.Debug.WriteLine($"{filterItem.DisplayName}: Successfully rendered audio stream!");

            return S_OK;
        }

        private bool GetVideoInterfaces()
        {
            basicVideo = (IBasicVideo)graphBuilder;
            if (basicVideo != null)
            {
                videoWindow = (IVideoWindow)graphBuilder;
                if (videoWindow != null)
                {
                    return true;
                }
            }
            return false;
        }

        public bool ConfigureVideoOutput()
        {
            if (OutputWindow == null || videoWindow == null || basicVideo == null || 
                basicVideo.GetVideoSize(out int w, out int h) != S_OK)
            {
                return false;
            }

            videoWindow.put_Owner(OutputWindow.Handle);
            videoWindow.put_MessageDrain(OutputWindow.Handle);
            videoWindow.put_WindowStyle(WindowStyle.Child | WindowStyle.ClipChildren | WindowStyle.ClipSiblings);

            _videoWidth = w;
            _videoHeight = h;

            Rectangle r = CenterRect(ResizeRect(new Rectangle(0, 0, _videoWidth, _videoHeight), OutputWindow.Size), OutputWindow.ClientRectangle);
            SetScreenRect(r);
            videoWindow.put_Visible(OABool.True);

            return true;
        }

        public int Play()
        {
            int res = S_OK;
            if (State == PLAYER_STATE.Null)
            {
                res = BuildGraph();
            }
            if (res == S_OK)
            {
                _state = PLAYER_STATE.Playing;
                mediaControl.Run();
                return S_OK;
            }
            return res;
        }
       
        public bool Pause()
        {
            if (State != PLAYER_STATE.Null && mediaControl != null)
            {
                mediaControl.Pause();
                _state = PLAYER_STATE.Paused;
                return true;
            }
            return false;
        }

        public bool Stop()
        {
            if (State != PLAYER_STATE.Null && mediaControl != null)
            {
                mediaControl.Stop();
                _state = PLAYER_STATE.Stopped;
                return true;
            }
            return false;
        }

        public void Seek(double seconds)
        {
            Position += seconds;
        }

        public void SetScreenRect(Rectangle rectangle)
        {
            videoWindow.SetWindowPosition(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
            _outputScreenRect = rectangle;
        }

        private void ClearVideoChain()
        {
            if (basicVideo != null)
            {
                Marshal.ReleaseComObject(basicVideo);
                basicVideo = null;
            }

            if (videoWindow != null)
            {
                Marshal.ReleaseComObject(videoWindow);
                videoWindow = null;
            }

            if (videoRenderer != null)
            {
                Marshal.ReleaseComObject(videoRenderer);
                videoRenderer = null;
            }

            if (videoDecoder != null)
            {
                Marshal.ReleaseComObject(videoDecoder);
                videoDecoder = null;
            }

            _videoWidth = _videoHeight = 0;
        }

        private void ClearAudioChain()
        {
            if (basicAudio != null)
            {
                Marshal.ReleaseComObject(basicAudio);
                basicAudio = null;
            }

            if (audioRenderer != null)
            {
                Marshal.ReleaseComObject(audioRenderer);
                audioRenderer = null;
            }

            if (audioDecoder != null)
            {
                Marshal.ReleaseComObject(audioDecoder);
                audioDecoder = null;
            }
        }

        public void Clear()
        {
            Stop();
            ClearAudioChain();
            ClearVideoChain();

            if (mediaEventEx != null)
            {
                Marshal.ReleaseComObject(mediaEventEx);
                mediaEventEx = null;
            }

            if (mediaPosition != null)
            {
                Marshal.ReleaseComObject(mediaPosition);
                mediaPosition = null;
            }

            if (mediaControl != null)
            {
                Marshal.ReleaseComObject(mediaControl);
                mediaControl = null;
            }

            if (mediaSplitter != null)
            {
                Marshal.ReleaseComObject(mediaSplitter);
                mediaSplitter = null;
            }

            if (fileSource != null)
            {
                Marshal.ReleaseComObject(fileSource);
                fileSource = null;
            }

            if (fileSourceFilter != null)
            {
                Marshal.ReleaseComObject(fileSourceFilter);
                fileSourceFilter = null;
            }

            if (captureGraphBuilder2 != null)
            {
                Marshal.ReleaseComObject(captureGraphBuilder2);
                captureGraphBuilder2 = null;
            }

            if (graphBuilder != null)
            {
                Marshal.ReleaseComObject(graphBuilder);
                graphBuilder = null;
            }

            _state = PLAYER_STATE.Null;

            Cleared?.Invoke(this);
        }

        public static int GetDecibelsVolume(int volume)
        {
            long vol = volume * UInt16.MaxValue / 100;
            int db = (int)Math.Truncate(100 * 33.22 * Math.Log((vol + 1e-6) / UInt16.MaxValue) / Math.Log(10));
            if (db < -10000)
            {
                db = -10000;
            }
            else if (db > 0)
            {
                db = 0;
            }
            return db;
        }

        public static string ErrorCodeToString(int errorCode)
        {
            switch (errorCode)
            {
                case ERROR_FILE_NOT_FOUND:
                    return "File not found";
                case ERROR_FILE_NAME_NOT_DEFINED:
                    return "File name not defined";
                case ERROR_NOTHING_RENDERED:
                    return "ERROR_NOTHING_RENDERED";

                default:
                    return DirectShowUtils.ErrorCodeToString(errorCode);
            }
        }

    }

}
