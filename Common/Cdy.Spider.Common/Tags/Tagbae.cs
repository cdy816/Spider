//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/4 15:06:26.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Cdy.Spider
{
    /// <summary>
    /// 变量类型
    /// </summary>
    public enum TagType
    {
        /// <summary>
        /// 
        /// </summary>
        Bool,
        /// <summary>
        /// 
        /// </summary>
        Byte,
        /// <summary>
        /// 
        /// </summary>
        Short,
        /// <summary>
        /// 
        /// </summary>
        UShort,
        /// <summary>
        /// 
        /// </summary>
        Int,
        /// <summary>
        /// 
        /// </summary>
        UInt,
        /// <summary>
        /// 
        /// </summary>
        Long,
        /// <summary>
        /// 
        /// </summary>
        ULong,
        /// <summary>
        /// 
        /// </summary>
        Double,
        /// <summary>
        /// 
        /// </summary>
        Float,
        /// <summary>
        /// 
        /// </summary>
        DateTime,
        /// <summary>
        /// 
        /// </summary>
        String,
        /// <summary>
        /// 
        /// </summary>
        IntPoint,
        /// <summary>
        /// 
        /// </summary>
        UIntPoint,
        /// <summary>
        /// 
        /// </summary>
        LongPoint,
        /// <summary>
        /// 
        /// </summary>
        ULongPoint,
        /// <summary>
        /// 
        /// </summary>
        IntPoint3,
        /// <summary>
        /// 
        /// </summary>
        UIntPoint3,
        /// <summary>
        /// 
        /// </summary>
        LongPoint3,
        /// <summary>
        /// 
        /// </summary>
        ULongPoint3
    }

    public abstract class Tagbae
    {

        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 数据库变量名称
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// 设备信息
        /// </summary>
        public string DeviceInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public abstract TagType Type { get; }

        /// <summary>
        /// 值
        /// </summary>
        public virtual object Value { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

    public static class TagExtends
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static XElement SaveToXML(this Tagbae tag)
        {
            XElement xx = new XElement("Tag");
            xx.SetAttributeValue("Type", (int)tag.Type);
            xx.SetAttributeValue("Id", tag.Id);
            xx.SetAttributeValue("Name", tag.Name);
            xx.SetAttributeValue("DatabaseName", tag.DatabaseName);
            xx.SetAttributeValue("DeviceInfo", tag.DeviceInfo);
            return xx;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        /// <returns></returns>
        public static Tagbae CreatFromXML(this XElement xe)
        {
            var type = (TagType)int.Parse(xe.Attribute("Type").Value);
            var tag = type.CreatTag();
            if (tag != null)
            {
                tag.Id = int.Parse(xe.Attribute("Id").Value);
                tag.Name = xe.Attribute("Name").Value;
                if (xe.Attribute("DatabaseName") != null)
                {
                    tag.DatabaseName = xe.Attribute("DatabaseName").Value;
                }
                if (xe.Attribute("DeviceInfo") != null)
                {
                    tag.DeviceInfo = xe.Attribute("DeviceInfo").Value;
                }
            }
            return tag;
        }

        public static Tagbae CreatTag(this TagType type)
        {
            switch (type)
            {
                case TagType.Bool:
                    return new BoolTag();
                case TagType.Byte:
                    return new ByteTag();
                case TagType.DateTime:
                    return new DateTimeTag();
                case TagType.Double:
                    return new DoubleTag();
                case TagType.Float:
                    return new FloatTag();
                case TagType.Int:
                    return new IntTag();
                case TagType.IntPoint:
                    return new IntPointTag();
                case TagType.IntPoint3:
                    return new IntPoint3Tag();
                case TagType.Long:
                    return new LongTag();
                case TagType.LongPoint:
                    return new LongPointTag();
                case TagType.ULong:
                    return new ULongTag();
                case TagType.ULongPoint:
                    return new ULongPointTag();
                case TagType.ULongPoint3:
                    return new ULongPoint3Tag();
                case TagType.UShort:
                    return new UShortTag();
                case TagType.String:
                    return new StringTag();
                case TagType.Short:
                    return new ShortTag();
                case TagType.UInt:
                    return new UIntTag();
                case TagType.UIntPoint:
                    return new UIntPointTag();
                case TagType.UIntPoint3:
                    return new UIntPoint3Tag();

            }

            return null;
        }

    }

}
