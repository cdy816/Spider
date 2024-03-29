﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace Cdy.Spider.CustomDriver
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICustomDriverImp
    {
        /// <summary>
        /// 
        /// </summary>
        void Init();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        object OnReceiveData(string key, object data);
        
        /// <summary>
        /// 
        /// </summary>
        void ProcessTimerElapsed();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <param name="value"></param>
        /// <param name="valueType"></param>
        void WriteValue(string deviceInfo, object value, byte valueType);
    }

    /// <summary>
    /// 
    /// </summary>
    public class CustomDriver : TimerDriverRunner
    {

        #region ... Variables  ...
        private CustomDriverData mData;

        private ICustomDriverImp mImp;
        /// <summary>
        /// 
        /// </summary>
        public const string mTemplate =
@"using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.Buffers;
using System.Threading;
using Cdy.Spider.CustomDriver;

namespace Cdy.Spider
{
/// <summary>
/// 
/// </summary>
public class $ClassName$Driver:Cdy.Spider.CustomDriver.CustomImp
{
$VariableBody$	                                   	
		
public override object OnReceiveData(string key, object data)
{
$OnReceiveDataBody$
}

public override void ProcessTimerElapsed()
{
$ProcessTimerElapsed$
}

public override void WriteValue(string deviceInfo, object value, byte valueType)
{
$WriteValue$
}
}
}";


        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "CustomDriver";


        /// <summary>
        /// 
        /// </summary>
        public override DriverData Data => mData;

        

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        public override void Init()
        {
            base.Init();
            Assembly assembly=null;
            StringBuilder sb = new StringBuilder(mTemplate);
            string scname = "C" + Guid.NewGuid().ToString().Replace("-", "");
            sb.Replace("$ClassName$", scname);
            sb.Replace("$VariableBody$", mData.VariableExpress);
            sb.Replace("$OnReceiveDataBody$",string.IsNullOrEmpty(mData.OnReceiveDataFunExpress)?"return null;":mData.OnReceiveDataFunExpress);
            sb.Replace("$WriteValue$", mData.OnSetTagValueToDeviceFunExpress);
            sb.Replace("$ProcessTimerElapsed$", mData.OnTimerFunExpress);
            
            List<string> ll = new List<string>();
            //

            // 元数据引用
            var references = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(System.Object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Collections.SortedList).Assembly.Location),
                MetadataReference.CreateFromFile(ll.GetType().Assembly.Location),
                MetadataReference.CreateFromFile(typeof(DriverRunnerBase).Assembly.Location),
                MetadataReference.CreateFromFile(this.GetType().Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Xml.Linq.XElement).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.MemoryExtensions).Assembly.Location)
            };
            Assembly.GetEntryAssembly().GetReferencedAssemblies().ToList().ForEach(e => references.Add(MetadataReference.CreateFromFile(Assembly.Load(e).Location)));

            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(sb.ToString());

            var compileOption = new CSharpCompilationOptions(Microsoft.CodeAnalysis.OutputKind.DynamicallyLinkedLibrary,allowUnsafe:true);
            var cc = CSharpCompilation.Create("test", new SyntaxTree[] { syntaxTree }, references, compileOption);


            using (var ms = new System.IO.MemoryStream())
            {
                var result = cc.Emit(ms);
                if (result.Success)
                {
                    try
                    {
                        // 编译成功则从内存中加载程序集
                        ms.Seek(0, SeekOrigin.Begin);
                        assembly = Assembly.Load(ms.ToArray());
                    }
                    catch
                    {

                    }
                }
                else
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                    diagnostic.IsWarningAsError ||
                    diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (Diagnostic diagnostic in failures)
                    {
                        Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                    }
                }
            }
           
            if(assembly!=null)
            {
                try
                {
                    mImp = assembly.CreateInstance("Cdy.Spider." + scname + "Driver") as ICustomDriverImp;
                    (mImp as CustomImp).Owner = this;
                    (mImp as CustomImp).Comm = this.mComm;
                    if (mImp != null)
                    {
                        mImp.Init();
                    }
                }
                catch(Exception ex)
                {
                    LoggerService.Service.Erro("CustomDriver", ex.Message);
                }
            }
        }



        /// <summary>
        /// 被动接受数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="handled"></param>
        /// <returns></returns>
        protected override object OnReceiveData(string key, object data, out bool handled)
        {
            if (mImp != null)
            {
                handled = true;
               return mImp.OnReceiveData(key,data);
            }
            return base.OnReceiveData(key, data, out handled);
        }


        /// <summary>
        /// 定时执行
        /// </summary>
        protected override void ProcessTimerElapsed()
        {
            if(this.mImp != null)
            {
                mImp.ProcessTimerElapsed();
            }
            base.ProcessTimerElapsed();
        }

        /// <summary>
        /// 写入值到设备上
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <param name="value"></param>
        /// <param name="valueType"></param>
        public override void WriteValue(string deviceInfo, object value, byte valueType)
        {
           if(this.mImp!=null)
            {
                this.mImp.WriteValue(deviceInfo, value, valueType);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDriverRuntime NewApi()
        {
            return new CustomDriver();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void Load(XElement xe)
        {
            mData = new CustomDriverData();
            mData.LoadFromXML(xe);
            base.Load(xe);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <param name="value"></param>
        public void UpdateTagValue(string deviceInfo, object value)
        {
            this.UpdateValue(deviceInfo, value);
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...


    }

    /// <summary>
    /// 
    /// </summary>
    public class CustomImp : ICustomDriverImp
    {

        /// <summary>
        /// 
        /// </summary>
        internal CustomDriver Owner { get; set; }

        /// <summary>
        /// 通信对象
        /// </summary>
        public ICommChannel2 Comm { get; set; }

        /// <summary>
        /// 获取变量的值
        /// </summary>
        /// <param name="tagfullname">变量的全名:Tag.device1.tag1</param>
        /// <returns></returns>
        protected object GetTagValue(string tagfullname)
        {
            var manager = ServiceLocator.Locator.Resolve<IDeviceRuntimeManager>();
            var tags = GetDeviceAndTagsName(tagfullname);
            if (!string.IsNullOrEmpty(tags[0]))
            {
                var dev = manager.GetDevice(tags[0]);
                if (dev != null)
                {
                    var vtag = dev.GetTag(tags[1]);
                    if(vtag != null)
                    return vtag.Value;
                }
            }
            return null;
        }

        /// <summary>
        /// 设置变量的值
        /// </summary>
        /// <param name="tagfullname">变量的全名:Tag.device1.tag1</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        protected void SetTagValue(string tagfullname,object value)
        {
            var manager = ServiceLocator.Locator.Resolve<IDeviceRuntimeManager>();
            var tags = GetDeviceAndTagsName(tagfullname);
            if (!string.IsNullOrEmpty(tags[0]))
            {
                var dev = manager.GetDevice(tags[0]);
                if (dev != null)
                {
                    var vtag = dev.GetTag(tags[1]);
                    vtag.Value=value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        private string[] GetDeviceAndTagsName(string tag)
        {
            string stag = tag.Substring(tag.LastIndexOf(".") + 1);
            string sdevice = tag.Replace("Tag.", "").Replace("." + stag, "");
            return new string[2] { sdevice, stag };
        }

        /// <summary>
        /// 通过寄存器名称写入值到一个或多个变量
        /// </summary>
        /// <param name="deviceInfo">寄存器名称</param>
        /// <param name="value">值</param>
        protected void UpdateValue(string deviceInfo, object value)
        {
            byte[] bals=null;
           var  vv=  bals.AsSpan<byte>();
            Owner?.UpdateTagValue(deviceInfo, value);
        }

        /// <summary>
        /// Json 字符串转换成对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected T JsonStringToObject<T>(string value)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        /// Json 字符串转换成对象
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected object JsonStringToObject(string value,Type type)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject(value,type);
        }

        /// <summary>
        /// Json 字符串转换成字典对象
        /// Json 属性的值为Json 对象时,字典的的值同样为字典对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected Dictionary<string,object> JsonStringToDictionaryObject(string value)
        {
            Dictionary<string, object> re = new Dictionary<string, object>();
            var objs = Newtonsoft.Json.Linq.JObject.Parse(value);
            foreach(var vv in objs)
            {
                re.Add(vv.Key, JTokenToObject(vv.Value));
            }
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private object JTokenToObject(Newtonsoft.Json.Linq.JToken value)
        {
            if(value is JProperty)
            {
                var jval = ((value as JProperty).Value as JValue);
                if(jval.Value is byte[] || jval.Value==null)
                return jval.Value;
                else
                {
                    return jval.Value.ToString();
                }
            }
            else if(value is JObject)
            {
                Dictionary<string, object> re = new Dictionary<string, object>();
                foreach (var vv in (value as JObject))
                {
                    re.Add(vv.Key, JTokenToObject(vv.Value));
                }
                return re;
            }
            return null;
        }

        /// <summary>
        /// 如果 value 是 byte 数组 或者 List  以指定编码转换成字符串
        /// 其他类型直接调用ToString
        /// </summary>
        /// <param name="values">byte[]\list\Object</param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public string BytesToStirng(object values, Encoding encoding)
        {
            if (values is byte[])
            {
                return encoding.GetString(values as byte[]);
            }
            else if(values is List<byte>)
            {
                return encoding.GetString((values as List<byte>).ToArray());
            }
            else
            {
                return values.ToString();
            }
        }

        /// <summary>
        /// Bytes 数组转换成字符串
        /// </summary>
        /// <param name="values"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public string BytesToStirng(byte[] values,Encoding encoding)
        {
            return encoding.GetString(values);
        }

        /// <summary>
        /// Bytes 数组转换成字符串
        /// </summary>
        /// <param name="values"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public string BytesToStirng(byte[] values,int start,int len, Encoding encoding)
        {
            return encoding.GetString(values,start,len);
        }

        /// <summary>
        /// 打印日志 Info
        /// </summary>
        /// <param name="msg"></param>
        protected void Info(string msg)
        {
            LoggerService.Service.Info("CustomDriver", msg);
        }

        /// <summary>
        /// 打印日志 Erro
        /// </summary>
        /// <param name="msg"></param>
        protected void Erro(string msg)
        {
            LoggerService.Service.Erro("CustomDriver", msg);
        }

        /// <summary>
        /// 通过字符串请求数据
        /// </summary>
        /// <param name="value">请求字符串</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        protected string QueryDataByString(string value,Encoding encoding)
        {
            var re = Comm.SendAndWait(encoding.GetBytes(value));
            if(re!=null)
            {
                return encoding.GetString(re);
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 通过字节数组请求数据
        /// </summary>
        /// <param name="value">请求字节数组</param>
        /// <returns></returns>
        protected byte[] QueryDataByBytes(byte[] value)
        {
            return Comm.SendAndWait(value);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Init()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public virtual object OnReceiveData(string key, object data)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void ProcessTimerElapsed()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <param name="value"></param>
        /// <param name="valueType"></param>
        public virtual void WriteValue(string deviceInfo, object value, byte valueType)
        {

        }

        /// <summary>
        /// 获取该设备定义的变量名称列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> ListTags()
        {
            return Owner.Device.ListTags().Select(e=>e.DeviceInfo).ToList();
        }

        /// <summary>
        /// 写入数据到设备并等待
        /// </summary>
        /// <param name="value">Byte 值</param>
        public byte[] WriteToDeviceSync(byte[] value)
        {
            return Owner.SendAndWait(value);
        }

        /// <summary>
        /// 写入数据到设备无需等待
        /// </summary>
        /// <param name="value">Byte 值</param>
        /// <returns></returns>
        public bool WriteToDeviceAsync(byte[] value)
        {
            return Owner.Send(value);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public static class ImpExtends
    {
        /// <summary>
        /// Json 字符串转换成字典对象
        /// Json 属性的值为Json 对象时,字典的的值同样为字典对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Dictionary<string, object> JsonStringToDictionaryObject(this string value)
        {
            Dictionary<string, object> re = new Dictionary<string, object>();
            var objs = Newtonsoft.Json.Linq.JObject.Parse(value);
            foreach (var vv in objs)
            {
                re.Add(vv.Key, JTokenToObject(vv.Value));
            }
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static object JTokenToObject(Newtonsoft.Json.Linq.JToken value)
        {
            if (value is JProperty)
            {
                var jval = ((value as JProperty).Value as JValue);
                if (jval.Value is byte[] || jval.Value == null)
                    return jval.Value;
                else
                {
                    return jval.Value.ToString();
                }
            }
            else if (value is JObject)
            {
                Dictionary<string, object> re = new Dictionary<string, object>();
                foreach (var vv in (value as JObject))
                {
                    re.Add(vv.Key, JTokenToObject(vv.Value));
                }
                return re;
            }
            return null;
        }


        /// <summary>
        /// Json 字符串转换成对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T JsonStringToObject<T>(this string value)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        /// 将对象转化成Json 字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToJsonString(this object value)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(value);
        }

        /// <summary>
        /// 将Byte 数组转化成字符串
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string ToString(this byte[] value,Encoding encoding)
        {
            return encoding.GetString(value);
        }


        /// <summary>
        /// 将Byte 数组转化成Hex字符串
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static string ToHexString(this byte[] value)
        {
            return Convert.ToHexString(value);
        }


        /// <summary>
        /// 将Byte 数组转化成Base64字符串
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static string ToBase64String(this byte[] value)
        {
            return Convert.ToBase64String(value);
        }

        /// <summary>
        /// 转换成字节数组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToBytes(double value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// 转换成字节数组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToBytes(float value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// 转换成字节数组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToBytes(short value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// 转换成字节数组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToBytes(ushort value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// 转换成字节数组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToBytes(int value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// 转换成字节数组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToBytes(uint value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// 转换成字节数组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToBytes(long value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// 转换成字节数组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToBytes(ulong value)
        {
            return BitConverter.GetBytes(value);
        }


        /// <summary>
        /// 转换成字节数组
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToBytes(bool value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// 转换成Double
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDouble(this object value)
        {
            return Convert.ToDouble(value);
        }

        /// <summary>
        /// 转换成Float
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float ToFloat(this byte[] value)
        {
            return BitConverter.ToSingle(value);
        }


        /// <summary>
        /// 转换成Float
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startindex"></param>
        /// <returns></returns>
        public static float ToFloat(this byte[] value,int startindex)
        {
            return BitConverter.ToSingle(value,startindex);
        }


        /// <summary>
        /// 转换成Double
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDouble(this byte[] value)
        {
            return BitConverter.ToDouble(value);
        }


        /// <summary>
        /// 转换成Double
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startindex"></param>
        /// <returns></returns>
        public static double ToDouble(this byte[] value, int startindex)
        {
            return BitConverter.ToDouble(value, startindex);
        }


        /// <summary>
        /// 转换成int
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(this byte[] value)
        {
            return BitConverter.ToInt32(value);
        }


        /// <summary>
        /// 转换成int
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startindex"></param>
        /// <returns></returns>
        public static int ToInt(this byte[] value, int startindex)
        {
            return BitConverter.ToInt32(value, startindex);
        }

        /// <summary>
        /// 转换成long
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ToLong(this byte[] value)
        {
            return BitConverter.ToInt64(value);
        }


        /// <summary>
        /// 转换成long
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startindex"></param>
        /// <returns></returns>
        public static long ToLong(this byte[] value, int startindex)
        {
            return BitConverter.ToInt64(value, startindex);
        }


        /// <summary>
        /// 转换成Short
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static short ToShort(this byte[] value)
        {
            return BitConverter.ToInt16(value);
        }


        /// <summary>
        /// 转换成Short
        /// </summary>
        /// <param name="value"></param>
        /// <param name="startindex"></param>
        /// <returns></returns>
        public static short ToShort(this byte[] value, int startindex)
        {
            return BitConverter.ToInt16(value, startindex);
        }

        /// <summary>
        /// 转换成Float
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float ToFloat(this object value)
        {
            return Convert.ToSingle(value);
        }

        /// <summary>
        /// 串转换成Int
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(this object value)
        {
            return Convert.ToInt32(value);
        }

        /// <summary>
        /// 转换成Long
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ToLong(this object value)
        {
            return Convert.ToInt64(value);
        }

        /// <summary>
        /// 转换成Datetime
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object value)
        {
            return Convert.ToDateTime(value);
        }

        /// <summary>
        /// 转换成byte
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte ToByte(this object value)
        {
            return Convert.ToByte(value);
        }

        /// <summary>
        /// 转换成byte
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static short ToShort(this object value)
        {
            return Convert.ToInt16(value);
        }
    }

}
