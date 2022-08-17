using System;

namespace Opc.Ae
{
	// Token: 0x02000071 RID: 113
	public interface ISubscription : IDisposable
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060002C6 RID: 710
		// (remove) Token: 0x060002C7 RID: 711
		event EventChangedEventHandler EventChanged;

		// Token: 0x060002C8 RID: 712
		SubscriptionState GetState();

		// Token: 0x060002C9 RID: 713
		SubscriptionState ModifyState(int masks, SubscriptionState state);

		// Token: 0x060002CA RID: 714
		SubscriptionFilters GetFilters();

		// Token: 0x060002CB RID: 715
		void SetFilters(SubscriptionFilters filters);

		// Token: 0x060002CC RID: 716
		int[] GetReturnedAttributes(int eventCategory);

		// Token: 0x060002CD RID: 717
		void SelectReturnedAttributes(int eventCategory, int[] attributeIDs);

		// Token: 0x060002CE RID: 718
		void Refresh();

		// Token: 0x060002CF RID: 719
		void CancelRefresh();
	}
}
