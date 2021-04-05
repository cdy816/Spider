using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Cdy.Spider
{
    /// <summary>
    /// 
    /// </summary>
    public class ModbusIpDriverData: DriverData
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
        /// Long 规则
        /// </summary>
        public EightValueFormate LongFormate { get; set; }

        /// <summary>
        /// Double 规则
        /// </summary>
        public EightValueFormate DoubleFormate { get; set; }

        /// <summary>
        /// Int 规则
        /// </summary>
        public FourValueFormate IntFormate { get; set; }

        /// <summary>
        /// Float 规则
        /// </summary>
        public FourValueFormate FloatFormate { get; set; }

        /// <summary>
        /// 寄存器高低位规则
        /// </summary>
        public ShortValueFormate ShortFormate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public StringEncoding StringEncoding { get; set; }


        /// <summary>
        /// 打包长度
        /// </summary>
        public ushort PackageLen { get; set; } = 60;

        #endregion ...Properties...

        #region ... Methods    ...

        public override void LoadFromXML(XElement xe)
        {
            base.LoadFromXML(xe);

            if(xe.Attribute("Id")!=null)
            {
                this.Id = int.Parse(xe.Attribute("Id").Value);
            }

            if (xe.Attribute("PackageLen") != null)
            {
                this.PackageLen = ushort.Parse(xe.Attribute("PackageLen").Value);
            }

            if (xe.Attribute("LongFormate") !=null)
            {
                this.LongFormate = (EightValueFormate)(int.Parse(xe.Attribute("LongFormate").Value));
            }

            if (xe.Attribute("DoubleFormate") != null)
            {
                this.DoubleFormate = (EightValueFormate)(int.Parse(xe.Attribute("DoubleFormate").Value));
            }

            if (xe.Attribute("IntFormate") != null)
            {
                this.IntFormate = (FourValueFormate)(int.Parse(xe.Attribute("IntFormate").Value));
            }

            if (xe.Attribute("FloatFormate") != null)
            {
                this.FloatFormate = (FourValueFormate)(int.Parse(xe.Attribute("FloatFormate").Value));
            }

            if (xe.Attribute("ShortFormate") != null)
            {
                this.ShortFormate = (ShortValueFormate)(int.Parse(xe.Attribute("ShortFormate").Value));
            }

            if (xe.Attribute("StringEncoding") != null)
            {
                this.StringEncoding = (StringEncoding)(int.Parse(xe.Attribute("StringEncoding").Value));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override XElement SaveToXML()
        {
            var re = base.SaveToXML();
            re.SetAttributeValue("Id", Id);
            re.SetAttributeValue("PackageLen", PackageLen);
            re.SetAttributeValue("LongFormate", (int)this.LongFormate);
            re.SetAttributeValue("DoubleFormate", (int)this.DoubleFormate);
            re.SetAttributeValue("IntFormate", (int)this.IntFormate);
            re.SetAttributeValue("FloatFormate", (int)this.FloatFormate);
            re.SetAttributeValue("ShortFormate", (int)this.ShortFormate);
            re.SetAttributeValue("StringEncoding", (int)this.StringEncoding);
            return re;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

    public enum StringEncoding
    {
        Ascii,
        Utf8,
        Unicode
    }


    /// <summary>
    /// 8字节排序规则
    /// </summary>
    public enum EightValueFormate
    {
        D4321,
        D1234,
        D3412,
        D2143
    }

    /// <summary>
    /// 4字节排序规则
    /// </summary>
    public enum FourValueFormate
    {
        D21,
        D12,
    }

    /// <summary>
    /// 2字节数据高低位排列规则
    /// </summary>
    public enum ShortValueFormate
    {
        D12,
        D21
    }


}
