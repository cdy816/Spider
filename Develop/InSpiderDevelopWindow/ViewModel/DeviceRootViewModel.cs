//==============================================================
//  Copyright (C) 2020 Chongdaoyang Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/25 20:16:01 .
//  Version 1.0
//  CDYWORK
//==============================================================

using Cdy.Spider;
using InSpiderDevelop;
using InSpiderDevelop.Device;
using InSpiderDevelopWindow.ViewModel;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace InSpiderDevelopWindow
{

    public class CopyPasteHelper
    {

        #region ... Variables  ...
        /// <summary>
        /// 
        /// </summary>
        public static CopyPasteHelper Helper = new CopyPasteHelper();
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public object CopyObj { get; set; }

        #endregion ...Properties...

        #region ... Methods    ...


        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }


    /// <summary>
    /// 
    /// </summary>
    public class DeviceGroupViewModel : HasChildrenTreeItemViewModel
    {

        #region ... Variables  ...
        
        private DeviceGroup mModel;

        /// <summary>
        /// 
        /// </summary>
        private DeviceDocument mDocument;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...



        /// <summary>
        /// 
        /// </summary>
        public virtual DeviceDocument Document
        {
            get
            {
                return mDocument;
            }
            set
            {
                mDocument = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DeviceGroup Model
        {
            get
            {
                return mModel;
            }
            set
            {
                if (mModel != value)
                {
                    mModel = value;
                    mName = value.Name;
                    this.ParseName(mName);
                    Init();
                    OnPropertyChanged("Model");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override string FullName => mModel.FullName;

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        protected virtual void Init()
        {
            if (mDocument == null)
            {
                return;
            }
            var vcount = mDocument.ListDevice(this.FullName).Count + mDocument.ListDeviceGroup(this.FullName).Count;
            if (vcount > 0)
                this.PreLoadChildForExpend(true);
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
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool CanCopy()
        {
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool CanPaste()
        {
            return CopyPasteHelper.Helper.CopyObj != null;
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
        public override void Copy()
        {
            CopyPasteHelper.Helper.CopyObj = this.Model;
            base.Copy();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Paste()
        {
            ProcessPaste();
            base.Paste();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private MachineViewModel GetMachineViewModel(TreeItemViewModel model)
        {
            if (model is MachineViewModel) return model as MachineViewModel;
            else
            {
                return GetMachineViewModel(model.Parent);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ProcessPaste()
        {
            if (CopyPasteHelper.Helper.CopyObj is IDeviceDevelop)
            {
                var vv = (CopyPasteHelper.Helper.CopyObj as IDeviceDevelop).Clone();
                vv.Group = this.FullName;
                vv.Name = mDocument.GetAvaiableName(vv.Name, vv.Group);

                if (Document.AddDevice(vv))
                {
                    var vvv = new DeviceTreeViewModel() { Document = this.Document, Model = vv, Parent = this };
                    this.Children.Add(vvv);
                    OnNameRefresh();

                    var driver = (vv as DeviceDevelop).Driver;
                    var driverdoc = GetMachineViewModel(this).Model.Driver;
                    driverdoc?.AddDriver(driver);
                }
            }
            else if (CopyPasteHelper.Helper.CopyObj is DeviceGroup)
            {
                DeviceGroup dgg = CopyPasteHelper.Helper.CopyObj as DeviceGroup;
                PasteGroup(dgg,this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        private void PasteGroup(DeviceGroup group,DeviceGroupViewModel parentViewModel)
        {
            var vgg = mDocument.GetGroups(group);
            var ggg = group.Clone();

            DeviceGroup parent = (this.Parent is DeviceGroupViewModel) ? (Parent as DeviceGroupViewModel).Model : null;

            ggg.Name = Document.GetAvaiableGroupName(ggg.Name, parent != null ? parent.FullName : string.Empty);

            Document.AddDeviceGroup(parent, ggg);
            var vmodel = new DeviceGroupViewModel() { Model = ggg,Document=this.Document,Parent=parentViewModel };
            parentViewModel.Children.Add(vmodel);
            

            foreach (IDeviceDevelop vv in ggg.Devices.ToArray())
            {
                vv.Group = ggg.FullName;
                Document.AddDevice(vv);

                vmodel.Children.Add(new DeviceTreeViewModel() { Model = vv, Document = this.Document,Parent=vmodel });

                var driver = (vv as DeviceDevelop).Driver;
                if (driver != null)
                {
                    var driverdoc = GetMachineViewModel(this).Model.Driver;
                    driverdoc?.AddDriver(driver);
                }
            }

            foreach (var vv in vgg)
            {
                PasteGroup(vv, vmodel);
            }
            parentViewModel.RefreshView();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Remove()
        {
            mDocument.RemoveGroup(this.FullName);
            (Parent as HasChildrenTreeItemViewModel).Children.Remove(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        public override bool OnRename(string oldName, string newName)
        {
            return mDocument.ChangeGroupName(this.FullName, newName);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void LoadData()
        {
            foreach (var vv in mDocument.ListDeviceGroup(this.FullName))
            {
                var vmm = new DeviceGroupViewModel() { Document = this.Document, Model = vv, Parent = this };
                Children.Add(vmm);
            }

            foreach (var vv in mDocument.ListDevice(this.FullName))
            {
                var vvv = new DeviceTreeViewModel() {  Document = this.Document, Model = vv, Parent = this };
                Children.Add(vvv);
            }
        }

       

        /// <summary>
        /// 
        /// </summary>
        public override void AddGroup()
        {
            string sname = mDocument.GetAvaiableGroupName("Group",this.FullName);
            var vgd = new DeviceGroup() { Parent = this.mModel, Name = sname };
            if(mDocument.AddDeviceGroup(this.Model,vgd))
            {
                var vmm = new DeviceGroupViewModel() {  Document = this.Document, Model = vgd,Parent=this };
                this.Children.Add(vmm);
                vmm.IsSelected = true;
                vmm.IsEdit = true;
            }
            this.IsExpanded = true;
            base.AddGroup();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Add()
        {
            string sname = this.Document.GetAvaiableName("Device",this.FullName);
            var vd = new DeviceDevelop() { Data = new DeviceData() ,Group = this.FullName};
            vd.Name = sname;
            if (mDocument.AddDevice(vd))
            {
                var vmm = new DeviceTreeViewModel() { Document=this.Document, Model = vd, Parent = this,IsCommFirst=true };
                this.Children.Add(vmm);
                vmm.IsSelected = true;
                vmm.IsEdit = true;
            }
            this.IsExpanded = true;
            base.Add();
        }
        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

    /// <summary>
    /// 
    /// </summary>
    public class DeviceRootViewModel: DeviceGroupViewModel
    {



        /// <summary>
        /// 
        /// </summary>
        public override string Name { get => Res.Get("Device"); set => base.Name = value; }

        /// <summary>
        /// 
        /// </summary>
        public override string FullName => string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public override DeviceDocument Document { get { return base.Document; } set{ base.Document = value; base.Init(); } }


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
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool CanRemove()
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
        public override bool CanReName()
        {
            return false;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class DeviceTreeViewModel : TreeItemViewModel
    {

        #region ... Variables  ...
       
        private IDeviceDevelop mModel;
        private DeviceDocument mDocument;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public virtual DeviceDocument Document
        {
            get
            {
                return mDocument;
            }
            set
            {
                mDocument = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IDeviceDevelop Model
        {
            get
            {
                return mModel;
            }
            set
            {
                if (mModel != value)
                {
                    mModel = value;
                    mName = value.Name;
                    this.ParseName(mName);
                    OnPropertyChanged("Model");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool IsCommFirst { get; set; }



        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        protected override void OnNameRefresh()
        {
            (Parent as HasChildrenTreeItemViewModel)?.RefreshView();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private MachineViewModel GetMachineViewModel(TreeItemViewModel model)
        {
            if (model is MachineViewModel) return model as MachineViewModel;
            else
            {
                return GetMachineViewModel(model.Parent);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public override ViewModelBase GetModel(ViewModelBase mode)
        {
            var vmm = GetMachineViewModel(this);
            if (mode is DeviceDetailViewModel)
            {
                (mode as DeviceDetailViewModel).Model = this.Model;
                (mode as DeviceDetailViewModel).MachineModel = vmm.Model;

                if (IsCommFirst)
                {
                    (mode as DeviceDetailViewModel).SelectIndex = 0;
                    IsCommFirst = false;
                }
                return mode;
            }
            else
            {
                var re = new DeviceDetailViewModel() { Model = this.Model,MachineModel = vmm.Model,SelectIndex=IsCommFirst?0:1 };
                IsCommFirst = false;
                return re;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        public override bool OnRename(string oldName, string newName)
        {
            return mDocument.ReName(this.Model, newName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool CanCopy()
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool CanPaste()
        {
            return CopyPasteHelper.Helper.CopyObj!=null;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Copy()
        {
            CopyPasteHelper.Helper.CopyObj = this.Model;
            base.Copy();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Paste()
        {
            ProcessPaste();
            base.Paste();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ProcessPaste()
        {
            if(CopyPasteHelper.Helper.CopyObj is IDeviceDevelop)
            {
                var vv = (CopyPasteHelper.Helper.CopyObj as IDeviceDevelop).Clone();
                vv.Group = this.Parent != null ? this.Parent.FullName : string.Empty;
                vv.Name = mDocument.GetAvaiableName(vv.Name, vv.Group);

                if (Document.AddDevice(vv))
                {
                    var vvv = new DeviceTreeViewModel() { Document = this.Document, Model = vv, Parent = this.Parent };
                    (Parent as HasChildrenTreeItemViewModel).Children.Add(vvv);
                    OnNameRefresh();
                    var driver = (vv as DeviceDevelop).Driver;
                    if (driver!=null)
                    {
                        var driverdoc = GetMachineViewModel(this).Model.Driver;
                        driverdoc?.AddDriver(driver);
                    }
                }
            }
            else if(CopyPasteHelper.Helper.CopyObj is DeviceGroup)
            {
                DeviceGroup dgg = CopyPasteHelper.Helper.CopyObj as DeviceGroup;
                PasteGroup(dgg,this.Parent as DeviceGroupViewModel);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        private void PasteGroup(DeviceGroup group, DeviceGroupViewModel parentViewModel)
        {
            var vgg = mDocument.GetGroups(group);
            var ggg = group.Clone();

            DeviceGroup parent = (this.Parent is DeviceGroupViewModel) ? (Parent as DeviceGroupViewModel).Model : null;

            ggg.Name = Document.GetAvaiableGroupName(ggg.Name, parent != null ? parent.FullName : "");

            Document.AddDeviceGroup(parent, ggg);

            var vmodel = new DeviceGroupViewModel() { Model = ggg, Document = this.Document,Parent=parentViewModel };
            parentViewModel.Children.Add(vmodel);

            foreach (IDeviceDevelop vv in ggg.Devices.ToArray())
            {
                vv.Group = ggg.FullName;
                Document.AddDevice(vv);
                vmodel.Children.Add(new DeviceTreeViewModel() { Model = vv, Document = this.Document,Parent=vmodel });
                var driver = (vv as DeviceDevelop).Driver;
                if (driver != null)
                {
                    var driverdoc = GetMachineViewModel(this).Model.Driver;
                    driverdoc?.AddDriver(driver);
                }
            }

            foreach(var vv in vgg)
            {
                PasteGroup(vv, vmodel);
            }

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
            mDocument.RemoveDevice(this.Model.FullName);

            var vp = (Parent as HasChildrenTreeItemViewModel);
            int id = vp.Children.IndexOf(this);
            if (id > 0)
            {
                if (id >= vp.Children.Count - 1)
                {
                    id = vp.Children.Count - 2;
                }
                if (id < 0) id = 0;
            }
            vp.Children.Remove(this);

            if(vp.Children.Count>id)
            {
                vp.Children[id].IsSelected = true;
            }

            base.Remove();
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
        public override void Add()
        {
            Parent.AddCommand.Execute(null);
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

    /// <summary>
    /// 
    /// </summary>
    public class APITreeViewModel : TreeItemViewModel
    {

        #region ... Variables  ...
        private IApiDevelop mModel;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public IApiDevelop Model
        {
            get
            {
                return mModel;
            }
            set
            {
                if (mModel != value)
                {
                    mModel = value;
                    OnPropertyChanged("Model");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override string Name { get => Res.Get("API"); set => base.Name = value; }


        #endregion ...Properties...

        #region ... Methods    ...

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
        public override bool CanCopy()
        {
            return false;
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
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool CanRemove()
        {
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public override ViewModelBase GetModel(ViewModelBase mode)
        {
            if (mode is APIDetailViewModel)
            {
                (mode as APIDetailViewModel).Model = this.Model;
                return mode;
            }
            else
            {
                return new APIDetailViewModel() { Model = this.Model };
            }
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

   

}
