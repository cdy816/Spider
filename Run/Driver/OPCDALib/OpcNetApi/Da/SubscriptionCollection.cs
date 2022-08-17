using System;
using System.Collections;

namespace Opc.Da
{
    [Serializable]
    public class SubscriptionCollection : ICloneable, IList, ICollection, IEnumerable
    {
        private ArrayList m_subscriptions = new ArrayList();

        public Subscription this[int index]
        {
            get
            {
                return (Subscription)m_subscriptions[index];
            }
            set
            {
                m_subscriptions[index] = value;
            }
        }

        public bool IsSynchronized => false;

        public int Count
        {
            get
            {
                if (m_subscriptions == null)
                {
                    return 0;
                }

                return m_subscriptions.Count;
            }
        }

        public object SyncRoot => this;

        public bool IsReadOnly => false;

        object IList.this[int index]
        {
            get
            {
                return m_subscriptions[index];
            }
            set
            {
                if (!typeof(Subscription).IsInstanceOfType(value))
                {
                    throw new ArgumentException("May only add Subscription objects into the collection.");
                }

                m_subscriptions[index] = value;
            }
        }

        public bool IsFixedSize => false;

        public SubscriptionCollection()
        {
        }

        public SubscriptionCollection(SubscriptionCollection subscriptions)
        {
            if (subscriptions == null)
            {
                return;
            }

            foreach (Subscription subscription in subscriptions)
            {
                Add(subscription);
            }
        }

        public virtual object Clone()
        {
            SubscriptionCollection subscriptionCollection = (SubscriptionCollection)MemberwiseClone();
            subscriptionCollection.m_subscriptions = new ArrayList();
            foreach (Subscription subscription in m_subscriptions)
            {
                subscriptionCollection.m_subscriptions.Add(subscription.Clone());
            }

            return subscriptionCollection;
        }

        public void CopyTo(Array array, int index)
        {
            if (m_subscriptions != null)
            {
                m_subscriptions.CopyTo(array, index);
            }
        }

        public void CopyTo(Subscription[] array, int index)
        {
            CopyTo((Array)array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return m_subscriptions.GetEnumerator();
        }

        public void RemoveAt(int index)
        {
            m_subscriptions.RemoveAt(index);
        }

        public void Insert(int index, object value)
        {
            if (!typeof(Subscription).IsInstanceOfType(value))
            {
                throw new ArgumentException("May only add Subscription objects into the collection.");
            }

            m_subscriptions.Insert(index, value);
        }

        public void Remove(object value)
        {
            m_subscriptions.Remove(value);
        }

        public bool Contains(object value)
        {
            return m_subscriptions.Contains(value);
        }

        public void Clear()
        {
            m_subscriptions.Clear();
        }

        public int IndexOf(object value)
        {
            return m_subscriptions.IndexOf(value);
        }

        public int Add(object value)
        {
            if (!typeof(Subscription).IsInstanceOfType(value))
            {
                throw new ArgumentException("May only add Subscription objects into the collection.");
            }

            return m_subscriptions.Add(value);
        }

        public void Insert(int index, Subscription value)
        {
            Insert(index, (object)value);
        }

        public void Remove(Subscription value)
        {
            Remove((object)value);
        }

        public bool Contains(Subscription value)
        {
            return Contains((object)value);
        }

        public int IndexOf(Subscription value)
        {
            return IndexOf((object)value);
        }

        public int Add(Subscription value)
        {
            return Add((object)value);
        }
    }
}
