using System;

namespace Opc.Da
{
	// Token: 0x02000006 RID: 6
	public interface IServer : Opc.IServer, IDisposable
	{
		// Token: 0x0600002F RID: 47
		int GetResultFilters();

		// Token: 0x06000030 RID: 48
		void SetResultFilters(int filters);

		// Token: 0x06000031 RID: 49
		ServerStatus GetStatus();

		// Token: 0x06000032 RID: 50
		ItemValueResult[] Read(Item[] items);

		// Token: 0x06000033 RID: 51
		IdentifiedResult[] Write(ItemValue[] values);

		// Token: 0x06000034 RID: 52
		ISubscription CreateSubscription(SubscriptionState state);

		// Token: 0x06000035 RID: 53
		void CancelSubscription(ISubscription subscription);

		// Token: 0x06000036 RID: 54
		BrowseElement[] Browse(ItemIdentifier itemID, BrowseFilters filters, out BrowsePosition position);

		// Token: 0x06000037 RID: 55
		BrowseElement[] BrowseNext(ref BrowsePosition position);

		// Token: 0x06000038 RID: 56
		ItemPropertyCollection[] GetProperties(ItemIdentifier[] itemIDs, PropertyID[] propertyIDs, bool returnValues);
	}
}
