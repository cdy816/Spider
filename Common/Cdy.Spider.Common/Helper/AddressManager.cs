using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Cdy.Spider.Common.Helper
{
    /// <summary>
    /// 
    /// </summary>
    public class AddressManager
    {

        #region ... Variables  ...

        private Dictionary<Type, List<AddressBlock>> mBlocks = new Dictionary<Type, List< Helper.AddressBlock>>();

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        #endregion ...Properties...

        #region ... Methods    ...


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AddressBlock> ListAddressBlock()
        {
           foreach (var block in mBlocks.Values)
            {
                foreach(var address in block)
                {
                    yield return address;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="address"></param>
        /// <param name="isbitbool"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        private AddressBlock GetBlock(Type tp,double address,bool isbitbool=false,int len=-1)
        {
            AddressBlock ab=null;
            if(mBlocks.ContainsKey(tp))
            {
                foreach(var vv in mBlocks[tp])
                {
                    if (len > 0)
                    {
                        if (vv.CanAdd(address)&&vv.PerAddressLength == len)
                        {
                            ab = vv;
                        }
                    }
                    else
                    {
                        if (vv.CanAdd(address))
                        {
                            ab = vv;
                        }
                    }
                }
            }

            if(ab == null)
            {
                if (isbitbool)
                {
                    ab = new AddressBlock(tp, isbitbool);
                }
                else if (len > 0)
                {
                    ab = new AddressBlock(tp, len);
                }
                else
                {
                    ab = new AddressBlock(tp);
                }

                if (!mBlocks.ContainsKey(tp))
                {
                    mBlocks.Add(tp, new List<AddressBlock>() { ab });
                }
                else
                {
                    mBlocks[tp].Add(ab);
                }
            }

            

            return ab;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saddress"></param>
        /// <param name="address"></param>
        /// <param name="type"></param>
        public void Add<T>(string saddress,double address)
        {
            var ab = GetBlock(typeof(T),address);
            ab.AddSubAdress(address,saddress);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saddress"></param>
        /// <param name="address"></param>
        /// <param name="type"></param>
        public void Add<T>(string saddress, double address,int len)
        {
            var ab = GetBlock(typeof(T), address,false,len);
            ab.AddSubAdress(address, saddress);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tp"></param>
        /// <param name="address"></param>
        /// <param name="saddress"></param>
        public void Add(TagType tp,double address,string saddress,bool isBitbool=true,int stringlen=-1)
        {
            switch (tp)
            {
                case TagType.Bool:
                    AddBool(saddress, address, true);
                    break;
                case TagType.Byte:
                    Add<byte>(saddress, address);
                    break;
                case TagType.Short:
                    Add<short>(saddress, address);
                    break;
                case TagType.UShort:
                    Add<ushort>(saddress, address);
                    break;
                case TagType.Int:
                    Add<int>(saddress, address);
                    break;
                case TagType.UInt:
                    Add<uint>(saddress, address);
                    break;
                case TagType.Long:
                    Add<long>(saddress, address);
                    break;
                case TagType.ULong:
                    Add<ulong>(saddress, address);
                    break;
                case TagType.Double:
                    Add<double>(saddress, address);
                    break;
                case TagType.Float:
                    Add<float>(saddress, address);
                    break;
                case TagType.DateTime:
                    Add<DateTime>(saddress, address);
                    break;
                case TagType.String:
                    AddString(saddress, address, stringlen);
                    break;
                case TagType.IntPoint:
                    Add<IntPoint>(saddress, address);
                    break;
                case TagType.UIntPoint:
                    Add<UIntPoint>(saddress, address);
                    break;
                case TagType.IntPoint3:
                    Add<IntPoint3>(saddress, address);
                    break;
                case TagType.UIntPoint3:
                    Add<UIntPoint3>(saddress, address);
                    break;
                case TagType.LongPoint:
                    Add<LongPoint>(saddress, address);
                    break;
                case TagType.ULongPoint:
                    Add<ULongPoint>(saddress, address);
                    break;
                case TagType.LongPoint3:
                    Add<LongPoint3>(saddress, address);
                    break;
                case TagType.ULongPoint3:
                    Add<ULongPoint3>(saddress, address);
                    break;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saddress"></param>
        /// <param name="address"></param>
        /// <param name="mIsBitBool"></param>
        public void AddBool(string saddress,double address,bool mIsBitBool)
        {
            var ab = GetBlock(typeof(bool), address, mIsBitBool);
            ab.AddSubAdress(address,saddress);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="saddress"></param>
        /// <param name="address"></param>
        public void AddString(string saddress,double address,int len)
        {
            var ab = GetBlock(typeof(bool), address, false,len);
            ab.AddSubAdress(address, saddress);
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
