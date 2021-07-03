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
using System.Windows.Controls.Ribbon;

namespace InSpiderDevelopWindow
{
    /// <summary>
    /// 
    /// </summary>
    public class MachineViewModel:HasChildrenTreeItemViewModel
    {

        #region ... Variables  ...
        
        private MachineDocument mModel;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        public MachineViewModel()
        {
            Children.Add(new TreeItemViewModel());
        }
        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public MachineDocument Model 
        {
            get { return mModel; }
            set
            {
                mModel = value;
                mName = mModel.Name;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public new MainViewModel Parent { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        protected override void LoadData()
        {
            this.Model.Load();
            Children.Clear();
            Children.Add(new DeviceRootViewModel() { Document = Model.Device, Parent = this });
            Children.Add(new APITreeViewModel() { Model = Model.Api.Api, Parent = this,MachineModel=Model });
            Children.Add(new LinkTreeViewModel() { Model = Model.Link.Link, Parent = this, MachineModel = Model });
            base.LoadData();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Add()
        {
            Parent?.AddMachine();
            base.Add();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Remove()
        {
            Parent?.RemoveMachine(this);
            base.Remove();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        public override bool OnRename(string oldName, string newName)
        {
            return DevelopManager.Manager.ReName(oldName,newName);
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool CanAddChild()
        {
            
            return true;
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
