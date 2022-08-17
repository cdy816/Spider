using System;

namespace Opc.Da
{
	// Token: 0x0200009E RID: 158
	public interface ISubscription : IDisposable
	{
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060004BD RID: 1213
		// (remove) Token: 0x060004BE RID: 1214
		event DataChangedEventHandler DataChanged;

		// Token: 0x060004BF RID: 1215
		int GetResultFilters();

		// Token: 0x060004C0 RID: 1216
		void SetResultFilters(int filters);

		// Token: 0x060004C1 RID: 1217
		SubscriptionState GetState();

		// Token: 0x060004C2 RID: 1218
		SubscriptionState ModifyState(int masks, SubscriptionState state);

		// Token: 0x060004C3 RID: 1219
		ItemResult[] AddItems(Item[] items);

		// Token: 0x060004C4 RID: 1220
		ItemResult[] ModifyItems(int masks, Item[] items);

		// Token: 0x060004C5 RID: 1221
		IdentifiedResult[] RemoveItems(ItemIdentifier[] items);

		// Token: 0x060004C6 RID: 1222
		ItemValueResult[] Read(Item[] items);

		// Token: 0x060004C7 RID: 1223
		IdentifiedResult[] Write(ItemValue[] items);

		// Token: 0x060004C8 RID: 1224
		IdentifiedResult[] Read(Item[] items, object requestHandle, ReadCompleteEventHandler callback, out IRequest request);

		// Token: 0x060004C9 RID: 1225
		IdentifiedResult[] Write(ItemValue[] items, object requestHandle, WriteCompleteEventHandler callback, out IRequest request);

		// Token: 0x060004CA RID: 1226
		void Cancel(IRequest request, CancelCompleteEventHandler callback);

		// Token: 0x060004CB RID: 1227
		void Refresh();

		// Token: 0x060004CC RID: 1228
		void Refresh(object requestHandle, out IRequest request);

		// Token: 0x060004CD RID: 1229
		void SetEnabled(bool enabled);

		// Token: 0x060004CE RID: 1230
		bool GetEnabled();
	}
}
