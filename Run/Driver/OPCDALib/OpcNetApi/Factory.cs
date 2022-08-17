using System;
using System.Runtime.Serialization;

namespace Opc
{
    // Token: 0x020000D2 RID: 210
    [Serializable]
    public class Factory : IFactory, IDisposable, ISerializable, ICloneable
    {
        // Token: 0x0600070C RID: 1804 RVA: 0x00011789 File Offset: 0x00010789
        public Factory(System.Type systemType, bool useRemoting)
        {
            this.m_systemType = systemType;
            this.m_useRemoting = useRemoting;
        }

        // Token: 0x0600070D RID: 1805 RVA: 0x000117A0 File Offset: 0x000107A0
        ~Factory()
        {
            this.Dispose(false);
        }

        // Token: 0x0600070E RID: 1806 RVA: 0x000117D0 File Offset: 0x000107D0
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Token: 0x0600070F RID: 1807 RVA: 0x000117DF File Offset: 0x000107DF
        protected virtual void Dispose(bool disposing)
        {
            if (!this.m_disposed)
            {
                this.m_disposed = true;
            }
        }

        // Token: 0x06000710 RID: 1808 RVA: 0x000117F2 File Offset: 0x000107F2
        protected Factory(SerializationInfo info, StreamingContext context)
        {
            this.m_useRemoting = info.GetBoolean("UseRemoting");
            this.m_systemType = (System.Type)info.GetValue("SystemType", typeof(Type));
        }

        // Token: 0x06000711 RID: 1809 RVA: 0x0001182B File Offset: 0x0001082B
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("UseRemoting", this.m_useRemoting);
            info.AddValue("SystemType", this.m_systemType);
        }

        // Token: 0x06000712 RID: 1810 RVA: 0x0001184F File Offset: 0x0001084F
        public virtual object Clone()
        {
            return base.MemberwiseClone();
        }

        // Token: 0x06000713 RID: 1811 RVA: 0x00011858 File Offset: 0x00010858
        public virtual IServer CreateInstance(URL url, ConnectData connectData)
        {
            IServer result;
            if (!this.m_useRemoting)
            {
                result = (IServer)Activator.CreateInstance(this.m_systemType, new object[]
                {
                    url,
                    connectData
                });
            }
            else
            {
                //result = (IServer)Activator.GetObject(this.m_systemType, url.ToString());
                result = null;
            }
            return result;
        }

        // Token: 0x170001A9 RID: 425
        // (get) Token: 0x06000714 RID: 1812 RVA: 0x000118AA File Offset: 0x000108AA
        // (set) Token: 0x06000715 RID: 1813 RVA: 0x000118B2 File Offset: 0x000108B2
        protected System.Type SystemType
        {
            get
            {
                return this.m_systemType;
            }
            set
            {
                this.m_systemType = value;
            }
        }

        // Token: 0x170001AA RID: 426
        // (get) Token: 0x06000716 RID: 1814 RVA: 0x000118BB File Offset: 0x000108BB
        // (set) Token: 0x06000717 RID: 1815 RVA: 0x000118C3 File Offset: 0x000108C3
        protected bool UseRemoting
        {
            get
            {
                return this.m_useRemoting;
            }
            set
            {
                this.m_useRemoting = value;
            }
        }

        // Token: 0x04000333 RID: 819
        private bool m_disposed;

        // Token: 0x04000334 RID: 820
        private System.Type m_systemType;

        // Token: 0x04000335 RID: 821
        private bool m_useRemoting;

        // Token: 0x020000D3 RID: 211
        private class Names
        {
            // Token: 0x04000336 RID: 822
            internal const string USE_REMOTING = "UseRemoting";

            // Token: 0x04000337 RID: 823
            internal const string SYSTEM_TYPE = "SystemType";
        }
    }
}
