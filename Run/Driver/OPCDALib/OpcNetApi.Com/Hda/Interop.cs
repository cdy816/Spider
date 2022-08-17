using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using Opc.Da;
using Opc.Hda;
using OpcRcw.Hda;

namespace OpcCom.Hda
{
    // Token: 0x02000017 RID: 23
    public class Interop
    {
        internal static OPCHDA_FILETIME Convert(FILETIME input)
        {
            OPCHDA_FILETIME result = default(OPCHDA_FILETIME);
            result.dwLowDateTime = input.dwLowDateTime;
            result.dwHighDateTime = input.dwHighDateTime;
            return result;
        }

        internal static FILETIME Convert(OPCHDA_FILETIME input)
        {
            FILETIME result = default(FILETIME);
            result.dwLowDateTime = input.dwLowDateTime;
            result.dwHighDateTime = input.dwHighDateTime;
            return result;
        }

        internal static OPCHDA_FILETIME GetFILETIME(decimal input)
        {
            OPCHDA_FILETIME result = default(OPCHDA_FILETIME);
            result.dwHighDateTime = (int)((ulong)((long)(ulong)(input * 10000000m) & -4294967296L) >> 32);
            result.dwLowDateTime = (int)((ulong)(input * 10000000m) & uint.MaxValue);
            return result;
        }

        internal static OPCHDA_FILETIME[] GetFILETIMEs(DateTime[] input)
        {
            OPCHDA_FILETIME[] array = null;
            if (input != null)
            {
                array = new OPCHDA_FILETIME[input.Length];
                for (int i = 0; i < input.Length; i++)
                {
                    ref OPCHDA_FILETIME reference = ref array[i];
                    reference = Convert(OpcCom.Interop.GetFILETIME(input[i]));
                }
            }

            return array;
        }

        internal static OPCHDA_TIME GetTime(Time input)
        {
            OPCHDA_TIME result = default(OPCHDA_TIME);
            if (input != null)
            {
                result.ftTime = Convert(OpcCom.Interop.GetFILETIME(input.AbsoluteTime));
                result.szTime = (input.IsRelative ? input.ToString() : "");
                result.bString = (input.IsRelative ? 1 : 0);
            }
            else
            {
                result.ftTime = Convert(OpcCom.Interop.GetFILETIME(DateTime.MinValue));
                result.szTime = "";
                result.bString = 1;
            }

            return result;
        }

        internal static ItemValueCollection[] GetItemValueCollections(ref IntPtr pInput, int count, bool deallocate)
        {
            ItemValueCollection[] array = null;
            if (pInput != IntPtr.Zero && count > 0)
            {
                array = new ItemValueCollection[count];
                IntPtr pInput2 = pInput;
                for (int i = 0; i < count; i++)
                {
                    array[i] = GetItemValueCollection(pInput2, deallocate);
                    pInput2 = (IntPtr)(pInput2.ToInt64() + Marshal.SizeOf(typeof(OPCHDA_ITEM)));
                }

                if (deallocate)
                {
                    Marshal.FreeCoTaskMem(pInput);
                    pInput = IntPtr.Zero;
                }
            }

            return array;
        }

        internal static ItemValueCollection GetItemValueCollection(IntPtr pInput, bool deallocate)
        {
            ItemValueCollection result = null;
            if (pInput != IntPtr.Zero)
            {
                object obj = Marshal.PtrToStructure(pInput, typeof(OPCHDA_ITEM));
                result = GetItemValueCollection((OPCHDA_ITEM)obj, deallocate);
                if (deallocate)
                {
                    Marshal.DestroyStructure(pInput, typeof(OPCHDA_ITEM));
                }
            }

            return result;
        }

        internal static ItemValueCollection GetItemValueCollection(OPCHDA_ITEM input, bool deallocate)
        {
            ItemValueCollection itemValueCollection = new ItemValueCollection();
            itemValueCollection.ClientHandle = input.hClient;
            itemValueCollection.AggregateID = input.haAggregate;
            object[] vARIANTs = OpcCom.Interop.GetVARIANTs(ref input.pvDataValues, input.dwCount, deallocate);
            DateTime[] fILETIMEs = OpcCom.Interop.GetFILETIMEs(ref input.pftTimeStamps, input.dwCount, deallocate);
            int[] int32s = OpcCom.Interop.GetInt32s(ref input.pdwQualities, input.dwCount, deallocate);
            for (int i = 0; i < input.dwCount; i++)
            {
                Opc.Hda.ItemValue itemValue = new Opc.Hda.ItemValue();
                itemValue.Value = vARIANTs[i];
                itemValue.Timestamp = fILETIMEs[i];
                itemValue.Quality = new Opc.Da.Quality((short)(int32s[i] & 0xFFFF));
                itemValue.HistorianQuality = (Opc.Hda.Quality)(int32s[i] & 4294901760u);
                itemValueCollection.Add(itemValue);
            }

            return itemValueCollection;
        }

        internal static ModifiedValueCollection[] GetModifiedValueCollections(ref IntPtr pInput, int count, bool deallocate)
        {
            ModifiedValueCollection[] array = null;
            if (pInput != IntPtr.Zero && count > 0)
            {
                array = new ModifiedValueCollection[count];
                IntPtr pInput2 = pInput;
                for (int i = 0; i < count; i++)
                {
                    array[i] = GetModifiedValueCollection(pInput2, deallocate);
                    pInput2 = (IntPtr)(pInput2.ToInt64() + Marshal.SizeOf(typeof(OPCHDA_MODIFIEDITEM)));
                }

                if (deallocate)
                {
                    Marshal.FreeCoTaskMem(pInput);
                    pInput = IntPtr.Zero;
                }
            }

            return array;
        }

        internal static ModifiedValueCollection GetModifiedValueCollection(IntPtr pInput, bool deallocate)
        {
            ModifiedValueCollection result = null;
            if (pInput != IntPtr.Zero)
            {
                object obj = Marshal.PtrToStructure(pInput, typeof(OPCHDA_MODIFIEDITEM));
                result = GetModifiedValueCollection((OPCHDA_MODIFIEDITEM)obj, deallocate);
                if (deallocate)
                {
                    Marshal.DestroyStructure(pInput, typeof(OPCHDA_MODIFIEDITEM));
                }
            }

            return result;
        }

        internal static ModifiedValueCollection GetModifiedValueCollection(OPCHDA_MODIFIEDITEM input, bool deallocate)
        {
            ModifiedValueCollection modifiedValueCollection = new ModifiedValueCollection();
            modifiedValueCollection.ClientHandle = input.hClient;
            object[] vARIANTs = OpcCom.Interop.GetVARIANTs(ref input.pvDataValues, input.dwCount, deallocate);
            DateTime[] fILETIMEs = OpcCom.Interop.GetFILETIMEs(ref input.pftTimeStamps, input.dwCount, deallocate);
            int[] int32s = OpcCom.Interop.GetInt32s(ref input.pdwQualities, input.dwCount, deallocate);
            DateTime[] fILETIMEs2 = OpcCom.Interop.GetFILETIMEs(ref input.pftModificationTime, input.dwCount, deallocate);
            int[] int32s2 = OpcCom.Interop.GetInt32s(ref input.pEditType, input.dwCount, deallocate);
            string[] unicodeStrings = OpcCom.Interop.GetUnicodeStrings(ref input.szUser, input.dwCount, deallocate);
            for (int i = 0; i < input.dwCount; i++)
            {
                ModifiedValue modifiedValue = new ModifiedValue();
                modifiedValue.Value = vARIANTs[i];
                modifiedValue.Timestamp = fILETIMEs[i];
                modifiedValue.Quality = new Opc.Da.Quality((short)(int32s[i] & 0xFFFF));
                modifiedValue.HistorianQuality = (Opc.Hda.Quality)(int32s[i] & 4294901760u);
                modifiedValue.ModificationTime = fILETIMEs2[i];
                modifiedValue.EditType = (EditType)int32s2[i];
                modifiedValue.User = unicodeStrings[i];
                modifiedValueCollection.Add(modifiedValue);
            }

            return modifiedValueCollection;
        }

        internal static AttributeValueCollection[] GetAttributeValueCollections(ref IntPtr pInput, int count, bool deallocate)
        {
            AttributeValueCollection[] array = null;
            if (pInput != IntPtr.Zero && count > 0)
            {
                array = new AttributeValueCollection[count];
                IntPtr pInput2 = pInput;
                for (int i = 0; i < count; i++)
                {
                    array[i] = GetAttributeValueCollection(pInput2, deallocate);
                    pInput2 = (IntPtr)(pInput2.ToInt64() + Marshal.SizeOf(typeof(OPCHDA_ATTRIBUTE)));
                }

                if (deallocate)
                {
                    Marshal.FreeCoTaskMem(pInput);
                    pInput = IntPtr.Zero;
                }
            }

            return array;
        }

        internal static AttributeValueCollection GetAttributeValueCollection(IntPtr pInput, bool deallocate)
        {
            AttributeValueCollection result = null;
            if (pInput != IntPtr.Zero)
            {
                object obj = Marshal.PtrToStructure(pInput, typeof(OPCHDA_ATTRIBUTE));
                result = GetAttributeValueCollection((OPCHDA_ATTRIBUTE)obj, deallocate);
                if (deallocate)
                {
                    Marshal.DestroyStructure(pInput, typeof(OPCHDA_ATTRIBUTE));
                }
            }

            return result;
        }

        internal static AttributeValueCollection GetAttributeValueCollection(OPCHDA_ATTRIBUTE input, bool deallocate)
        {
            AttributeValueCollection attributeValueCollection = new AttributeValueCollection();
            attributeValueCollection.AttributeID = input.dwAttributeID;
            object[] vARIANTs = OpcCom.Interop.GetVARIANTs(ref input.vAttributeValues, input.dwNumValues, deallocate);
            DateTime[] fILETIMEs = OpcCom.Interop.GetFILETIMEs(ref input.ftTimeStamps, input.dwNumValues, deallocate);
            for (int i = 0; i < input.dwNumValues; i++)
            {
                AttributeValue attributeValue = new AttributeValue();
                attributeValue.Value = vARIANTs[i];
                attributeValue.Timestamp = fILETIMEs[i];
                attributeValueCollection.Add(attributeValue);
            }

            return attributeValueCollection;
        }

        internal static AnnotationValueCollection[] GetAnnotationValueCollections(ref IntPtr pInput, int count, bool deallocate)
        {
            AnnotationValueCollection[] array = null;
            if (pInput != IntPtr.Zero && count > 0)
            {
                array = new AnnotationValueCollection[count];
                IntPtr pInput2 = pInput;
                for (int i = 0; i < count; i++)
                {
                    array[i] = GetAnnotationValueCollection(pInput2, deallocate);
                    pInput2 = (IntPtr)(pInput2.ToInt64() + Marshal.SizeOf(typeof(OPCHDA_ANNOTATION)));
                }

                if (deallocate)
                {
                    Marshal.FreeCoTaskMem(pInput);
                    pInput = IntPtr.Zero;
                }
            }

            return array;
        }

        internal static AnnotationValueCollection GetAnnotationValueCollection(IntPtr pInput, bool deallocate)
        {
            AnnotationValueCollection result = null;
            if (pInput != IntPtr.Zero)
            {
                object obj = Marshal.PtrToStructure(pInput, typeof(OPCHDA_ANNOTATION));
                result = GetAnnotationValueCollection((OPCHDA_ANNOTATION)obj, deallocate);
                if (deallocate)
                {
                    Marshal.DestroyStructure(pInput, typeof(OPCHDA_ANNOTATION));
                }
            }

            return result;
        }

        internal static AnnotationValueCollection GetAnnotationValueCollection(OPCHDA_ANNOTATION input, bool deallocate)
        {
            AnnotationValueCollection annotationValueCollection = new AnnotationValueCollection();
            annotationValueCollection.ClientHandle = input.hClient;
            DateTime[] fILETIMEs = OpcCom.Interop.GetFILETIMEs(ref input.ftTimeStamps, input.dwNumValues, deallocate);
            string[] unicodeStrings = OpcCom.Interop.GetUnicodeStrings(ref input.szAnnotation, input.dwNumValues, deallocate);
            DateTime[] fILETIMEs2 = OpcCom.Interop.GetFILETIMEs(ref input.ftAnnotationTime, input.dwNumValues, deallocate);
            string[] unicodeStrings2 = OpcCom.Interop.GetUnicodeStrings(ref input.szUser, input.dwNumValues, deallocate);
            for (int i = 0; i < input.dwNumValues; i++)
            {
                AnnotationValue annotationValue = new AnnotationValue();
                annotationValue.Timestamp = fILETIMEs[i];
                annotationValue.Annotation = unicodeStrings[i];
                annotationValue.CreationTime = fILETIMEs2[i];
                annotationValue.User = unicodeStrings2[i];
                annotationValueCollection.Add(annotationValue);
            }

            return annotationValueCollection;
        }
    }
}
