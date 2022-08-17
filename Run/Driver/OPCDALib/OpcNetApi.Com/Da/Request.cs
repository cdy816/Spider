using System;
using Opc;
using Opc.Da;

namespace OpcCom.Da
{
    [Serializable]
    public class Request : Opc.Da.Request
    {
        internal int RequestID;

        internal int CancelID;

        internal Delegate Callback;

        internal int Filters;

        internal ItemIdentifier[] InitialResults;

        public Request(ISubscription subscription, object clientHandle, int filters, int requestID, Delegate callback)
            : base(subscription, clientHandle)
        {
            Filters = filters;
            RequestID = requestID;
            Callback = callback;
            CancelID = 0;
            InitialResults = null;
        }

        public bool BeginRead(int cancelID, IdentifiedResult[] results)
        {
            CancelID = cancelID;
            ItemValueResult[] array = null;
            if (InitialResults != null && (object)InitialResults.GetType() == typeof(ItemValueResult[]))
            {
                array = (ItemValueResult[])InitialResults;
                InitialResults = results;
                EndRequest(array);
                return true;
            }

            foreach (IdentifiedResult identifiedResult in results)
            {
                if (identifiedResult.ResultID.Succeeded())
                {
                    InitialResults = results;
                    return false;
                }
            }

            return true;
        }

        public bool BeginWrite(int cancelID, IdentifiedResult[] results)
        {
            CancelID = cancelID;
            if (InitialResults != null && (object)InitialResults.GetType() == typeof(IdentifiedResult[]))
            {
                IdentifiedResult[] callbackResults = (IdentifiedResult[])InitialResults;
                InitialResults = results;
                EndRequest(callbackResults);
                return true;
            }

            foreach (IdentifiedResult identifiedResult in results)
            {
                if (identifiedResult.ResultID.Succeeded())
                {
                    InitialResults = results;
                    return false;
                }
            }

            for (int j = 0; j < results.Length; j++)
            {
                if ((Filters & 1) == 0)
                {
                    results[j].ItemName = null;
                }

                if ((Filters & 2) == 0)
                {
                    results[j].ItemPath = null;
                }

                if ((Filters & 4) == 0)
                {
                    results[j].ClientHandle = null;
                }
            }

            ((WriteCompleteEventHandler)Callback)(base.Handle, results);
            return true;
        }

        public bool BeginRefresh(int cancelID)
        {
            CancelID = cancelID;
            return false;
        }

        public void EndRequest()
        {
            if (typeof(CancelCompleteEventHandler).IsInstanceOfType(Callback))
            {
                ((CancelCompleteEventHandler)Callback)(base.Handle);
            }
        }

        public void EndRequest(ItemValueResult[] results)
        {
            if (InitialResults == null)
            {
                InitialResults = results;
                return;
            }

            if (typeof(CancelCompleteEventHandler).IsInstanceOfType(Callback))
            {
                ((CancelCompleteEventHandler)Callback)(base.Handle);
                return;
            }

            for (int i = 0; i < results.Length; i++)
            {
                if ((Filters & 1) == 0)
                {
                    results[i].ItemName = null;
                }

                if ((Filters & 2) == 0)
                {
                    results[i].ItemPath = null;
                }

                if ((Filters & 4) == 0)
                {
                    results[i].ClientHandle = null;
                }

                if ((Filters & 8) == 0)
                {
                    results[i].Timestamp = DateTime.MinValue;
                    results[i].TimestampSpecified = false;
                }
            }

            if (typeof(ReadCompleteEventHandler).IsInstanceOfType(Callback))
            {
                ((ReadCompleteEventHandler)Callback)(base.Handle, results);
            }
        }

        public void EndRequest(IdentifiedResult[] callbackResults)
        {
            if (InitialResults == null)
            {
                InitialResults = callbackResults;
                return;
            }

            if ((object)Callback != null && (object)Callback.GetType() == typeof(CancelCompleteEventHandler))
            {
                ((CancelCompleteEventHandler)Callback)(base.Handle);
                return;
            }

            IdentifiedResult[] array = (IdentifiedResult[])InitialResults;
            int i = 0;
            for (int j = 0; j < array.Length; j++)
            {
                for (; i < callbackResults.Length; i++)
                {
                    if (callbackResults[j].ServerHandle.Equals(array[i].ServerHandle))
                    {
                        array[i++] = callbackResults[j];
                        break;
                    }
                }
            }

            for (int k = 0; k < array.Length; k++)
            {
                if ((Filters & 1) == 0)
                {
                    array[k].ItemName = null;
                }

                if ((Filters & 2) == 0)
                {
                    array[k].ItemPath = null;
                }

                if ((Filters & 4) == 0)
                {
                    array[k].ClientHandle = null;
                }
            }

            if ((object)Callback != null && (object)Callback.GetType() == typeof(WriteCompleteEventHandler))
            {
                ((WriteCompleteEventHandler)Callback)(base.Handle, array);
            }
        }
    }
}
