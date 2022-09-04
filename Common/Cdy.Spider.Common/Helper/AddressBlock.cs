using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace Cdy.Spider.Common.Helper
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AddressBlock
    {

        private bool mIsBitBool = false;

        /// <summary>
        /// 
        /// </summary>
        public AddressBlock(Type type)
        {
            T=type;
            Init();
            CalTagType();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isBitBool"></param>
        public AddressBlock(Type type,bool isBitBool)
        {
            mIsBitBool = isBitBool;
            T = type;
            Init();
            CalTagType();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isBitBool"></param>
        public AddressBlock(Type type, int mLen)
        {
            T= type;
            Init();
            PerAddressLength = mLen;
            CalTagType();
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsBitBool
        {
            get
            {
                return mIsBitBool;
            }
        }

        public Type T { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<string> SubAddress { get;set; }=new List<string>();

        /// <summary>
        /// 开始地址
        /// </summary>
        public double StartNumberAddress { get; set; }

        /// <summary>
        /// 结束地址
        /// </summary>
        public double EndNumberAddress { get;private set; }

        /// <summary>
        /// 单个地址所占大小
        /// </summary>
        public double PerAddressLength { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public TagType TagType
        {
            get;
            set;
        }


        /// <summary>
        /// 
        /// </summary>
        private void Init(bool isBit=false)
        {
            TypeName = T.Name.ToLower();
            switch(TypeName)
            {
                case "bool":
                    if (mIsBitBool)
                    {
                        PerAddressLength = 0.1f;
                    }
                    else
                    {
                        PerAddressLength = 1;
                    }
                    break;
                case "byte":
                    PerAddressLength = 1;
                    break;
                case "short":
                    PerAddressLength = 2;
                    break;
                case "ushort":
                    PerAddressLength = 2;
                    break;
                case "int":
                    PerAddressLength = 4;
                    break;
                case "uint":
                    PerAddressLength = 4;
                    break;
                case "long":
                    PerAddressLength = 8;
                    break;
                case "ulong":
                    PerAddressLength = 8;
                    break;
                case "datetime":
                    PerAddressLength = 8;
                    break ;
                case "string":
                    break;
                case "intpoint":
                    PerAddressLength = 8;
                    break;
                case "uintpoint":
                    PerAddressLength = 8;
                    break;
                case "intpoint3":
                    PerAddressLength = 12;
                    break;
                case "uintpoint3":
                    PerAddressLength = 12;
                    break;
                case "longpoint":
                    PerAddressLength = 16;
                    break;
                case "ulongpoint":
                    PerAddressLength = 16;
                    break;
                case "longpoint3":
                    PerAddressLength = 24;
                    break;
                case "ulongpoint3":
                    PerAddressLength = 24;
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void CalTagType()
        {
            TypeName = T.Name.ToLower();
            switch (TypeName)
            {
                case "bool":
                    TagType = TagType.Bool;
                    break;
                case "byte":
                    TagType = TagType.Byte;
                    break;
                case "short":
                    TagType = TagType.Short;
                    break;
                case "ushort":
                    TagType = TagType.UShort;
                    break;
                case "int":
                    if (PerAddressLength == 4)
                    {
                        TagType = TagType.Int;
                    }
                    else if (PerAddressLength == 8)
                    {
                        TagType = TagType.IntPoint;
                    }
                    else
                    {
                        TagType = TagType.IntPoint3;
                    }
                    break;
                case "uint":
                    if (PerAddressLength == 4)
                    {
                        TagType = TagType.UInt;
                    }
                    else if (PerAddressLength == 8)
                    {
                        TagType = TagType.UIntPoint;
                    }
                    else
                    {
                        TagType = TagType.UIntPoint3;
                    }
                    break;
                case "long":
                    if (PerAddressLength == 8)
                    {
                        TagType = TagType.Long;
                    }
                    else if (PerAddressLength == 16)
                    {
                        TagType = TagType.LongPoint;
                    }
                    else
                    {
                        TagType = TagType.LongPoint3;
                    }
                    break;
                case "ulong":
                    if (PerAddressLength == 8)
                    {
                        TagType = TagType.ULong;
                    }
                    else if (PerAddressLength == 16)
                    {
                        TagType = TagType.ULongPoint;
                    }
                    else
                    {
                        TagType = TagType.ULongPoint3;
                    }
                    break;
                case "datetime":
                    TagType = TagType.DateTime;
                    break;
                case "string":
                    TagType = TagType.String;
                    break;
                case "intpoint":
                    TagType = TagType.IntPoint;
                    break;
                case "uintpoint":
                    TagType = TagType.UIntPoint;
                    break;
                case "intpoint3":
                    TagType = TagType.IntPoint3;
                    break;
                case "uintpoint3":
                    TagType = TagType.UIntPoint3;
                    break;
                case "longpoint":
                    TagType = TagType.LongPoint;
                    break;
                case "ulongpoint":
                    TagType = TagType.ULongPoint;
                    break;
                case "longpoint3":
                    TagType = TagType.LongPoint3;
                    break;
                case "ulongpoint3":
                    TagType = TagType.ULongPoint3;
                    break;
            }
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dval"></param>
        /// <returns></returns>
        private double CalAddress(double dval)
        {
            double dtmp = dval - Math.Ceiling(dval);
            if(dtmp>0.7)
            {
                return Math.Ceiling(dval) + 1;
            }
            else
            {
                return dval;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public bool CanAdd(double address)
        {
            if(address<= CalAddress(PerAddressLength + EndNumberAddress))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        public void AddSubAdress(double numberAddress, string address)
        {
            if(SubAddress.Count==0)
            {
                StartNumberAddress = numberAddress;
                Address=string.IsNullOrEmpty(address)?numberAddress.ToString():address;
            }
            SubAddress.Add(address);
            EndNumberAddress = CalAddress(EndNumberAddress + PerAddressLength);
        }
    }
}
