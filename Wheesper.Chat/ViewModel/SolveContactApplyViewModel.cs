using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using System.Diagnostics;
using Wheesper.Chat.Model;
using Wheesper.Login.events;

namespace Wheesper.Chat.ViewModel
{
    public class SolveContactApplyViewModel : NotificationObject
    {
        #region private menber
        private IUnityContainer container = null;
        private IEventAggregator eventAggregator = null;
        private WheesperModel model = null;
        #endregion private menber

        #region properties
        public string ApplierEMail
        {
            get { return applierEMail; }
            set
            {
                applierEMail = value;
                RaisePropertyChanged("ApplierEMail");
            }
        }
        public string TargetEMail
        {
            get { return targetEMail; }
            set
            {
                targetEMail = value;
                RaisePropertyChanged("TargetEMail");
            }
        }
        public string Discription
        {
            get { return discription; }
            set
            {
                discription = value;
                RaisePropertyChanged("Discription");
            }
        }

        private string applierEMail { get; set; }
        private string targetEMail { get; set; }
        private string discription { get; set; }
        #endregion properties

        #region Commond
        public DelegateCommand AcceptContactApplyCommod
        {
            get
            {
                if (acceptContactApplyCommod == null)
                    acceptContactApplyCommod = new DelegateCommand(acceptContactApply, canAcceptContactApply);
                return acceptContactApplyCommod;
            }
            private set { }
        }
        public DelegateCommand RejectContactApplyCommod
        {
            get
            {
                if (rejectContactApplyCommod == null)
                    rejectContactApplyCommod = new DelegateCommand(rejectContactApply, canRejectContactApply);
                return rejectContactApplyCommod;
            }
            private set { }
        }
        public DelegateCommand CloseSolveContactApplyCommond
        {
            get
            {
                if (closeSolveContactApplyCommond == null)
                    closeSolveContactApplyCommond = new DelegateCommand(closeSolveContactApply, canCloseSolveContactApply);
                return closeSolveContactApplyCommond;
            }
            private set { }
        }

        private DelegateCommand acceptContactApplyCommod = null;
        private DelegateCommand rejectContactApplyCommod = null;
        private DelegateCommand closeSolveContactApplyCommond = null;
        #endregion Commond

        #region Command Delegate Method
        private void acceptContactApply()
        {
            model.sendContactReplyRequest(ApplierEMail, TargetEMail, true, Discription);
            eventAggregator.GetEvent<CloseUserExistOrNotExistViewEvent>().Publish(true);
        }
        private bool canAcceptContactApply()
        {
            return true;
        }

        private void rejectContactApply()
        {
            model.sendContactReplyRequest(ApplierEMail, TargetEMail, false, Discription);
            eventAggregator.GetEvent<CloseUserExistOrNotExistViewEvent>().Publish(true);
        }
        private bool canRejectContactApply()
        {
            return true;
        }

        private void closeSolveContactApply()
        {
            eventAggregator.GetEvent<ShowSystemMessageViewEvent>().Publish(0);
        }
        private bool canCloseSolveContactApply()
        {
            return true;
        }
        #endregion Command Delegate Method

        #region Constructor & deconstructor
        public SolveContactApplyViewModel(IUnityContainer container)
        {
            Debug.WriteLine("SolveContactApplyViewModel constructor");
            this.container = container;
            eventAggregator = this.container.Resolve<IEventAggregator>();
            model = this.container.Resolve<WheesperModel>();

            subevent();
        }

        ~SolveContactApplyViewModel()
        {
            Debug.WriteLine("SolveContactApplyViewModel Deconstrutor");
        }
        #endregion Constructor & deconstructor


        #region helper functoin
        private void subevent()
        {
            Debug.WriteLine("SolveContactApplyViewModel subscribe event");
        }
        #endregion helper function
    }
}
