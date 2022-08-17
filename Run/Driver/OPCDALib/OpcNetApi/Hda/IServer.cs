using System;

namespace Opc.Hda
{
	// Token: 0x02000096 RID: 150
	public interface IServer : Opc.IServer, IDisposable
	{
		// Token: 0x06000438 RID: 1080
		ServerStatus GetStatus();

		// Token: 0x06000439 RID: 1081
		Attribute[] GetAttributes();

		// Token: 0x0600043A RID: 1082
		Aggregate[] GetAggregates();

		// Token: 0x0600043B RID: 1083
		IBrowser CreateBrowser(BrowseFilter[] filters, out ResultID[] results);

		// Token: 0x0600043C RID: 1084
		IdentifiedResult[] CreateItems(ItemIdentifier[] items);

		// Token: 0x0600043D RID: 1085
		IdentifiedResult[] ReleaseItems(ItemIdentifier[] items);

		// Token: 0x0600043E RID: 1086
		IdentifiedResult[] ValidateItems(ItemIdentifier[] items);

		// Token: 0x0600043F RID: 1087
		ItemValueCollection[] ReadRaw(Time startTime, Time endTime, int maxValues, bool includeBounds, ItemIdentifier[] items);

		// Token: 0x06000440 RID: 1088
		IdentifiedResult[] ReadRaw(Time startTime, Time endTime, int maxValues, bool includeBounds, ItemIdentifier[] items, object requestHandle, ReadValuesEventHandler callback, out IRequest request);

		// Token: 0x06000441 RID: 1089
		IdentifiedResult[] AdviseRaw(Time startTime, decimal updateInterval, ItemIdentifier[] items, object requestHandle, DataUpdateEventHandler callback, out IRequest request);

		// Token: 0x06000442 RID: 1090
		IdentifiedResult[] PlaybackRaw(Time startTime, Time endTime, int maxValues, decimal updateInterval, decimal playbackDuration, ItemIdentifier[] items, object requestHandle, DataUpdateEventHandler callback, out IRequest request);

		// Token: 0x06000443 RID: 1091
		ItemValueCollection[] ReadProcessed(Time startTime, Time endTime, decimal resampleInterval, Item[] items);

		// Token: 0x06000444 RID: 1092
		IdentifiedResult[] ReadProcessed(Time startTime, Time endTime, decimal resampleInterval, Item[] items, object requestHandle, ReadValuesEventHandler callback, out IRequest request);

		// Token: 0x06000445 RID: 1093
		IdentifiedResult[] AdviseProcessed(Time startTime, decimal resampleInterval, int numberOfIntervals, Item[] items, object requestHandle, DataUpdateEventHandler callback, out IRequest request);

		// Token: 0x06000446 RID: 1094
		IdentifiedResult[] PlaybackProcessed(Time startTime, Time endTime, decimal resampleInterval, int numberOfIntervals, decimal updateInterval, Item[] items, object requestHandle, DataUpdateEventHandler callback, out IRequest request);

		// Token: 0x06000447 RID: 1095
		ItemValueCollection[] ReadAtTime(DateTime[] timestamps, ItemIdentifier[] items);

		// Token: 0x06000448 RID: 1096
		IdentifiedResult[] ReadAtTime(DateTime[] timestamps, ItemIdentifier[] items, object requestHandle, ReadValuesEventHandler callback, out IRequest request);

		// Token: 0x06000449 RID: 1097
		ModifiedValueCollection[] ReadModified(Time startTime, Time endTime, int maxValues, ItemIdentifier[] items);

		// Token: 0x0600044A RID: 1098
		IdentifiedResult[] ReadModified(Time startTime, Time endTime, int maxValues, ItemIdentifier[] items, object requestHandle, ReadValuesEventHandler callback, out IRequest request);

		// Token: 0x0600044B RID: 1099
		ItemAttributeCollection ReadAttributes(Time startTime, Time endTime, ItemIdentifier item, int[] attributeIDs);

		// Token: 0x0600044C RID: 1100
		ResultCollection ReadAttributes(Time startTime, Time endTime, ItemIdentifier item, int[] attributeIDs, object requestHandle, ReadAttributesEventHandler callback, out IRequest request);

		// Token: 0x0600044D RID: 1101
		AnnotationValueCollection[] ReadAnnotations(Time startTime, Time endTime, ItemIdentifier[] items);

		// Token: 0x0600044E RID: 1102
		IdentifiedResult[] ReadAnnotations(Time startTime, Time endTime, ItemIdentifier[] items, object requestHandle, ReadAnnotationsEventHandler callback, out IRequest request);

		// Token: 0x0600044F RID: 1103
		ResultCollection[] InsertAnnotations(AnnotationValueCollection[] items);

		// Token: 0x06000450 RID: 1104
		IdentifiedResult[] InsertAnnotations(AnnotationValueCollection[] items, object requestHandle, UpdateCompleteEventHandler callback, out IRequest request);

		// Token: 0x06000451 RID: 1105
		ResultCollection[] Insert(ItemValueCollection[] items, bool replace);

		// Token: 0x06000452 RID: 1106
		IdentifiedResult[] Insert(ItemValueCollection[] items, bool replace, object requestHandle, UpdateCompleteEventHandler callback, out IRequest request);

		// Token: 0x06000453 RID: 1107
		ResultCollection[] Replace(ItemValueCollection[] items);

		// Token: 0x06000454 RID: 1108
		IdentifiedResult[] Replace(ItemValueCollection[] items, object requestHandle, UpdateCompleteEventHandler callback, out IRequest request);

		// Token: 0x06000455 RID: 1109
		IdentifiedResult[] Delete(Time startTime, Time endTime, ItemIdentifier[] items);

		// Token: 0x06000456 RID: 1110
		IdentifiedResult[] Delete(Time startTime, Time endTime, ItemIdentifier[] items, object requestHandle, UpdateCompleteEventHandler callback, out IRequest request);

		// Token: 0x06000457 RID: 1111
		ResultCollection[] DeleteAtTime(ItemTimeCollection[] items);

		// Token: 0x06000458 RID: 1112
		IdentifiedResult[] DeleteAtTime(ItemTimeCollection[] items, object requestHandle, UpdateCompleteEventHandler callback, out IRequest request);

		// Token: 0x06000459 RID: 1113
		void CancelRequest(IRequest request);

		// Token: 0x0600045A RID: 1114
		void CancelRequest(IRequest request, CancelCompleteEventHandler callback);
	}
}
