using System;
using System.Runtime.InteropServices;
using Opc;
using Opc.Ae;
using Opc.Da;
using OpcRcw.Ae;

namespace OpcCom.Ae
{
    public class Interop
    {
        internal static OpcRcw.Ae.FILETIME Convert(System.Runtime.InteropServices.ComTypes.FILETIME input)
        {
            OpcRcw.Ae.FILETIME result = default(OpcRcw.Ae.FILETIME);
            result.dwLowDateTime = input.dwLowDateTime;
            result.dwHighDateTime = input.dwHighDateTime;
            return result;
        }

        internal static System.Runtime.InteropServices.ComTypes.FILETIME Convert(OpcRcw.Ae.FILETIME input)
        {
            System.Runtime.InteropServices.ComTypes.FILETIME result = default(System.Runtime.InteropServices.ComTypes.FILETIME);
            result.dwLowDateTime = input.dwLowDateTime;
            result.dwHighDateTime = input.dwHighDateTime;
            return result;
        }

        internal static ResultID GetResultID(int input)
        {
            if (input == -1073479165)
            {
                return ResultID.Ae.E_INVALIDBRANCHNAME;
            }

            return OpcCom.Interop.GetResultID(input);
        }

        internal static Opc.Ae.ServerStatus GetServerStatus(ref IntPtr pInput, bool deallocate)
        {
            Opc.Ae.ServerStatus serverStatus = null;
            if (pInput != IntPtr.Zero)
            {
                OPCEVENTSERVERSTATUS oPCEVENTSERVERSTATUS = (OPCEVENTSERVERSTATUS)Marshal.PtrToStructure(pInput, typeof(OPCEVENTSERVERSTATUS));
                serverStatus = new Opc.Ae.ServerStatus();
                serverStatus.VendorInfo = oPCEVENTSERVERSTATUS.szVendorInfo;
                serverStatus.ProductVersion = $"{oPCEVENTSERVERSTATUS.wMajorVersion}.{oPCEVENTSERVERSTATUS.wMinorVersion}.{oPCEVENTSERVERSTATUS.wBuildNumber}";
                serverStatus.ServerState = (ServerState)oPCEVENTSERVERSTATUS.dwServerState;
                serverStatus.StatusInfo = null;
                serverStatus.StartTime = OpcCom.Interop.GetFILETIME(Convert(oPCEVENTSERVERSTATUS.ftStartTime));
                serverStatus.CurrentTime = OpcCom.Interop.GetFILETIME(Convert(oPCEVENTSERVERSTATUS.ftCurrentTime));
                serverStatus.LastUpdateTime = OpcCom.Interop.GetFILETIME(Convert(oPCEVENTSERVERSTATUS.ftLastUpdateTime));
                if (deallocate)
                {
                    Marshal.DestroyStructure(pInput, typeof(OPCEVENTSERVERSTATUS));
                    Marshal.FreeCoTaskMem(pInput);
                    pInput = IntPtr.Zero;
                }
            }

            return serverStatus;
        }

        internal static OPCAEBROWSETYPE GetBrowseType(BrowseType input)
        {
            switch (input)
            {
                case BrowseType.Area:
                    return OPCAEBROWSETYPE.OPC_AREA;
                case BrowseType.Source:
                    return OPCAEBROWSETYPE.OPC_SOURCE;
                default:
                    return OPCAEBROWSETYPE.OPC_AREA;
            }
        }

        internal static EventNotification[] GetEventNotifications(ONEVENTSTRUCT[] input)
        {
            EventNotification[] array = null;
            if (input != null && input.Length > 0)
            {
                array = new EventNotification[input.Length];
                for (int i = 0; i < input.Length; i++)
                {
                    array[i] = GetEventNotification(input[i]);
                }
            }

            return array;
        }

        internal static EventNotification GetEventNotification(ONEVENTSTRUCT input)
        {
            EventNotification eventNotification = new EventNotification();
            eventNotification.SourceID = input.szSource;
            eventNotification.Time = OpcCom.Interop.GetFILETIME(Convert(input.ftTime));
            eventNotification.Severity = input.dwSeverity;
            eventNotification.Message = input.szMessage;
            eventNotification.EventType = (EventType)input.dwEventType;
            eventNotification.EventCategory = input.dwEventCategory;
            eventNotification.ChangeMask = input.wChangeMask;
            eventNotification.NewState = input.wNewState;
            eventNotification.Quality = new Quality(input.wQuality);
            eventNotification.ConditionName = input.szConditionName;
            eventNotification.SubConditionName = input.szSubconditionName;
            eventNotification.AckRequired = (input.bAckRequired != 0);
            eventNotification.ActiveTime = OpcCom.Interop.GetFILETIME(Convert(input.ftActiveTime));
            eventNotification.Cookie = input.dwCookie;
            eventNotification.ActorID = input.szActorID;
            object[] vARIANTs = OpcCom.Interop.GetVARIANTs(ref input.pEventAttributes, input.dwNumEventAttrs, deallocate: false);
            eventNotification.SetAttributes(vARIANTs);
            return eventNotification;
        }

        internal static Condition[] GetConditions(ref IntPtr pInput, int count, bool deallocate)
        {
            Condition[] array = null;
            if (pInput != IntPtr.Zero && count > 0)
            {
                array = new Condition[count];
                IntPtr ptr = pInput;
                for (int i = 0; i < count; i++)
                {
                    OPCCONDITIONSTATE oPCCONDITIONSTATE = (OPCCONDITIONSTATE)Marshal.PtrToStructure(ptr, typeof(OPCCONDITIONSTATE));
                    array[i] = new Condition();
                    array[i].State = oPCCONDITIONSTATE.wState;
                    array[i].Quality = new Quality(oPCCONDITIONSTATE.wQuality);
                    array[i].Comment = oPCCONDITIONSTATE.szComment;
                    array[i].AcknowledgerID = oPCCONDITIONSTATE.szAcknowledgerID;
                    array[i].CondLastActive = OpcCom.Interop.GetFILETIME(Convert(oPCCONDITIONSTATE.ftCondLastActive));
                    array[i].CondLastInactive = OpcCom.Interop.GetFILETIME(Convert(oPCCONDITIONSTATE.ftCondLastInactive));
                    array[i].SubCondLastActive = OpcCom.Interop.GetFILETIME(Convert(oPCCONDITIONSTATE.ftSubCondLastActive));
                    array[i].LastAckTime = OpcCom.Interop.GetFILETIME(Convert(oPCCONDITIONSTATE.ftLastAckTime));
                    array[i].ActiveSubCondition.Name = oPCCONDITIONSTATE.szActiveSubCondition;
                    array[i].ActiveSubCondition.Definition = oPCCONDITIONSTATE.szASCDefinition;
                    array[i].ActiveSubCondition.Severity = oPCCONDITIONSTATE.dwASCSeverity;
                    array[i].ActiveSubCondition.Description = oPCCONDITIONSTATE.szASCDescription;
                    string[] unicodeStrings = OpcCom.Interop.GetUnicodeStrings(ref oPCCONDITIONSTATE.pszSCNames, oPCCONDITIONSTATE.dwNumSCs, deallocate);
                    int[] int32s = OpcCom.Interop.GetInt32s(ref oPCCONDITIONSTATE.pdwSCSeverities, oPCCONDITIONSTATE.dwNumSCs, deallocate);
                    string[] unicodeStrings2 = OpcCom.Interop.GetUnicodeStrings(ref oPCCONDITIONSTATE.pszSCDefinitions, oPCCONDITIONSTATE.dwNumSCs, deallocate);
                    string[] unicodeStrings3 = OpcCom.Interop.GetUnicodeStrings(ref oPCCONDITIONSTATE.pszSCDescriptions, oPCCONDITIONSTATE.dwNumSCs, deallocate);
                    array[i].SubConditions.Clear();
                    if (oPCCONDITIONSTATE.dwNumSCs > 0)
                    {
                        for (int j = 0; j < unicodeStrings.Length; j++)
                        {
                            SubCondition subCondition = new SubCondition();
                            subCondition.Name = unicodeStrings[j];
                            subCondition.Severity = int32s[j];
                            subCondition.Definition = unicodeStrings2[j];
                            subCondition.Description = unicodeStrings3[j];
                            array[i].SubConditions.Add(subCondition);
                        }
                    }

                    object[] vARIANTs = OpcCom.Interop.GetVARIANTs(ref oPCCONDITIONSTATE.pEventAttributes, oPCCONDITIONSTATE.dwNumEventAttrs, deallocate);
                    int[] int32s2 = OpcCom.Interop.GetInt32s(ref oPCCONDITIONSTATE.pErrors, oPCCONDITIONSTATE.dwNumEventAttrs, deallocate);
                    array[i].Attributes.Clear();
                    if (oPCCONDITIONSTATE.dwNumEventAttrs > 0)
                    {
                        for (int k = 0; k < vARIANTs.Length; k++)
                        {
                            AttributeValue attributeValue = new AttributeValue();
                            attributeValue.ID = 0;
                            attributeValue.Value = vARIANTs[k];
                            attributeValue.ResultID = GetResultID(int32s2[k]);
                            array[i].Attributes.Add(attributeValue);
                        }
                    }

                    if (deallocate)
                    {
                        Marshal.DestroyStructure(ptr, typeof(OPCCONDITIONSTATE));
                    }

                    ptr = (IntPtr)(ptr.ToInt64() + Marshal.SizeOf(typeof(OPCCONDITIONSTATE)));
                }

                if (deallocate)
                {
                    Marshal.FreeCoTaskMem(pInput);
                    pInput = IntPtr.Zero;
                }
            }

            return array;
        }
    }
}
