using System;

namespace Opc.Ae
{
	// Token: 0x0200000C RID: 12
	public interface IServer : Opc.IServer, IDisposable
	{
		// Token: 0x06000073 RID: 115
		ServerStatus GetStatus();

		// Token: 0x06000074 RID: 116
		ISubscription CreateSubscription(SubscriptionState state);

		// Token: 0x06000075 RID: 117
		int QueryAvailableFilters();

		// Token: 0x06000076 RID: 118
		Category[] QueryEventCategories(int eventType);

		// Token: 0x06000077 RID: 119
		string[] QueryConditionNames(int eventCategory);

		// Token: 0x06000078 RID: 120
		string[] QuerySubConditionNames(string conditionName);

		// Token: 0x06000079 RID: 121
		string[] QueryConditionNames(string sourceName);

		// Token: 0x0600007A RID: 122
		Attribute[] QueryEventAttributes(int eventCategory);

		// Token: 0x0600007B RID: 123
		ItemUrl[] TranslateToItemIDs(string sourceName, int eventCategory, string conditionName, string subConditionName, int[] attributeIDs);

		// Token: 0x0600007C RID: 124
		Condition GetConditionState(string sourceName, string conditionName, int[] attributeIDs);

		// Token: 0x0600007D RID: 125
		ResultID[] EnableConditionByArea(string[] areas);

		// Token: 0x0600007E RID: 126
		ResultID[] DisableConditionByArea(string[] areas);

		// Token: 0x0600007F RID: 127
		ResultID[] EnableConditionBySource(string[] sources);

		// Token: 0x06000080 RID: 128
		ResultID[] DisableConditionBySource(string[] sources);

		// Token: 0x06000081 RID: 129
		EnabledStateResult[] GetEnableStateByArea(string[] areas);

		// Token: 0x06000082 RID: 130
		EnabledStateResult[] GetEnableStateBySource(string[] sources);

		// Token: 0x06000083 RID: 131
		ResultID[] AcknowledgeCondition(string acknowledgerID, string comment, EventAcknowledgement[] conditions);

		// Token: 0x06000084 RID: 132
		BrowseElement[] Browse(string areaID, BrowseType browseType, string browseFilter);

		// Token: 0x06000085 RID: 133
		BrowseElement[] Browse(string areaID, BrowseType browseType, string browseFilter, int maxElements, out IBrowsePosition position);

		// Token: 0x06000086 RID: 134
		BrowseElement[] BrowseNext(int maxElements, ref IBrowsePosition position);
	}
}
