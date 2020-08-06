//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/6 11:33:53.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cdy.Spider
{
    /// <summary>
    /// 
    /// </summary>
    public class DeviceRuner : IDeviceRuntime,IDeviceForDriver
    {

        #region ... Variables  ...

        /// <summary>
        /// 
        /// </summary>
        Dictionary<string, Tagbae> mDatabaseMapTags = new Dictionary<string, Tagbae>();

        /// <summary>
        /// 
        /// </summary>
        Dictionary<string, List<Tagbae>> mDeviceMapTags = new Dictionary<string, List<Tagbae>>();

        /// <summary>
        /// 
        /// </summary>
        Dictionary<int, Tagbae> mIdMapTags = new Dictionary<int, Tagbae>();

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public DeviceData Device { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name => Device.Name;

        /// <summary>
        /// 
        /// </summary>
        public IDriverRuntime Driver { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Device?.Dispose();
            Device = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Init()
        {
            mIdMapTags = Device.Tags;
            foreach (var vv in Device.Tags)
            {
                string dbname = vv.Value.DatabaseName;
                string dvname = vv.Value.DeviceInfo;
                if(!mDatabaseMapTags.ContainsKey(dbname))
                {
                    mDatabaseMapTags.Add(dbname, vv.Value);
                }
                
                if (mDeviceMapTags.ContainsKey(dvname))
                {
                    mDeviceMapTags[dvname].Add(vv.Value);
                }
                else
                {
                    List<Tagbae> ll = new List<Tagbae>() { vv.Value };
                    mDeviceMapTags.Add(dvname, ll);
                }
            }
            Driver = ServiceLocator.Locator.Resolve<IDriverRuntimeManager>().GetDriver(Device.DriverName);
            Driver.Device = this;
            
            Driver.Init();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object ReadValue(string name)
        {
            return mDatabaseMapTags.ContainsKey(name) ? mDatabaseMapTags[name].Value : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public object ReadValue(int id)
        {
            return mIdMapTags.ContainsKey(id) ? mIdMapTags[id].Value : null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseTag"></param>
        /// <returns></returns>
        public object ReadValueByDatabaseName(string databaseTag)
        {
            return mDatabaseMapTags.ContainsKey(databaseTag) ? mDatabaseMapTags[databaseTag].Value : null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            Driver.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            Driver.Stop();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseTag"></param>
        /// <param name="value"></param>
        public void WriteValueByDatabaseName(string databaseTag, object value)
        {
            if (mDatabaseMapTags.ContainsKey(databaseTag))
            {
                var vtag = mDatabaseMapTags[databaseTag];
                if (vtag != null && !string.IsNullOrEmpty(vtag.DeviceInfo))
                    Driver.WriteValue(vtag.DeviceInfo, value);
            }
        }

        #region IDeviceForDriver

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Tagbae> ListTags()
        {
            return mIdMapTags.Values.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateAllTagQualityToCommBad()
        {
            DateTime dtmp = DateTime.Now;
            foreach (var vv in mIdMapTags.Values)
            {
                vv.Quality = Tagbae.BadCommQuality;
                vv.Time = dtmp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceTag"></param>
        /// <param name="value"></param>
        public void UpdateDeviceValue(string deviceTag, object value)
        {
           if(mDeviceMapTags.ContainsKey(deviceTag))
            {
                DateTime dtmp = DateTime.Now;
                foreach(var vv in  mDeviceMapTags[deviceTag])
                {
                    vv.Value = value;
                    vv.Time = dtmp;
                    vv.Quality = Tagbae.GoodQuality;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public void UpdateDeviceValue(List<int> id, object value)
        {
            DateTime dtmp = DateTime.Now;
            foreach (var vv in id)
            {
                if(mIdMapTags.ContainsKey(vv))
                {
                    var vvv = mIdMapTags[vv];
                    vvv.Value = value;
                    vvv.Quality = Tagbae.GoodQuality;
                    vvv.Time = dtmp;
                }
            }
        }


        #endregion

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
