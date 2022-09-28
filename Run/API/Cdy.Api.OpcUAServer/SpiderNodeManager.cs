using Cdy.Spider;
using Opc.Ua;
using Opc.Ua.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Api.OpcUAServer
{
    /// <summary>
    /// Spider 数据节点
    /// </summary>
    internal class SpiderNodeManager : CustomNodeManager2
    {

        private Dictionary<string, FolderState> mFolders = new Dictionary<string, FolderState>();

        //private Dictionary<int, MarsTag> mTags = new Dictionary<int, MarsTag>();

        private Dictionary<NodeId, SpiderTag> mIdTagMaps = new Dictionary<NodeId, SpiderTag>();

        private bool mIsClosed = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="server"></param>
        /// <param name="configuration"></param>
        public SpiderNodeManager(IServerInternal server, ApplicationConfiguration configuration) : base(server, configuration, "http://cdyfoundationorg/Mars")
        {
        }

        protected SpiderNodeManager(IServerInternal server, ApplicationConfiguration configuration, params string[] namespaceUris) : base(server, configuration, namespaceUris)
        {
        }

        /// <summary>
        /// 重写NodeId生成方式(目前采用'_'分隔,如需更改,请修改此方法)
        /// </summary>
        /// <param name="context"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public override NodeId New(ISystemContext context, NodeState node)
        {
            BaseInstanceState instance = node as BaseInstanceState;

            if (instance != null && instance.Parent != null)
            {
                string id = instance.Parent.NodeId.Identifier as string;

                if (id != null)
                {
                    return new NodeId(id + "_" + instance.SymbolicName, instance.Parent.NodeId.NamespaceIndex);
                }
            }

            return node.NodeId;
        }

        /// <summary>
        /// 重写获取节点句柄的方法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="nodeId"></param>
        /// <param name="cache"></param>
        /// <returns></returns>
        protected override NodeHandle GetManagerHandle(ServerSystemContext context, NodeId nodeId, IDictionary<NodeId, NodeState> cache)
        {
            lock (Lock)
            {
                // quickly exclude nodes that are not in the namespace. 
                if (!IsNodeIdInNamespace(nodeId))
                {
                    return null;
                }

                NodeState node = null;

                if (!PredefinedNodes.TryGetValue(nodeId, out node))
                {
                    return null;
                }

                NodeHandle handle = new NodeHandle();

                handle.NodeId = nodeId;
                handle.Node = node;
                handle.Validated = true;

                return handle;
            }
        }

        /// <summary>
        /// 重写节点的验证方式
        /// </summary>
        /// <param name="context"></param>
        /// <param name="handle"></param>
        /// <param name="cache"></param>
        /// <returns></returns>
        protected override NodeState ValidateNode(ServerSystemContext context, NodeHandle handle, IDictionary<NodeId, NodeState> cache)
        {
            // not valid if no root.
            if (handle == null)
            {
                return null;
            }

            // check if previously validated.
            if (handle.Validated)
            {
                return handle.Node;
            }
            // TBD
            return null;
        }

        /// <summary>
        /// 创建对外服务的目录结构
        /// </summary>
        /// <param name="externalReferences"></param>
        public override void CreateAddressSpace(IDictionary<NodeId, IList<IReference>> externalReferences)
        {
            lock (Lock)
            {
                IList<IReference> references = null;

                if (!externalReferences.TryGetValue(ObjectIds.ObjectsFolder, out references))
                {
                    externalReferences[ObjectIds.ObjectsFolder] = references = new List<IReference>();
                }

                FolderState root = CreateFolder(null, "device", "device");
                root.AddReference(ReferenceTypes.Organizes, true, ObjectIds.ObjectsFolder);
                references.Add(new NodeStateReference(ReferenceTypes.Organizes, false, root.NodeId));
                root.EventNotifier = EventNotifiers.SubscribeToEvents;
                AddRootNotifier(root);
                mFolders.Add("", root);

                FillFolderNode(root, "");

                AddPredefinedNode(SystemContext, root);

                StartDataSync();
            }
        }


        private void FillFolderNode(NodeState parent,string parentFoldName)
        {
            var manager = ServiceLocator.Locator.Resolve<IDeviceRuntimeManager>();
            foreach (var vv in manager.ListDevice())
            {
                var fs = CreateFolder(parent, vv.Name, vv.Name);
                mFolders.Add(vv.Name, fs);
                FillTagNode(fs, vv);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="group"></param>
        private void FillTagNode(NodeState parent,IDeviceRuntime device)
        {
            foreach (var vv in device.ListTags())
            {
                CreateVariable(parent, device, vv);
            }
        }
        
        /// <summary>
        /// Creates a new folder.
        /// </summary>
        private FolderState CreateFolder(NodeState parent, string path, string name)
        {
            FolderState folder = new FolderState(parent);

            folder.SymbolicName = name;
            folder.ReferenceTypeId = ReferenceTypes.Organizes;
            folder.TypeDefinitionId = ObjectTypeIds.FolderType;
            folder.NodeId = new NodeId(path, NamespaceIndex);
            folder.BrowseName = new QualifiedName(path, NamespaceIndex);
            folder.DisplayName = new LocalizedText("en", name);
            folder.WriteMask = AttributeWriteMask.None;
            folder.UserWriteMask = AttributeWriteMask.None;
            folder.EventNotifier = EventNotifiers.None;

            if (parent != null)
            {
                parent.AddChild(folder);
            }

            return folder;
        }

        /// <summary>
        /// 创建节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <param name="dataType"></param>
        /// <param name="valueRank"></param>
        /// <returns></returns>
        private SpiderTag CreateVariable(NodeState parent,IDeviceRuntime device, Tagbase tag)
        {
            SpiderTag variable = new SpiderTag(parent);

            variable.SymbolicName = tag.Name;
            variable.ReferenceTypeId = ReferenceTypes.Organizes;
            variable.TypeDefinitionId = VariableTypeIds.BaseDataVariableType;
            variable.NodeId = new NodeId(tag.Name, NamespaceIndex);
            variable.BrowseName = new QualifiedName(tag.Name);
            variable.DisplayName = new LocalizedText("en", tag.Name);
            variable.WriteMask = AttributeWriteMask.DisplayName | AttributeWriteMask.Description;
            variable.UserWriteMask = AttributeWriteMask.DisplayName | AttributeWriteMask.Description;
            variable.ValueRank = ValueRanks.Scalar;

            switch (tag.Type)
            {
                case TagType.Bool:
                    variable.DataType = DataTypeIds.Boolean;
                    break;
                case TagType.Short:
                    variable.DataType = DataTypeIds.Int16;
                    break;
                case TagType.UShort:
                    variable.DataType = DataTypeIds.UInt16;
                    break;
                case TagType.Int:
                    variable.DataType = DataTypeIds.Int32;
                    break;
                case TagType.UInt:
                    variable.DataType = DataTypeIds.UInt32;
                    break;
                case TagType.Long:
                    variable.DataType = DataTypeIds.Int64;
                    break;
                case TagType.ULong:
                    variable.DataType = DataTypeIds.UInt64;
                    break;
                case TagType.Double:
                    variable.DataType = DataTypeIds.Double;
                    break;
                case TagType.Float:
                    variable.DataType = DataTypeIds.Float;
                    break;
                case TagType.DateTime:
                    variable.DataType = DataTypeIds.DateTime;
                    break;
                case TagType.String:
                    variable.DataType = DataTypeIds.String;
                    break;
                case TagType.Byte:
                    variable.DataType = DataTypeIds.Byte;
                    break;
                case TagType.IntPoint:
                    variable.DataType = DataTypeIds.String;
                    break;
                case TagType.UIntPoint:
                    variable.DataType = DataTypeIds.String;
                    break;
                case TagType.IntPoint3:
                    variable.DataType = DataTypeIds.String;
                    break;
                case TagType.UIntPoint3:
                case TagType.LongPoint:
                case TagType.ULongPoint:
                case TagType.LongPoint3:
                case TagType.ULongPoint3:
                    variable.DataType = DataTypeIds.String;
                    break;
            }
            variable.AccessLevel = AccessLevels.CurrentReadOrWrite;
            variable.UserAccessLevel = AccessLevels.CurrentReadOrWrite;
            variable.Historizing = false;
            variable.StatusCode = StatusCodes.Good;
            variable.Timestamp = DateTime.Now;
            variable.OnWriteValue = OnWriteDataValue;
            variable.Id = tag.Id;
            variable.DID = tag.Name;
            variable.Device = device;

            if (parent != null)
            {
                parent.AddChild(variable);
            }
            //mTags.Add(tag.Id, variable);
            mIdTagMaps.Add(variable.NodeId, variable);
            return variable;
        }

        public override void Read(OperationContext context, double maxAge, IList<ReadValueId> nodesToRead, IList<DataValue> values, IList<ServiceResult> errors)
        {
            base.Read(context, maxAge, nodesToRead, values, errors);
        }

        /// <summary>
        /// 客户端写入值时触发(绑定到节点的写入事件上)
        /// </summary>
        /// <param name="context"></param>
        /// <param name="node"></param>
        /// <param name="indexRange"></param>
        /// <param name="dataEncoding"></param>
        /// <param name="value"></param>
        /// <param name="statusCode"></param>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        private ServiceResult OnWriteDataValue(ISystemContext context, NodeState node, NumericRange indexRange, QualifiedName dataEncoding,
            ref object value, ref StatusCode statusCode, ref DateTime timestamp)
        {
            BaseDataVariableState variable = node as BaseDataVariableState;
            try
            {
                //验证数据类型
                TypeInfo typeInfo = TypeInfo.IsInstanceOfDataType(
                    value,
                    variable.DataType,
                    variable.ValueRank,
                    context.NamespaceUris,
                    context.TypeTable);

                if (typeInfo == null || typeInfo == TypeInfo.Unknown)
                {
                    return StatusCodes.BadTypeMismatch;
                }

                if (mIdTagMaps.TryGetValue(node.NodeId, out var tag))
                {
                    var vid = tag.Id;
                    var vtag = (tag.Device as IDeviceRuntime).GetTag(tag.DID);
                    if (vtag != null)
                    {
                        vtag.Value = value;
                        return ServiceResult.Good;
                    }
                    else
                    {
                        return StatusCodes.Bad;
                    }
                }

                return ServiceResult.Good;
            }
            catch (Exception)
            {
                return StatusCodes.BadTypeMismatch;
            }
        }

        private void StartDataSync()
        {
            var manager = ServiceLocator.Locator.Resolve<IDeviceRuntimeManager>();
            Task.Run(() => { 
                while(!mIsClosed)
                {
                    foreach(var vv in mIdTagMaps.Values)
                    {
                        var vtag = (vv.Device as IDeviceRuntime).GetTag(vv.DID);
                        if (vtag == null) continue;

                        var oldval = vtag.Value;
                        switch ((TagType)(vtag.Type))
                        {
                            case TagType.IntPoint:
                            case TagType.UIntPoint:
                            case TagType.IntPoint3:
                            case TagType.UIntPoint3:
                            case TagType.LongPoint:
                            case TagType.ULongPoint:
                            case TagType.LongPoint3:
                            case TagType.ULongPoint3:
                                vv.Value = vtag.Value.ToString();
                                break;
                            default:
                                vv.Value =vtag.Value;
                                break;
                        }

                        vv.Timestamp = vtag.Time;
                        vv.StatusCode = IsGoodQuality(vtag.Quality) ? StatusCodes.Good: StatusCodes.Bad;
                        if(oldval != vtag.Value)
                        {
                            vv.ClearChangeMasks(SystemContext, false);
                        }
                    }
                    Thread.Sleep(500);
                }
            });
        }

        private bool IsGoodQuality(byte qua)
        {
            return true;
        }
    }


    public class ValueItem
    {

        #region ... Variables  ...

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public Variant Value { get; set; }
        /// <summary>
        /// 质量戳
        /// </summary>
        public byte Quality { get; set; }
        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }


}
