using Cdy.Spider.DevelopCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.CalculateDriver.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class ExpressionEditViewModel: WindowViewModelBase
    {

        #region ... Variables  ...
        
        private string mExpresse = "";

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        /// <summary>
        /// 
        /// </summary>
        public string Expresse
        {
            get
            {
                return mExpresse;
            }
            set
            {
                if (mExpresse != value)
                {
                    mExpresse = value;
                    OnPropertyChanged("Expresse");
                }
            }
        }

        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
