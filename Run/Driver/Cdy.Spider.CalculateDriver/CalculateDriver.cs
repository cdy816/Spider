using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Linq;

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
            if (CalculateExtend.extend.ExtendDlls.Count > 0)
            {
                sop.AddReferences(CalculateExtend.extend.ExtendDlls.Select(e => Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(e)));
            }
            sop.WithReferences(this.GetType().Assembly).WithImports(" Cdy.Spider.CalculateDriver");


            foreach (var vv in Device.ListTags())
            {
                if (!string.IsNullOrEmpty(vv.DeviceInfo))
                {
                    var vsp = Microsoft.CodeAnalysis.CSharp.Scripting.CSharpScript.Create(vv.DeviceInfo, sop,typeof(CalContext));
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
                    mScriptMaps.Add(new CalItem() { Tag = vv, Script = vsp }.Init());
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ProcessTimerElapsed()
        {
            foreach(var vv in mScriptMaps)
            {
                vv.Execute();
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
    public class CalContext
    {
        public Dictionary<string, Tagbase> TagMaps = new Dictionary<string, Tagbase>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public object GetTagValue(string tag)
        {
            if(TagMaps.ContainsKey(tag))
            {
                return TagMaps[tag];
            }
            return null;
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
        public Tagbase Tag { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsNeedCal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<string> Tags { get; set; }

        public CalContext Context { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public CalItem Init()
        {
            Context = new CalContext();
            var manager = ServiceLocator.Locator.Resolve<IDeviceRuntimeManager>();

            List<string> ll = new List<string>();

            if (string.IsNullOrEmpty(Tag.DeviceInfo))
            {
                var tags = AnalysizeTags(Tag.DeviceInfo);

                foreach (var vv in tags)
                {
                    var dd = GetDeviceAndTagsName(vv);
                    if(!ll.Contains(dd[0]))
                    {
                        var vtag = manager.GetDevice(dd[0]).GetTag(dd[1]);

                        Context.TagMaps.Add(vv, vtag);

                        vtag.ValueChangedCallBack=((sender, val) => {
                            IsNeedCal = true;
                        });
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
            string stag = tag.Substring(tag.LastIndexOf("."));
            string sdevice = tag.Replace("." + stag, "").Replace("Tag.","");
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
        public void Execute()
        {
            if (IsNeedCal)
            {
                try
                {
                    Tag.Value = Script.RunAsync(Context).Result.ReturnValue;
                    byte bqa = 0;
                    foreach (var vv in Context.TagMaps)
                    {
                        if(vv.Value.Quality>1)
                        {
                            bqa = Tagbase.BadCommQuality;
                        }
                    }
                    Tag.Quality = bqa;
                }
                catch (Exception ex)
                {
                    LoggerService.Service.Erro("CalculateDriver", Tag.Name + " : " + ex.Message);
                }
            }
        }
    }

}
