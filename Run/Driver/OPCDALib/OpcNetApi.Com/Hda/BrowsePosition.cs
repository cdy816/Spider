using System;
using Opc.Hda;

namespace OpcCom.Hda
{
    internal class BrowsePosition : Opc.Hda.BrowsePosition
    {
        private bool m_disposed;

        private string m_branchPath;

        private EnumString m_enumerator;

        private bool m_fetchingItems;

        internal string BranchPath
        {
            get
            {
                return m_branchPath;
            }
            set
            {
                m_branchPath = value;
            }
        }

        internal EnumString Enumerator
        {
            get
            {
                return m_enumerator;
            }
            set
            {
                m_enumerator = value;
            }
        }

        internal bool FetchingItems
        {
            get
            {
                return m_fetchingItems;
            }
            set
            {
                m_fetchingItems = value;
            }
        }

        internal BrowsePosition(string branchPath, EnumString enumerator, bool fetchingItems)
        {
            m_branchPath = branchPath;
            m_enumerator = enumerator;
            m_fetchingItems = fetchingItems;
        }

        protected override void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                if (disposing && m_enumerator != null)
                {
                    m_enumerator.Dispose();
                    m_enumerator = null;
                }

                m_disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
