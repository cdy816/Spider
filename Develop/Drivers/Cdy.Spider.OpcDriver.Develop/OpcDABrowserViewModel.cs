using Cdy.Spider.DevelopCommon;
using Opc;
using Opc.Ua;
using OpcCom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cdy.Spider.OpcDriver.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class OpcDABrowserViewModel: WindowViewModelBase
    {

        #region ... Variables  ...
        
        private string mServerAddress;

        private ICommand mConnectCommand;

        private ICommand mServerDiscoveryCommand;

        private List<string> mServerList;

        private Opc.Da.Server mClient;

        private System.Collections.ObjectModel.ObservableCollection<DANodeItem> mChildren = new System.Collections.ObjectModel.ObservableCollection<DANodeItem>();

        private System.Collections.ObjectModel.ObservableCollection<DAVariableItem> mVariablesChildren = new System.Collections.ObjectModel.ObservableCollection<DAVariableItem>();

        private DANodeItem mSelectItem;

        private DAVariableItem mSelectVariable;

        private string mUserName;

        private string mPassword;

        private static ServerEnumerator m_discovery = new ServerEnumerator();

        private bool mIsInited = false;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        /// <summary>
        /// 
        /// </summary>
        public OpcDABrowserViewModel()
        {
            DefaultHeight = 600;
            DefaultWidth = 1024;
            Title = Res.Get("OpcBrowserTitle");
        }

        #endregion ...Constructor...

        #region ... Properties ...


        /// <summary>
        /// 
        /// </summary>
        public string ConnectString
        {
            get
            {
                if(mClient!=null && mClient.IsConnected)
                {
                    return Res.Get("DisConnect");
                }
                else
                {
                   
                    return Res.Get("Connect");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DAVariableItem SelectVariable
        {
            get
            {
                return mSelectVariable;
            }
            set
            {
                if (mSelectVariable != value)
                {
                    mSelectVariable = value;
                    OnPropertyChanged("SelectVariable");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public DANodeItem SelectItem
        {
            get
            {
                return mSelectItem;
            }
            set
            {
                if (mSelectItem != value)
                {
                    mSelectItem = value;
                    Task.Run(new Action(() => { LoadVariables(); }));
                    OnPropertyChanged("SelectItem");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string ServerAddress
        {
            get
            {
                return mServerAddress;
            }
            set
            {
                if (mServerAddress != value)
                {
                    mServerAddress = value;
                    OnPropertyChanged("ServerAddress");
                }
            }
        }

        /// <summary>
            /// 
            /// </summary>
        public string UserName
        {
            get
            {
                return mUserName;
            }
            set
            {
                if (mUserName != value)
                {
                    mUserName = value;
                    OnPropertyChanged("UserName");
                }
            }
        }


        /// <summary>
            /// 
            /// </summary>
        public string Password
        {
            get
            {
                return mPassword;
            }
            set
            {
                if (mPassword != value)
                {
                    mPassword = value;
                    OnPropertyChanged("Password");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public ICommand ConnectCommand
        {
            get
            {
                if(mConnectCommand==null)
                {
                    mConnectCommand = new RelayCommand(() => {

                        Init();
                        if (!mClient.IsConnected)
                        {
                            if (!string.IsNullOrEmpty(UserName))
                            {
                                mClient.Connect(new ConnectData(new System.Net.NetworkCredential() { UserName = UserName, Password = Password }));
                            }
                            else
                            {
                                mClient.Connect();
                            }
                            Load();
                        }
                        else
                        {
                            mClient.Disconnect();
                        }
                        OnPropertyChanged("ConnectString");

                    });
                }
                return mConnectCommand;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public System.Collections.ObjectModel.ObservableCollection<DANodeItem> Children
        {
            get
            {
                return mChildren;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Collections.ObjectModel.ObservableCollection<DAVariableItem> VariablesChildren
        {
            get
            {
                return mVariablesChildren;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public Opc.Da.Server Client
        {
            get
            {
                return mClient;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DataGrid GridInstance { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ServerName { get; set; }

       

        #endregion ...Properties...



        #region ... Methods    ...

        private void Init()
        {
            if (!mIsInited)
            {
                mIsInited = true;
                mClient = InitClient(Specification.COM_DA_30, ServerName, ServerAddress);
                if (mClient == null)
                {
                    mClient = InitClient(Specification.COM_DA_20, ServerName, ServerAddress);
                    if (mClient == null)
                    {
                        mClient = InitClient(Specification.COM_DA_10, ServerName, ServerAddress);
                    }
                }

            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="host"></param>
        private Opc.Da.Server InitClient(Specification spec, string name, string host)
        {
            Opc.Server[] servers = m_discovery.GetAvailableServers(spec, host, null);

            if (servers != null)
            {
                foreach (Opc.Da.Server server in servers)
                {
                    if (server != null)
                    {
                        if (string.Compare(server.Name, name, true) == 0)
                        {
                            return server;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void CheckAutoConnect()
        {
            if(!string.IsNullOrEmpty(ServerAddress))
            {
                ConnectCommand.Execute(null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadVariables()
        {
            Application.Current.Dispatcher.Invoke(new Action(() => {
                VariablesChildren.Clear();
            }));

            var bbs = Client.Browse(new ItemIdentifier(SelectItem.FullName), new Opc.Da.BrowseFilters() { MaxElementsReturned = 1000, ReturnAllProperties = true, ReturnPropertyValues = true }, out Opc.Da.BrowsePosition postions);

            if(bbs!=null && bbs.Any())
            {
                foreach(var vv in bbs.Where(e=>e.IsItem))
                {
                    var vitem = new DAVariableItem(vv, this, SelectItem);
                    Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                        mVariablesChildren.Add(vitem);
                    }));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MClient_OpcStatusChange(object sender, OpcUaStatusEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                Message = e.Text + " " + e.Time;
            }));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MClient_ConnectComplete(object sender, EventArgs e)
        {
            OnPropertyChanged("ConnectString");
            if (mClient.IsConnected)
            {
                Task.Run(() => { Load(); });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Load()
        {
            Application.Current.Dispatcher.Invoke(new Action(() => {
                mChildren.Clear();
            }));

           
            if(mClient.IsConnected)
            {
                var re = mClient.Browse(null,new Opc.Da.BrowseFilters() { MaxElementsReturned=1000, ReturnAllProperties = true, ReturnPropertyValues = true }, out Opc.Da.BrowsePosition postions);
                if(re!=null && re.Any())
                {
                    foreach(var vv in re.Distinct().Where(e=>!e.IsItem && !e.Name.StartsWith("_")))
                    {
                        Application.Current.Dispatcher.Invoke(new Action(() => {
                            mChildren.Add(new DANodeItem(vv) { Parent = this });
                        }));
                        
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetSelectTags()
        {
            List<string> re = new List<string>();
            foreach (VariableItem vv in GridInstance.SelectedItems)
            {
                re.Add(vv.NodeId+ "||" + vv.DataType);
            }
            return re;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
    public class DAVariableItem : ViewModelBase
    {

        #region ... Variables  ...

        private Opc.Da.BrowseElement mModel;
        private OpcDABrowserViewModel mParent;
        private DANodeItem mOwner;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public DAVariableItem(Opc.Da.BrowseElement model, OpcDABrowserViewModel parent, DANodeItem owner)
        {
            mModel = model;
            mParent = parent;
            Init();
        }

        #endregion ...Constructor...

        #region ... Properties ...



        /// <summary>
        /// 
        /// </summary>
        public DANodeItem Owner
        {
            get
            {
                return mOwner;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string FullName
        {
            get
            {
                return Owner.FullName + "." + DisplayName;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string DisplayName
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string NodeId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string BrowseName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AccessLevel { get; set; }


        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        private void Init()
        {
            try
            {
                //todo fill property


               this.DisplayName =  this.BrowseName = this.mModel.Name;
                this.NodeId = this.mModel.ItemName;
                if (this.mModel.Properties != null)
                {
                    foreach (var vv in mModel.Properties)
                    {
                        if(vv.ID == Opc.Da.Property.DESCRIPTION )
                        {
                            this.Description = vv.Value.ToString();
                        }
                        else if(vv.ID == Opc.Da.Property.DATATYPE)
                        {
                            this.DataType = vv.Value.ToString();
                        }
                        else if (vv.ID == Opc.Da.Property.ACCESSRIGHTS)
                        {
                            this.AccessLevel = vv.Value.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }


    public class DANodeItem : ViewModelBase
    {
        /// <summary>
        /// 
        /// </summary>
        private System.Collections.ObjectModel.ObservableCollection<DANodeItem> mChildren = new System.Collections.ObjectModel.ObservableCollection<DANodeItem>();

        private Opc.Da.BrowseElement mMode;

        private bool mIsInited = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        public DANodeItem(Opc.Da.BrowseElement mode)
        {
            if (mode != null)
            {
                this.mMode = mode;
                if (this.mMode.HasChildren)
                    mChildren.Add(new DANodeItem(null));
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public System.Collections.ObjectModel.ObservableCollection<DANodeItem> Children
        {
            get
            {
                return mChildren;
            }
        }

        public NodeId NodeId
        {
            get
            {
                return mMode != null ? (NodeId)mMode.ItemName : new NodeId();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get
            {
                return mMode != null ? mMode.Name : string.Empty;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public string FullName
        {
            get
            {
                return Owner != null ? Owner.FullName + "." + this.Name : this.Name;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        private bool mIsSelected;

        /// <summary>
        /// 
        /// </summary>
        private bool mIsExpanded;

        /// <summary>
        /// 
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return mIsSelected;
            }
            set
            {
                if (mIsSelected != value)
                {
                    mIsSelected = value;
                    if (Parent != null)
                    {
                        Parent.SelectItem = this;
                    }
                    OnPropertyChanged("IsSelected");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool IsExpanded
        {
            get
            {
                return mIsExpanded;
            }
            set
            {
                if (mIsExpanded != value)
                {
                    mIsExpanded = value;
                    if (value) CheckAndLoad();
                    OnPropertyChanged("IsExpanded");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public OpcDABrowserViewModel Parent
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public DANodeItem Owner { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private void CheckAndLoad()
        {
            if (!mIsInited)
            {
                mIsInited = true;
                Children.Clear();
                if (this.mMode.HasChildren)
                {
                    Task.Run(() => {
                        var re = Parent.Client.Browse(new ItemIdentifier(this.mMode.ItemName), new Opc.Da.BrowseFilters() { MaxElementsReturned = 1000,ReturnAllProperties=true,ReturnPropertyValues=true }, out Opc.Da.BrowsePosition position);
                        if (re != null&&re.Any())
                        {
                            foreach (var vv in re.Where(e => !e.IsItem && !e.Name.StartsWith("_")))
                            {
                                var nitem = new DANodeItem(vv) { Parent = this.Parent, Owner = this };
                                Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                                    Children.Add(nitem);
                                }));

                            }
                        }

                    });

                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            Parent = null;
            mMode = null;
            base.Dispose();
        }

    }

}
