using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using DirectShowLib;

namespace Zeratool_player_C_Sharp
{
    public static class DirectShowUtils
    {
        public const int S_OK = 0;
        public const int S_FALSE = 1;
        public const int E_FAIL = -2147467259; //0x80004005
        public const int CLASS_E_NOAGGREGATION = -2147221232; //0x80040110
        public const int REGDB_E_CLASSNOTREG = -2147221164; //0x80040154
        public const int E_NOINTERFACE = -2147467262; //0x80004002
        public const int E_POINTER = -2147467261; //0x80004003
        public const int VFW_E_CANNOT_CONNECT = -2147220969; //0x80040217

        public static readonly Guid CLSID_FileSourceAsync = new Guid("{E436EBB5-524F-11CE-9F53-0020AF0BA770}");
        public static readonly Guid CLSID_LAV_Splitter = new Guid("{171252A0-8820-4AFE-9DF8-5C92B2D66B04}");
        public static readonly Guid CLSID_HaaliMediaSplitter = new Guid("{55DA30FC-F16B-49FC-BAA5-AE59FC65F82D}");
        public static readonly Guid CLSID_HaaliMediaSplitterAR = new Guid("{564FD788-86C9-4444-971E-CC4A243DA150}");
        public static readonly Guid CLSID_MPEG2Splitter = new Guid("{3AE86B20-7BE8-11D1-ABE6-00A0C905F375}");
        public static readonly Guid CLSID_LAV_VideoDecoder = new Guid("{EE30215D-164F-4A92-A4EB-9D4C13390F9F}");
        public static readonly Guid CLSID_FFDShowVideoDecoder = new Guid("{04FE9017-F873-410E-871E-AB91661A4EF7}");
        public static readonly Guid CLSID_LAV_AudioDecoder = new Guid("{E8E73B6B-4CB3-44A4-BE99-4F7BCB96E491}");
        public static readonly Guid CLSID_FFDShowAudioDecoder = new Guid("{0F40E1E5-4F79-4988-B1A9-CC98794E6B55}");
        public static readonly Guid CLSID_AC3Filter = new Guid("{A753A1EC-973E-4718-AF8E-A3F554D45C44}");
        public static readonly Guid CLSID_DirectSoundAudioRenderer = new Guid("{79376820-07D0-11CF-A24D-0020AFD79767}");

        public static readonly Guid CLSID_VideoMixingRenderer9 = new Guid("{51B4ABF3-748F-4E3B-A276-C828330E926A}");
        public static readonly Guid CLSID_EnhancedVideoRenderer = new Guid("{FA10746C-9B63-4B6C-BC49-FC300EA5F256}");
        public static readonly Guid CLSID_HaaliVideoRenderer = new Guid("{760A8F35-97E7-479D-AAF5-DA9EFF95D751}");
        public static readonly Guid CLSID_DefaultVideoRenderer = new Guid("{6BC1CFFA-8FC1-4261-AC22-CFB4CC38DB50}");
        public static readonly Guid CLSID_VideoRenderer = new Guid("{70E102B0-5556-11CE-97C0-00AA0055595A}");

        public static readonly Guid CLSID_AudioRendererCategory = new Guid("{E0F158E1-CB04-11D0-BD4E-00A0C911CE86}");

        public static int CreateComObject<T, T2>(out T2 obj) where T : new()
        {
            try
            {
                object o = new T();
                obj = (T2)o;
                return S_OK;
            }
            catch (Exception ex)
            {
                obj = default;
                return ex.HResult;
            }
        }

        public static int CreateDirectShowFilter(Guid guid, out IBaseFilter filter)
        {
            Type type = Type.GetTypeFromCLSID(guid);
            try
            {
                filter = (IBaseFilter)Activator.CreateInstance(type);
                return S_OK;
            }
            catch (Exception ex)
            {
                filter = null;
                return ex.HResult;
            }
        }

        public static int CreateDirectShowFilter(IMoniker moniker, out IBaseFilter filter)
        {
            object obj = null;
            try
            {
                Guid guid = typeof(IBaseFilter).GUID;
                moniker.BindToObject(null, null, ref guid, out obj);
                if (obj != null)
                {
                    filter = (IBaseFilter)obj;
                    return S_OK;
                }
                filter = null;
                return E_POINTER;
            }
            catch (Exception ex)
            {
                if (obj != null)
                {
                    Marshal.ReleaseComObject(obj);
                }
                filter = null;
                return ex.HResult;
            }
        }

        public static bool GetComInterface<T>(IGraphBuilder graphBuilder, out T obj)
        {
            obj = (T)graphBuilder;
            return obj != null;
        }

        public static int FindPin(IBaseFilter filter, int pinId, PinDirection pinDirection, out IPin resultPin)
        {
            if (filter != null && filter.EnumPins(out IEnumPins enumPins) == S_OK)
            {
                int id = 0;
                IPin[] pins = new IPin[1];
                while (enumPins.Next(1, pins, new IntPtr(0)) == S_OK)
                {
                    if (pins[0].QueryDirection(out PinDirection dir) == S_OK && dir == pinDirection)
                    {
                        if (pinId == id)
                        {
                            Marshal.ReleaseComObject(enumPins);
                            resultPin = pins[0];
                            return S_OK;
                        }
                        id++;
                    }
                    Marshal.ReleaseComObject(pins[0]);
                }
                Marshal.ReleaseComObject(enumPins);
            }
            
            resultPin = null;
            return S_FALSE;
        }

        public static int FindPin(IBaseFilter filter, string pinName, PinDirection pinDirection, out IPin resultPin)
        {
            if (filter != null && filter.EnumPins(out IEnumPins enumPins) == S_OK)
            {
                IPin[] pins = new IPin[1];
                while (enumPins.Next(1, pins, new IntPtr(0)) == S_OK)
                {
                    if (pins[0].QueryPinInfo(out PinInfo pinInfo) == S_OK &&
                        pinInfo.dir == pinDirection && pinInfo.name.Contains(pinName))
                    {
                        Marshal.ReleaseComObject(enumPins);
                        resultPin = pins[0];
                        return S_OK;
                    }
                    Marshal.ReleaseComObject(pins[0]);
                }
                Marshal.ReleaseComObject(enumPins);
            }

            resultPin = null;
            return S_FALSE;
        }

        public static int ListAudioRenderers(List<MonikerItem> list)
        {
            int errorCode = CreateComObject<CreateDevEnum, ICreateDevEnum>(out ICreateDevEnum deviceEnum);
            if (errorCode != S_OK)
            {
                return errorCode;
            }

            errorCode = deviceEnum.CreateClassEnumerator(CLSID_AudioRendererCategory, out IEnumMoniker enumMoniker, CDef.None);
            if (errorCode != S_OK)
            {
                Marshal.ReleaseComObject(enumMoniker);
                return errorCode;
            }

            IMoniker[] monikers = new IMoniker[1];
            while (enumMoniker.Next(1, monikers, new IntPtr(0)) == S_OK)
            {
                Guid bagId = typeof(IPropertyBag).GUID;
                object bagObj = null;
                try
                {
                    monikers[0].BindToStorage(null, null, ref bagId, out bagObj);
                    if (bagObj != null)
                    {
                        IPropertyBag propertyBag = (IPropertyBag)bagObj;
                        if (propertyBag != null)
                        {
                            errorCode = propertyBag.Read("FriendlyName", out object t, null);
                            if (errorCode == S_OK)
                            {
                                list.Add(new MonikerItem(monikers[0], propertyBag, (string)t));
                            }
                            else
                            {
                                Marshal.ReleaseComObject(monikers[0]);
                            }
                        }
                        Marshal.ReleaseComObject(bagObj);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                    if (bagObj != null)
                    {
                        Marshal.ReleaseComObject(bagObj);
                    }
                }
            }
            Marshal.ReleaseComObject(deviceEnum);

            return errorCode;
        }

        public static string ErrorCodeToString(int errorCode)
        {
            switch (errorCode)
            {
                case S_OK: return "S_OK";
                case S_FALSE: return "S_FALSE";
                case E_FAIL: return "E_FAIL";
                case CLASS_E_NOAGGREGATION: return "CLASS_E_NOAGGREGATION";
                case REGDB_E_CLASSNOTREG: return "REGDB_E_CLASSNOTREG";
                case E_NOINTERFACE: return "E_NOINTERFACE";
                case E_POINTER: return "E_POINTER";
                case VFW_E_CANNOT_CONNECT: return "VFW_E_CANNOT_CONNECT";

                default: return $"Error {errorCode}";
            }
        }
    }
}
