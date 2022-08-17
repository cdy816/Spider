using Opc.Ae;
using System;
using System.Runtime.InteropServices.ComTypes;

namespace OpcCom.Ae
{
    [Serializable]
    public class BrowsePosition : Opc.Ae.BrowsePosition
    {
        private bool m_disposed;

        private IEnumString m_enumerator;

        public IEnumString Enumerator => m_enumerator;

        public BrowsePosition(string areaID, BrowseType browseType, string browseFilter, IEnumString enumerator)
            : base(areaID, browseType, browseFilter)
        {
            m_enumerator = enumerator;
        }

        protected override void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                if (m_enumerator != null)
                {
                    OpcCom.Interop.ReleaseServer(m_enumerator);
                    m_enumerator = null;
                }

                m_disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
