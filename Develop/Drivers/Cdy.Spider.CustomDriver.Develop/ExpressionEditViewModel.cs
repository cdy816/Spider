using Cdy.Spider.CalculateExpressEditor;
using Cdy.Spider.DevelopCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.CustomDriver.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class ExpressionEditViewModel: WindowViewModelBase
    {

        #region ... Variables  ...
        
        private string mExpresse = "";

        private RoslynCodeEditor mExpressEditor;

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...
        /// <summary>
        /// 
        /// </summary>
        public ExpressionEditViewModel()
        {
            Title = Res.Get("Expresse");
            DefaultWidth = 800;
            DefaultHeight = 600;
        }
        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public RoslynCodeEditor ExpressEditor
        {
            get
            {
                return mExpressEditor;
            }
            set
            {
                if (mExpressEditor != value)
                {
                    mExpressEditor = value;
                    //mExpressEditor.Text = mExpresse;
                    OnPropertyChanged("ExpressEditor");
                }
            }
        }


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
                    //if(mExpressEditor!=null)
                    //mExpressEditor.Text = value;
                    OnPropertyChanged("Expresse");
                }
            }
        }

        #endregion ...Properties...

        #region ... Methods    ...



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetExpressResult()
        {
            return mExpressEditor.Text;
        }

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...
    }
}
