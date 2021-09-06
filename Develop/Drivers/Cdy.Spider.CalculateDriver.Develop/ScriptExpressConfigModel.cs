using Cdy.Spider.DevelopCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Cdy.Spider.CalculateDriver.Develop
{
    /// <summary>
    /// 
    /// </summary>
    public class ScriptExpressConfigModel : ViewModelBase, IRegistorConfigModel
    {
        private ICommand mExpressionEditCommand;
        private ICommand mClearCommand;
        private string mExpresstion = "";
        /// <summary>
        /// 
        /// </summary>
        public Action<string> UpdateRegistorCallBack { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public IDeviceDevelopService Service { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Expresstion
        {
            get
            {
                return mExpresstion;
            }
            set
            {
                if (mExpresstion != value)
                {
                    mExpresstion = value;
                    OnPropertyChanged("Expresstion");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand ExpressionEditCommand
        {
            get
            {
                if(mExpressionEditCommand==null)
                {
                    mExpressionEditCommand = new RelayCommand(() => {
                        ExpressionEditViewModel mm = new ExpressionEditViewModel();
                        mm.Expresse = this.Expresstion;
                        if(mm.ShowDialog().Value)
                        {
                            Expresstion = mm.GetExpressResult();
                            UpdateRegistor();
                        }
                    });
                }
                return mExpressionEditCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand ClearCommand
        {
            get
            {
                if(mClearCommand==null)
                {
                    mClearCommand = new RelayCommand(() => {
                        Expresstion = "";
                        UpdateRegistor();
                    });
                }
                return mClearCommand;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> Config()
        {
            return new List<string>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="registor"></param>
        public void FreshRegistor(string registor)
        {
            Expresstion = registor;
        }

        /// <summary>
        /// 
        /// </summary>
        public void OnDisActived()
        {
            UpdateRegistor();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void UpdateRegistor()
        {
            UpdateRegistorCallBack?.Invoke(Expresstion);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            UpdateRegistorCallBack = null;
            base.Dispose();
        }
    }
}
