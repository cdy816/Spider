using System;

namespace Opc.Da
{
    [Serializable]
    public class Item : ItemIdentifier
    {
        private System.Type m_reqType;

        private int m_maxAge;

        private bool m_maxAgeSpecified;

        private bool m_active = true;

        private bool m_activeSpecified;

        private float m_deadband;

        private bool m_deadbandSpecified;

        private int m_samplingRate;

        private bool m_samplingRateSpecified;

        private bool m_enableBuffering;

        private bool m_enableBufferingSpecified;

        public System.Type ReqType
        {
            get
            {
                return m_reqType;
            }
            set
            {
                m_reqType = value;
            }
        }

        public int MaxAge
        {
            get
            {
                return m_maxAge;
            }
            set
            {
                m_maxAge = value;
            }
        }

        public bool MaxAgeSpecified
        {
            get
            {
                return m_maxAgeSpecified;
            }
            set
            {
                m_maxAgeSpecified = value;
            }
        }

        public bool Active
        {
            get
            {
                return m_active;
            }
            set
            {
                m_active = value;
            }
        }

        public bool ActiveSpecified
        {
            get
            {
                return m_activeSpecified;
            }
            set
            {
                m_activeSpecified = value;
            }
        }

        public float Deadband
        {
            get
            {
                return m_deadband;
            }
            set
            {
                m_deadband = value;
            }
        }

        public bool DeadbandSpecified
        {
            get
            {
                return m_deadbandSpecified;
            }
            set
            {
                m_deadbandSpecified = value;
            }
        }

        public int SamplingRate
        {
            get
            {
                return m_samplingRate;
            }
            set
            {
                m_samplingRate = value;
            }
        }

        public bool SamplingRateSpecified
        {
            get
            {
                return m_samplingRateSpecified;
            }
            set
            {
                m_samplingRateSpecified = value;
            }
        }

        public bool EnableBuffering
        {
            get
            {
                return m_enableBuffering;
            }
            set
            {
                m_enableBuffering = value;
            }
        }

        public bool EnableBufferingSpecified
        {
            get
            {
                return m_enableBufferingSpecified;
            }
            set
            {
                m_enableBufferingSpecified = value;
            }
        }

        public Item()
        {
        }

        public Item(ItemIdentifier item)
        {
            if (item != null)
            {
                base.ItemName = item.ItemName;
                base.ItemPath = item.ItemPath;
                base.ClientHandle = item.ClientHandle;
                base.ServerHandle = item.ServerHandle;
            }
        }

        public Item(Item item)
            : base(item)
        {
            if (item != null)
            {
                ReqType = item.ReqType;
                MaxAge = item.MaxAge;
                MaxAgeSpecified = item.MaxAgeSpecified;
                Active = item.Active;
                ActiveSpecified = item.ActiveSpecified;
                Deadband = item.Deadband;
                DeadbandSpecified = item.DeadbandSpecified;
                SamplingRate = item.SamplingRate;
                SamplingRateSpecified = item.SamplingRateSpecified;
                EnableBuffering = item.EnableBuffering;
                EnableBufferingSpecified = item.EnableBufferingSpecified;
            }
        }
    }
}
