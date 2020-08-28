//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/28 16:15:19.
//  Version 1.0
//  种道洋
//==============================================================

using InSpiderDevelop;
using System;
using System.Collections.Generic;
using System.Text;

namespace InSpiderDevelopWindow
{
    /// <summary>
    /// 
    /// </summary>
    public class MachineViewModel:HasChildrenTreeItemViewModel
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
        public MachineDocument Model { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public override string Name { get => Model.Name; set => Model.Name = value; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        protected override void LoadData()
        {
            Children.Add(new DeviceRootViewModel() { Document = Model.Device });
            Children.Add(new APITreeViewModel() { Model = Model.Api.Api });
            base.LoadData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool CanAddChild()
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool CanAddGroup()
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool CanCopy()
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool CanRemove()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Remove()
        {

            base.Remove();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool CanPaste()
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool CanReName()
        {
            return true;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
