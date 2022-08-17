using System;
using System.Collections;
using Opc.Hda;
using OpcRcw.Hda;

namespace OpcCom.Hda
{
    internal class DataCallback : IOPCHDA_DataCallback
    {
        private int m_nextID;

        private Hashtable m_requests = new Hashtable();

        private CallbackExceptionEventHandler m_callbackException;

        public event CallbackExceptionEventHandler CallbackException
        {
            add
            {
                lock (this)
                {
                    m_callbackException = (CallbackExceptionEventHandler)Delegate.Combine(m_callbackException, value);
                }
            }
            remove
            {
                lock (this)
                {
                    m_callbackException = (CallbackExceptionEventHandler)Delegate.Remove(m_callbackException, value);
                }
            }
        }

        public Request CreateRequest(object requestHandle, Delegate callback)
        {
            lock (this)
            {
                Request request = new Request(requestHandle, callback, ++m_nextID);
                m_requests[request.RequestID] = request;
                return request;
            }
        }

        public bool CancelRequest(Request request, CancelCompleteEventHandler callback)
        {
            lock (this)
            {
                if (!m_requests.Contains(request.RequestID))
                {
                    return false;
                }

                if (callback != null)
                {
                    request.CancelComplete += callback;
                }
                else
                {
                    m_requests.Remove(request.RequestID);
                }

                return true;
            }
        }

        public void OnDataChange(int dwTransactionID, int hrStatus, int dwNumItems, OPCHDA_ITEM[] pItemValues, int[] phrErrors)
        {
            try
            {
                lock (this)
                {
                    Request request = (Request)m_requests[dwTransactionID];
                    if (request != null)
                    {
                        ItemValueCollection[] array = new ItemValueCollection[pItemValues.Length];
                        for (int i = 0; i < pItemValues.Length; i++)
                        {
                            array[i] = Interop.GetItemValueCollection(pItemValues[i], deallocate: false);
                            array[i].ServerHandle = array[i].ClientHandle;
                            array[i].ClientHandle = null;
                            array[i].ResultID = OpcCom.Interop.GetResultID(phrErrors[i]);
                        }

                        if (request.InvokeCallback(array))
                        {
                            m_requests.Remove(request.RequestID);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                HandleException(dwTransactionID, exception);
            }
        }

        public void OnReadComplete(int dwTransactionID, int hrStatus, int dwNumItems, OPCHDA_ITEM[] pItemValues, int[] phrErrors)
        {
            try
            {
                lock (this)
                {
                    Request request = (Request)m_requests[dwTransactionID];
                    if (request != null)
                    {
                        ItemValueCollection[] array = new ItemValueCollection[pItemValues.Length];
                        for (int i = 0; i < pItemValues.Length; i++)
                        {
                            array[i] = Interop.GetItemValueCollection(pItemValues[i], deallocate: false);
                            array[i].ServerHandle = pItemValues[i].hClient;
                            array[i].ResultID = OpcCom.Interop.GetResultID(phrErrors[i]);
                        }

                        if (request.InvokeCallback(array))
                        {
                            m_requests.Remove(request.RequestID);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                HandleException(dwTransactionID, exception);
            }
        }

        public void OnReadModifiedComplete(int dwTransactionID, int hrStatus, int dwNumItems, OPCHDA_MODIFIEDITEM[] pItemValues, int[] phrErrors)
        {
            try
            {
                lock (this)
                {
                    Request request = (Request)m_requests[dwTransactionID];
                    if (request != null)
                    {
                        ModifiedValueCollection[] array = new ModifiedValueCollection[pItemValues.Length];
                        for (int i = 0; i < pItemValues.Length; i++)
                        {
                            array[i] = Interop.GetModifiedValueCollection(pItemValues[i], deallocate: false);
                            array[i].ServerHandle = pItemValues[i].hClient;
                            array[i].ResultID = OpcCom.Interop.GetResultID(phrErrors[i]);
                        }

                        if (request.InvokeCallback(array))
                        {
                            m_requests.Remove(request.RequestID);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                HandleException(dwTransactionID, exception);
            }
        }

        public void OnReadAttributeComplete(int dwTransactionID, int hrStatus, int hClient, int dwNumItems, OPCHDA_ATTRIBUTE[] pAttributeValues, int[] phrErrors)
        {
            try
            {
                lock (this)
                {
                    Request request = (Request)m_requests[dwTransactionID];
                    if (request != null)
                    {
                        ItemAttributeCollection itemAttributeCollection = new ItemAttributeCollection();
                        itemAttributeCollection.ServerHandle = hClient;
                        AttributeValueCollection[] array = new AttributeValueCollection[pAttributeValues.Length];
                        for (int i = 0; i < pAttributeValues.Length; i++)
                        {
                            array[i] = Interop.GetAttributeValueCollection(pAttributeValues[i], deallocate: false);
                            array[i].ResultID = OpcCom.Interop.GetResultID(phrErrors[i]);
                            itemAttributeCollection.Add(array[i]);
                        }

                        if (request.InvokeCallback(itemAttributeCollection))
                        {
                            m_requests.Remove(request.RequestID);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                HandleException(dwTransactionID, exception);
            }
        }

        public void OnReadAnnotations(int dwTransactionID, int hrStatus, int dwNumItems, OPCHDA_ANNOTATION[] pAnnotationValues, int[] phrErrors)
        {
            try
            {
                lock (this)
                {
                    Request request = (Request)m_requests[dwTransactionID];
                    if (request != null)
                    {
                        AnnotationValueCollection[] array = new AnnotationValueCollection[pAnnotationValues.Length];
                        for (int i = 0; i < pAnnotationValues.Length; i++)
                        {
                            array[i] = Interop.GetAnnotationValueCollection(pAnnotationValues[i], deallocate: false);
                            array[i].ServerHandle = pAnnotationValues[i].hClient;
                            array[i].ResultID = OpcCom.Interop.GetResultID(phrErrors[i]);
                        }

                        if (request.InvokeCallback(array))
                        {
                            m_requests.Remove(request.RequestID);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                HandleException(dwTransactionID, exception);
            }
        }

        public void OnInsertAnnotations(int dwTransactionID, int hrStatus, int dwCount, int[] phClients, int[] phrErrors)
        {
            try
            {
                lock (this)
                {
                    Request request = (Request)m_requests[dwTransactionID];
                    if (request == null)
                    {
                        return;
                    }

                    ArrayList arrayList = new ArrayList();
                    if (dwCount > 0)
                    {
                        int num = phClients[0];
                        ResultCollection resultCollection = new ResultCollection();
                        for (int i = 0; i < dwCount; i++)
                        {
                            if (phClients[i] != num)
                            {
                                resultCollection.ServerHandle = num;
                                arrayList.Add(resultCollection);
                                num = phClients[i];
                                resultCollection = new ResultCollection();
                            }

                            Result value = new Result(OpcCom.Interop.GetResultID(phrErrors[i]));
                            resultCollection.Add(value);
                        }

                        resultCollection.ServerHandle = num;
                        arrayList.Add(resultCollection);
                    }

                    if (request.InvokeCallback((ResultCollection[])arrayList.ToArray(typeof(ResultCollection))))
                    {
                        m_requests.Remove(request.RequestID);
                    }
                }
            }
            catch (Exception exception)
            {
                HandleException(dwTransactionID, exception);
            }
        }

        public void OnPlayback(int dwTransactionID, int hrStatus, int dwNumItems, IntPtr ppItemValues, int[] phrErrors)
        {
            try
            {
                lock (this)
                {
                    Request request = (Request)m_requests[dwTransactionID];
                    if (request == null)
                    {
                        return;
                    }

                    ItemValueCollection[] array = new ItemValueCollection[dwNumItems];
                    int[] int32s = OpcCom.Interop.GetInt32s(ref ppItemValues, dwNumItems, deallocate: false);
                    for (int i = 0; i < dwNumItems; i++)
                    {
                        IntPtr pInput = (IntPtr)int32s[i];
                        ItemValueCollection[] itemValueCollections = Interop.GetItemValueCollections(ref pInput, 1, deallocate: false);
                        if (itemValueCollections != null && itemValueCollections.Length == 1)
                        {
                            array[i] = itemValueCollections[0];
                            array[i].ServerHandle = array[i].ClientHandle;
                            array[i].ClientHandle = null;
                            array[i].ResultID = OpcCom.Interop.GetResultID(phrErrors[i]);
                        }
                    }

                    if (request.InvokeCallback(array))
                    {
                        m_requests.Remove(request.RequestID);
                    }
                }
            }
            catch (Exception exception)
            {
                HandleException(dwTransactionID, exception);
            }
        }

        public void OnUpdateComplete(int dwTransactionID, int hrStatus, int dwCount, int[] phClients, int[] phrErrors)
        {
            try
            {
                lock (this)
                {
                    Request request = (Request)m_requests[dwTransactionID];
                    if (request == null)
                    {
                        return;
                    }

                    ArrayList arrayList = new ArrayList();
                    if (dwCount > 0)
                    {
                        int num = phClients[0];
                        ResultCollection resultCollection = new ResultCollection();
                        for (int i = 0; i < dwCount; i++)
                        {
                            if (phClients[i] != num)
                            {
                                resultCollection.ServerHandle = num;
                                arrayList.Add(resultCollection);
                                num = phClients[i];
                                resultCollection = new ResultCollection();
                            }

                            Result value = new Result(OpcCom.Interop.GetResultID(phrErrors[i]));
                            resultCollection.Add(value);
                        }

                        resultCollection.ServerHandle = num;
                        arrayList.Add(resultCollection);
                    }

                    if (request.InvokeCallback((ResultCollection[])arrayList.ToArray(typeof(ResultCollection))))
                    {
                        m_requests.Remove(request.RequestID);
                    }
                }
            }
            catch (Exception exception)
            {
                HandleException(dwTransactionID, exception);
            }
        }

        public void OnCancelComplete(int dwCancelID)
        {
            try
            {
                lock (this)
                {
                    Request request = (Request)m_requests[dwCancelID];
                    if (request != null)
                    {
                        request.OnCancelComplete();
                        m_requests.Remove(request.RequestID);
                    }
                }
            }
            catch (Exception exception)
            {
                HandleException(dwCancelID, exception);
            }
        }

        private void HandleException(int requestID, Exception exception)
        {
            lock (this)
            {
                Request request = (Request)m_requests[requestID];
                if (request != null && m_callbackException != null)
                {
                    m_callbackException(request, exception);
                }
            }
        }
    }
}
