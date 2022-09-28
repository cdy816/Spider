using Opc.Ua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Api.OpcUAServer
{
    internal class SpiderTag : BaseDataVariableState
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        public SpiderTag(NodeState parent) : base(parent)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 设备
        /// </summary>
        public object Device { get; set; }

        /// <summary>
        /// 设备内变量的ID
        /// </summary>
        public string DID { get; set; }
    }
}
