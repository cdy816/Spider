using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdy.Spider.CustomDriver.Develop
{
    public class SaveAsFileDialogViewModel:Cdy.Spider.DevelopCommon.WindowViewModelBase
    {

        private string mFileName;

        public SaveAsFileDialogViewModel()
        {
            Title = Res.Get("SaveAsTemplate");
            DefaultHeight = 70;
            DefaultWidth = 450;
        }

        /// <summary>
            /// 
            /// </summary>
        public string FileName
        {
            get
            {
                return mFileName;
            }
            set
            {
                if (mFileName != value)
                {
                    mFileName = value;
                    OnPropertyChanged("FileName");
                }
            }
        }

        protected override bool CanOKCommandProcess()
        {
            return !string.IsNullOrEmpty(mFileName);
        }

    }
}
