using Cdy.Spider;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderRuntime
{
    /// <summary>
    /// 
    /// </summary>
    public class ScriptService : Cdy.Spider.IScriptService
    {
        /// <summary>
        /// 
        /// </summary>
        public static ScriptService Service = new ScriptService();

        private Dictionary<string, Script<object>> mCachedScripts = new Dictionary<string, Script<object>>();
        private ScriptOptions sop;
        /// <summary>
        /// 
        /// </summary>
        public ScriptService()
        {
            Init();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Init()
        {
            sop = ScriptOptions.Default;
            try
            {
                if (CalculateExtend.extend.ExtendDlls.Count > 0)
                {
                    sop = sop.AddReferences(CalculateExtend.extend.ExtendDlls.Select(e => Microsoft.CodeAnalysis.MetadataReference.CreateFromFile(e)));
                }
                sop = sop.AddReferences(typeof(System.Collections.Generic.ReferenceEqualityComparer).Assembly).AddReferences(this.GetType().Assembly).AddReferences(typeof(Tagbase).Assembly).WithImports("Cdy.Spider", "System", "System.Collections.Generic");
            }
            catch (Exception ex)
            {
                LoggerService.Service.Erro("ScriptService", ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string Compile(string exp, object context)
        {
            string sid = Guid.NewGuid().ToString();
            if (context != null)
            {
                var vsp = Microsoft.CodeAnalysis.CSharp.Scripting.CSharpScript.Create(exp, sop, context.GetType());
                var cp = vsp.Compile();

                if (cp != null && cp.Length > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var vvp in cp)
                    {
                        sb.Append(vvp.ToString());
                    }
                    LoggerService.Service.Warn("Calculate", exp + " " + sb.ToString());
                }
                mCachedScripts.Add(sid, vsp);
            }
            else
            {
                var vsp = Microsoft.CodeAnalysis.CSharp.Scripting.CSharpScript.Create(exp, sop);
                var cp = vsp.Compile();
                if (cp != null && cp.Length > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var vvp in cp)
                    {
                        sb.Append(vvp.ToString());
                    }
                    LoggerService.Service.Warn("Calculate", exp + " " + sb.ToString());
                }
                mCachedScripts.Add(sid, vsp);
            }
            return sid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public object Run(string exp, object context)
        {
            if (context != null)
                return Microsoft.CodeAnalysis.CSharp.Scripting.CSharpScript.RunAsync(exp, ScriptOptions.Default, context, context.GetType()).Result.ReturnValue;
            else
            {
                return Microsoft.CodeAnalysis.CSharp.Scripting.CSharpScript.RunAsync(exp, ScriptOptions.Default, context, context.GetType()).Result.ReturnValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public object RunById(string id, object context)
        {
            if(mCachedScripts.ContainsKey(id))
            {
                return mCachedScripts[id].RunAsync(context).Result.ReturnValue;
            }
            return null;
        }
    }
}
