//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/28 16:15:19.
//  Version 1.0
//  种道洋
//==============================================================

using Cdy.Spider;
using InSpiderDevelop;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;

namespace InSpiderDevelopWindow
{
    /// <summary>
    /// 
    /// </summary>
    public class MachineViewModel:HasChildrenTreeItemViewModel
    {

        #region ... Variables  ...
        
        private MachineDocument mModel;

        private ICommand mExportCommand;
        private ICommand mImportCommand;

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

        public ICommand ExportCommand 
        {
            get 
            {
                if(mExportCommand == null)
                {
                    mExportCommand = new RelayCommand(() => {
                        DoExport();
                    });
                }
                return mExportCommand;
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand ImportCommand 
        {
            get 
            {
                if(mImportCommand == null)
                {
                    mImportCommand = new RelayCommand(() =>
                    {
                        DoImport();
                    });
                }
                return mImportCommand;
            }
        }

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

        private void DoExport()
        {
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.Filter = "zip file|*.zip";
            if (ofd.ShowDialog().Value)
            {

                string spath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(ofd.FileName), "tmp"+DateTime.Now.Ticks);
                if (!Directory.Exists(spath))
                    Directory.CreateDirectory(spath);

                this.Model.Api.Save(System.IO.Path.Combine(spath, "Api.cfg"));
                this.Model.Channel.Save(System.IO.Path.Combine(spath, "Channel.cfg"));
                this.Model.Device.Save(System.IO.Path.Combine(spath, "Device.cfg"));
                this.Model.Driver.Save(System.IO.Path.Combine(spath, "Driver.cfg"));
                this.Model.Link.Save(System.IO.Path.Combine(spath, "Link.cfg"));

                System.IO.Compression.ZipFile.CreateFromDirectory(spath, ofd.FileName);

                System.IO.Directory.Delete(spath,true);

                MessageBox.Show(Res.Get("completely"));

            }
        }

        private void DoImport()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "zip file|*.zip";
            if (ofd.ShowDialog().Value)
            {
                try
                {
                    string spath = System.IO.Path.Combine( System.IO.Path.GetTempPath(), "tmp" + DateTime.Now.Ticks);
                    if (!Directory.Exists(spath))
                        Directory.CreateDirectory(spath);
                    System.IO.Compression.ZipFile.ExtractToDirectory(ofd.FileName,spath,true);

                  
                    using (Context context = new Context())
                    {
                        string sfile = System.IO.Path.Combine(spath, "Api.cfg");
                        if (System.IO.File.Exists(sfile))
                        {
                            this.Model.Api = APIDocument.Manager.LoadFromString(System.IO.File.ReadAllText(sfile));
                        }

                        sfile = System.IO.Path.Combine(spath, "Channel.cfg");
                        if (System.IO.File.Exists(sfile))
                        {
                            this.Model.Channel = ChannelDocument.Manager.LoadFromString(System.IO.File.ReadAllText(sfile),context);
                        }

                        sfile = System.IO.Path.Combine(spath, "Driver.cfg");
                        if (System.IO.File.Exists(sfile))
                        {
                            this.Model.Driver = DriverDocument.Manager.LoadFromString(System.IO.File.ReadAllText(sfile),context);
                        }

                        sfile = System.IO.Path.Combine(spath, "Device.cfg");
                        if (System.IO.File.Exists(sfile))
                        {
                            this.Model.Device = DeviceDocument.Manager.LoadFromString(System.IO.File.ReadAllText(sfile), context);
                        }

                        sfile = System.IO.Path.Combine(spath, "Link.cfg");
                        if (System.IO.File.Exists(sfile))
                        {
                            this.Model.Link = LinkDocument.Manager.LoadFromString(System.IO.File.ReadAllText(sfile));
                        }
                    }

                    System.IO.Directory.Delete(spath, true);
                    this.LoadData();
                    MessageBox.Show(Res.Get("importCompletely"));
                }
                catch
                {
                    MessageBox.Show("无效数据!");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void LoadData()
        {
            //this.Model.Load();
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
            if (MessageBox.Show(Res.Get("RemoveConfirm"), Res.Get("Remove"), MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Parent?.RemoveMachine(this);
                base.Remove();
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
            if (Parent != null)
            {
                return Parent.RenameMachine(this, oldName, newName);
            }
            return false;
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
