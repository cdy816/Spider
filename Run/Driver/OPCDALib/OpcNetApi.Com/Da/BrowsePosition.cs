using System;
using Opc;
using Opc.Da;

namespace OpcCom.Da
{
    [Serializable]
    internal class BrowsePosition : Opc.Da.BrowsePosition
    {
        internal string ContinuationPoint;

        internal bool MoreElements;

        internal BrowsePosition(ItemIdentifier itemID, BrowseFilters filters, string continuationPoint)
            : base(itemID, filters)
        {
            ContinuationPoint = continuationPoint;
        }
    }
}
