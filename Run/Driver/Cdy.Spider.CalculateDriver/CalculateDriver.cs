using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Linq;
using System.Runtime.Loader;
using System.Reflection;
using Microsoft.CodeAnalysis.Scripting.Hosting;

namespace Cdy.Spider.CalculateDriver
{
    public class CalculateDriver : TimerDriverRunner
    {

        #region ... Variables  ...
        
        /// <summary>
        /// 
        /// </summary>
        private CalculateDriverData mData;

        /// <summary>
        /// 
        /// </summary>
        private List<CalItem> mScriptMaps = new List<CalItem>();

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "CalculateDriver";

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
            ScriptOptions sop = ScriptOptions.Default;
            try
            {
                if (CalculateExtend.extend.ExtendDlls.Count > 0)
                {
                    sop = sop.AddReferences(CalculateExtend.extend.ExtendDlls.Select(e => Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(e)));
                }
                sop = sop.AddReferences(typeof(System.Collections.Generic.ReferenceEqualityComparer).Assembly).AddReferences(this.GetType().Assembly).WithImports("Cdy.Spider.CalculateDriver", "System", "System.Collections.Generic");
            }
            catch(Exception ex)
            {
                LoggerService.Service.Erro("Calculate", ex.Message);
            }

            InteractiveAssemblyLoader ass = new InteractiveAssemblyLoader();

            foreach (var vv in CalculateExtend.extend.ExtendDlls)
            {
                try
                {
                    var assb = new PluginLoadContext(vv).LoadFromAssemblyPath(vv);
                    ass.RegisterDependency(assb);
                }
                catch (Exception eex)
                {
                    LoggerService.Service.Erro("ScriptAlarmTagRun", eex.Message);
                }
            }

            foreach (var vv in Device.ListTags())
            {
                if (!string.IsNullOrEmpty(vv.DeviceInfo))
                {
                    var vsp = Microsoft.CodeAnalysis.CSharp.Scripting.CSharpScript.Create(vv.DeviceInfo, sop, typeof(CalItem), ass);
                    try
                    {
                        var cp = vsp.Compile();
                        if (cp != null && cp.Length > 0)
                        {
                            StringBuilder sb = new StringBuilder();
                            foreach(var vvp in cp)
                            {
                                sb.Append(vvp.ToString());
                            }
                            LoggerService.Service.Warn("Calculate", vv.Name+ " "+ sb.ToString());
                        }

                    }
                    catch (Exception ex)
                    {
                        LoggerService.Service.Erro("Calculate", ex.Message);
                    }
                    mScriptMaps.Add(new CalItem() { TagRef = vv, Script = vsp });
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Prepare()
        {
            foreach(var vv in mScriptMaps)
            {
                vv.Init();
            }
            base.Prepare();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ProcessTimerElapsed()
        {
            foreach(var vv in mScriptMaps)
            {
                vv.Execute(this);
            }
            base.ProcessTimerElapsed();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDriverRuntime NewApi()
        {
            return new CalculateDriver();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void Load(XElement xe)
        {
            mData = new CalculateDriverData();
            mData.LoadFromXML(xe);
            base.Load(xe);
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

    }

    /// <summary>
    /// 
    /// </summary>
    public class TagCallContext
    {
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, Tagbase> TagMaps = new Dictionary<string, Tagbase>();

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, IDeviceRuntime> DeviceMap = new Dictionary<string, IDeviceRuntime>();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public bool IsTagQualityGood(string tag)
        {
            if (TagMaps.ContainsKey(tag))
            {
                return TagMaps[tag].Quality == Tagbase.GoodQuality;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public object GetTagValue(string tag)
        {
            if(TagMaps.ContainsKey(tag))
            {
                //Console.WriteLine("get " + tag + " value:" + TagMaps[tag].Value);
                return TagMaps[tag].Value;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetTagValue(string tag, object value)
        {
            try
            {
                if (TagMaps.ContainsKey(tag))
                {
                    var vtag = TagMaps[tag];
                    //TagMaps[tag].Value = value;
                    if (DeviceMap.ContainsKey(tag))
                        (DeviceMap[tag] as DeviceRunner).UpdateDeviceValue(vtag.Id, value);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public  double TagValueSum(params string[] tags)
        {
            try
            {
                double[] dtmps = new double[tags.Length];
                for (int i = 0; i < tags.Length; i++)
                {
                    dtmps[i] = Convert.ToDouble(GetTagValue(tags[i]));
                }
                return dtmps.Sum();
            }
            catch (Exception ex)
            {
                LoggerService.Service.Erro("Calculate", ex.StackTrace);
            }
            return 0;
        }

        /// <summary>
        /// 对变量的值求平局
        /// </summary>
        /// <param name="tags">变量名</param>
        /// <returns></returns>
        public double TagValueAvg(params string[] tags)
        {
            try
            {
                double[] dtmps = new double[tags.Length];
                for (int i = 0; i < tags.Length; i++)
                {
                    dtmps[i] = Convert.ToDouble(GetTagValue(tags[i]));
                }
                return dtmps.Average();
            }
            catch(Exception ex)
            {
                LoggerService.Service.Erro("Calculate", ex.StackTrace);
            }
            return 0;
        }

        /// <summary>
        /// 对变量的值取最大
        /// </summary>
        /// <param name="tags">变量名</param>
        /// <returns></returns>
        public double TagValueMax(params string[] tags)
        {
            try
            {
                double[] dtmps = new double[tags.Length];
                for (int i = 0; i < tags.Length; i++)
                {
                    dtmps[i] = Convert.ToDouble(GetTagValue(tags[i]));
                }
                return dtmps.Max();
            }
            catch (Exception ex)
            {
                LoggerService.Service.Erro("Calculate", ex.StackTrace);
            }
            return 0;
        }

        /// <summary>
        /// 对变量的值取最小
        /// </summary>
        /// <param name="tags">变量名</param>
        /// <returns></returns>
        public double TagValueMin(params string[] tags)
        {
            try
            {
                double[] dtmps = new double[tags.Length];
                for (int i = 0; i < tags.Length; i++)
                {
                    dtmps[i] = Convert.ToDouble(GetTagValue(tags[i]));
                }
                return dtmps.Min();
            }
            catch (Exception ex)
            {
                LoggerService.Service.Erro("Calculate", ex.StackTrace);
            }
            return 0;
        }

        /// <summary>
        /// 对数值进行请平均值
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public double Avg(params object[] values)
        {
            try
            {
                double[] dtmps = new double[values.Length];
                for (int i = 0; i < values.Length; i++)
                {
                    dtmps[i] = Convert.ToDouble(values[i]);
                }
                return dtmps.Average();
            }
            catch (Exception ex)
            {
                LoggerService.Service.Erro("Calculate", ex.StackTrace);
            }
            return 0;
        }

        /// <summary>
        /// 对数值进行取最大值
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public double Max(params object[] values)
        {
            try
            {
                double[] dtmps = new double[values.Length];
                for (int i = 0; i < values.Length; i++)
                {
                    dtmps[i] = Convert.ToDouble(values[i]);
                }
                return dtmps.Max();
            }
            catch(Exception ex)
            {
                LoggerService.Service.Erro("Calculate", ex.StackTrace);
            }
            return 0;
        }

        /// <summary>
        /// 对数值进行取最小值
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public double Min(params object[] values)
        {
            try
            {
                double[] dtmps = new double[values.Length];
                for (int i = 0; i < values.Length; i++)
                {
                    dtmps[i] = Convert.ToDouble(values[i]);
                }
                return dtmps.Min();
            }
            catch(Exception ex)
            {
                LoggerService.Service.Erro("Calculate", ex.StackTrace);
            }
            return 0;
        }

        /// <summary>
        /// 对值进行取位
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="index">要取位的序号，从0开始</param>
        /// <returns></returns>
        public  byte Bit(object value, byte index)
        {
            var val = Convert.ToInt64(value);
            return (byte)(val >> index & 0x01);
        }

    }




    /// <summary>
    /// 
    /// </summary>
    public class CalItem
    {
        public Script<object> Script { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Tagbase TagRef { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsNeedCal { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TagCallContext Tag { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public CalItem Init()
        {
            Tag = new TagCallContext();
            var manager = ServiceLocator.Locator.Resolve<IDeviceRuntimeManager>();

            List<string> ll = new List<string>();

            if (!string.IsNullOrEmpty(TagRef.DeviceInfo))
            {
                var tags = AnalysizeTags(TagRef.DeviceInfo);

                foreach (var vv in tags)
                {
                    var dd = GetDeviceAndTagsName(vv);
                    if(!ll.Contains(dd[0]))
                    {
                        if (!string.IsNullOrEmpty(dd[0]))
                        {
                            var dev = manager.GetDevice(dd[0]);
                            if (dev != null)
                            {
                                var vtag = dev.GetTag(dd[1]);
                                if (vtag == null)
                                {
                                    LoggerService.Service.Warn(TagRef.Name, $"tag '{vv}' is not exist in expresse '{TagRef.DeviceInfo}'");
                                    continue;
                                }

                                Tag.TagMaps.Add(vv, vtag);
                                Tag.DeviceMap.Add(vv,dev);

                                vtag.ValueChangedCallBack = ((sender, val) =>
                                {
                                    lock (Tag)
                                        IsNeedCal = true;
                                });
                            }
                        }
                    }
                }
            }
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        private string[] GetDeviceAndTagsName(string tag)
        {
            string stag = tag.Substring(tag.LastIndexOf(".")+1);
            string sdevice = tag.Replace("Tag.", "").Replace("." + stag, "");
            return new string[2] { sdevice, stag };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        private List<string> AnalysizeTags(string exp)
        {
             Regex regex = new Regex(@"\bTag((\.\w*)(?!\())*\b",
              RegexOptions.IgnoreCase | RegexOptions.Multiline |
              RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

            List<string> ltmp = new List<string>();

            var vvs = regex.Matches(exp);
            if(vvs.Count>0)
            {
                foreach(var vv in vvs)
                {
                    ltmp.Add(vv.ToString());
                }
            }

            return ltmp;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Execute(CalculateDriver driver)
        {
            if (IsNeedCal||Tag.TagMaps.Count==0)
            {
                lock (Tag)
                    IsNeedCal = false;
                try
                {
                    var val = Script.RunAsync(this).Result.ReturnValue;
                    //Console.WriteLine(" 变量执行结果" + TagRef.DeviceInfo +" --> "+  TagRef.Name + ":" + TagRef.Value.ToString() + "   " + DateTime.Now.ToString());
                    byte bqa = Tagbase.GoodQuality;
                    foreach (var vv in Tag.TagMaps)
                    {
                        if(vv.Value.Quality>1)
                        {
                            bqa = Tagbase.BadCommQuality;
                        }
                    }
                    driver.Device.UpdateDeviceValue(TagRef.Id, val,bqa);
                    // TagRef.Quality = bqa;
                }
                catch (Exception ex)
                {
                    LoggerService.Service.Erro("CalculateDriver", TagRef.Name + " : " + ex.Message);
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PluginLoadContext : AssemblyLoadContext
    {
        private AssemblyDependencyResolver _resolver;

        public PluginLoadContext(string pluginPath)
        {
            _resolver = new AssemblyDependencyResolver(pluginPath);
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            string assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null)
            {
                return LoadFromAssemblyPath(assemblyPath);
            }

            return null;
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            string libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath != null)
            {
                return LoadUnmanagedDllFromPath(libraryPath);
            }

            return IntPtr.Zero;
        }
    }

}
