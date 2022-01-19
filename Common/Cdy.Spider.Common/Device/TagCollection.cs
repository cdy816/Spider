//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/4 15:36:55.
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
    public class TagCollection:SortedDictionary<int,Tagbase>
    {

        #region ... Variables  ...

        private Dictionary<string, Tagbase> mNamedTags = new Dictionary<string, Tagbase>();

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public int MaxId { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        public void ClearAll()
        {
            this.Clear();
            mNamedTags?.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> TagNames { get { return mNamedTags.Keys; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public bool UpdateOrAdd(Tagbase tag)
        {
            if(ContainsKey(tag.Id))
            {
                var vtag = this[tag.Id];

                this[tag.Id] = tag;
                if(!mNamedTags.ContainsKey(tag.Name))
                {
                    if(mNamedTags.ContainsKey(vtag.Name))
                    {
                        mNamedTags.Remove(vtag.Name);
                    }

                    mNamedTags.Add(tag.Name, tag);
                }
                else
                {
                    mNamedTags[tag.Name] = tag;
                }
            }
            else if(tag.Id>-1)
            {
                this.Add(tag.Id, tag);
                if (!mNamedTags.ContainsKey(tag.Name))
                {
                    mNamedTags.Add(tag.Name, tag);
                }
                else
                {
                    tag.Name = this.TagNames.ToList().GetAvaiableName("tag");
                    mNamedTags.Add(tag.Name, tag);
                }
                MaxId = Math.Max(tag.Id, MaxId);
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        public bool AddTag(Tagbase tag)
        {
            if(!this.ContainsKey(tag.Id) && !mNamedTags.ContainsKey(tag.Name))
            {
                Add(tag.Id, tag);
                mNamedTags.Add(tag.Name, tag);
                MaxId = Math.Max(tag.Id, MaxId);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        public bool AppendTag(Tagbase tag)
        {
            if(!mNamedTags.ContainsKey(tag.Name))
            {
                tag.Id = ++MaxId;
                Add(tag.Id, tag);
                mNamedTags.Add(tag.Name, tag);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        public bool RemoveTag(Tagbase tag)
        {
            if(this.ContainsKey(tag.Id))
            {
                this.Remove(tag.Id);

            }
            if(mNamedTags.ContainsKey(tag.Name))
            {
                mNamedTags.Remove(tag.Name);
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool RemoveTagById(int id)
        {
            if(this.ContainsKey(id))
            {
                var tag = this[id];

                if (mNamedTags.ContainsKey(tag.Name))
                {
                    mNamedTags.Remove(tag.Name);
                }

                this.Remove(id);
            }
            return true;
        }



        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
