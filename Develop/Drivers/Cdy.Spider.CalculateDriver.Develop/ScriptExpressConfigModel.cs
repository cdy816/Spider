using Cdy.Spider.DevelopCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Cdy.Spider.CalculateDriver.Develop
{
    public class ScriptExpressConfigModel : ViewModelBase, IRegistorConfigModel
    {
        private ICommand mExpressionEditCommand;
        private string mExpresstion = "";
        /// <summary>
        /// 
        /// </summary>
        public Action<string> UpdateRegistorCallBack { get; set; }
        
        
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
                        
                    });
                }
                return mExpressionEditCommand;
            }
        }

        public IEnumerable<string> Config()
        {
            throw new NotImplementedException();
        }

        public void FreshRegistor(string registor)
        {
            Expresstion = registor;
        }

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
