using Cdy.Spider.DevelopCommon;
using Opc.Ua;
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
    public class OpcBrowserViewModel: WindowViewModelBase
    {

        #region ... Variables  ...
        
        private static string mServerAddress;

        private ICommand mConnectCommand;

        private ICommand mServerDiscoveryCommand;

        private List<string> mServerList;

        private OpcUaClient mClient;

        private string mStateMessage;

        private System.Collections.ObjectModel.ObservableCollection<NodeItem> mChildren = new System.Collections.ObjectModel.ObservableCollection<NodeItem>();

        private System.Collections.ObjectModel.ObservableCollection<VariableItem> mVariablesChildren = new System.Collections.ObjectModel.ObservableCollection<VariableItem>();

        private NodeItem mSelectItem;

        private VariableItem mSelectVariable;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        /// <summary>
        /// 
        /// </summary>
        public OpcBrowserViewModel()
        {
            mClient = new OpcUaClient();
            mClient.ConnectComplete += MClient_ConnectComplete;
            mClient.OpcStatusChange += MClient_OpcStatusChange;
            DefaultHeight = 800;
            DefaultWidth = 1024;
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
        public VariableItem SelectVariable
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
        public NodeItem SelectItem
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
        public string StateMessage
        {
            get
            {
                return mStateMessage;
            }
            set
            {
                if (mStateMessage != value)
                {
                    mStateMessage = value;
                    OnPropertyChanged("StateMessage");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public List<string> ServerList
        {
            get
            {
                return mServerList;
            }
            set
            {
                if (mServerList != value)
                {
                    mServerList = value;
                    OnPropertyChanged("ServerList");
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
        public ICommand ConnectCommand
        {
            get
            {
                if(mConnectCommand==null)
                {
                    mConnectCommand = new RelayCommand(() => {

                        if (!mClient.IsConnected)
                        {
                            mClient.ConnectServer(ServerAddress).Wait();
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
        public ICommand ServerDiscoveryCommand
        {
            get
            {
                if(mServerDiscoveryCommand==null)
                {
                    mServerDiscoveryCommand = new RelayCommand(() =>
                    {
                        try
                        {
                            ServerList = mClient.Configuration.DiscoverServers();
                        }
                        catch(Exception ex)
                        {
                            System.Windows.MessageBox.Show(ex.Message);
                        }
                    });
                }
                return mServerDiscoveryCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Collections.ObjectModel.ObservableCollection<NodeItem> Children
        {
            get
            {
                return mChildren;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Collections.ObjectModel.ObservableCollection<VariableItem> VariablesChildren
        {
            get
            {
                return mVariablesChildren;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public OpcUaClient Client
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

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        private void LoadVariables()
        {
            Application.Current.Dispatcher.Invoke(new Action(() => {
                VariablesChildren.Clear();
            }));

            var res = Client.Browse(SelectItem.NodeId, ReferenceTypeIds.Organizes, ReferenceTypeIds.Aggregates);

            if(res!=null)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                    foreach (var vv in res.Where(e => e.NodeClass == NodeClass.Variable))
                    {
                        mVariablesChildren.Add(new VariableItem(vv, this));
                    }
                }));
                
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
                StateMessage = e.Error + "" + e.Text + " " + e.Time;
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
                var re = mClient.Browse(Opc.Ua.ObjectIds.ObjectsFolder, ReferenceTypeIds.Organizes, ReferenceTypeIds.Aggregates);
                if(re!=null)
                {
                    foreach(var vv in re)
                    {
                        Application.Current.Dispatcher.Invoke(new Action(() => {
                            mChildren.Add(new NodeItem(vv) { Parent = this });
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
                re.Add(vv.NodeId);
            }
            return re;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }

    public class VariableItem : ViewModelBase
    {

        #region ... Variables  ...
        
        private ReferenceDescription mModel;
        private OpcBrowserViewModel mParent;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public VariableItem(ReferenceDescription model, OpcBrowserViewModel parent)
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
        public string DisplayName
        {
            get;set;
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
            var re = mParent.Client.ReadAttributes((NodeId)this.mModel.NodeId);
            if(re!=null && re.Count>0)
            {
                this.NodeId = re["NodeId"];
                this.BrowseName = re["BrowseName"];
                this.DisplayName = re["DisplayName"];
                this.Description = re["Description"];
                this.DataType = re["DataType"];
                this.AccessLevel = re["AccessLevel"];
            }
            
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }


    public class NodeItem : ViewModelBase
    {
        /// <summary>
        /// 
        /// </summary>
        private System.Collections.ObjectModel.ObservableCollection<NodeItem> mChildren = new System.Collections.ObjectModel.ObservableCollection<NodeItem>();

        private Opc.Ua.ReferenceDescription mMode;

        private bool mIsInited = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        public NodeItem(Opc.Ua.ReferenceDescription mode)
        {
            this.mMode = mode;
            //if (this.mMode.NodeClass == NodeClass.Object)
            //    mChildren.Add(new NodeItem(null));
        }


        /// <summary>
        /// 
        /// </summary>
        public System.Collections.ObjectModel.ObservableCollection<NodeItem> Children
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
                return (NodeId)mMode.NodeId;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get
            {
                return mMode.DisplayName.Text;
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
                    if(Parent!=null)
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
        public OpcBrowserViewModel Parent
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        private void CheckAndLoad()
        {
            if(!mIsInited)
            {
                mIsInited = true;
                Children.Clear();
                if (this.mMode.NodeClass == NodeClass.Object)
                {
                    var re = Parent.Client.Browse((NodeId)this.mMode.NodeId, ReferenceTypeIds.Organizes, ReferenceTypeIds.Aggregates);
                    foreach (var vv in re)
                    {
                        Children.Add(new NodeItem(vv) { Parent = this.Parent });
                    }
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
