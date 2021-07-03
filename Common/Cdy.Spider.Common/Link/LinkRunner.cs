using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Cdy.Spider
{
    public abstract class LinkRunner: ILink, ILinkForFactory
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
        public abstract LinkData Data { get; }

        /// <summary>
        /// 
        /// </summary>
        public abstract string TypeName { get; }
        #endregion ...Properties...

        #region ... Methods    ...
        /// <summary>
        /// 
        /// </summary>
        public virtual void Init()
        {

        }



        /// <summary>
        /// 
        /// </summary>
        public virtual void Start()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Stop()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public virtual void Load(XElement xe)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract ILink NewLink();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="tagid"></param>
        /// <param name="value"></param>
        /// <param name="valuetype"></param>
        public virtual void WriteValue(string device,string tagid, object value,byte valuetype)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="valueUpdate"></param>
        public virtual void RegistorValueUpdateCallBack(string device, Action<Dictionary<string, object>> valueUpdate)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="valueUpdate"></param>
        public virtual void RegistorValueUpdateCallBack(string device, Action<Dictionary<int, object>> valueUpdate)
        {

        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

    }
}
