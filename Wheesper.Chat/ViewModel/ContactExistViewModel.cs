using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using System.Diagnostics;
using Wheesper.Chat.Model;
using Wheesper.Login.events;

namespace Wheesper.Chat.ViewModel
{
    class ContactExistViewModel : NotificationObject
    {

        #region private menber
        private IUnityContainer container = null;
        private IEventAggregator eventAggregator = null;
        private WheesperModel model = null;
        #endregion private menber

        #region properties
        public string EMail
        {
            get { return email; }
            set
            {
                email = value;
                RaisePropertyChanged("EMail");
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

        private string email { get; set; }
        private string discription { get; set; }
        #endregion properties

        #region Commond
        public DelegateCommand ApplyCommod
        {
            get
            {
                if (applyCommod == null)
                    applyCommod = new DelegateCommand(apply, canApply);
                return applyCommod;
            }
            private set { }
        }
        public DelegateCommand CancelCommod
        {
            get
            {
                if (cancelCommod == null)
                    cancelCommod = new DelegateCommand(cancel, canCancel);
                return cancelCommod;
            }
            private set { }
        }

        private DelegateCommand applyCommod = null;
        private DelegateCommand cancelCommod = null;
        #endregion Commond

        #region Command Delegate Method
        private void apply()
        {
            model.sendContactApplyRequest(model.CurrentUser.EMail, EMail, Discription);
            eventAggregator.GetEvent<CloseUserExistOrNotExistViewEvent>().Publish(true);
        }
        private bool canApply()
        {
            return true;
        }

        private void cancel()
        {
            eventAggregator.GetEvent<CloseUserExistOrNotExistViewEvent>().Publish(true);
        }
        private bool canCancel()
        {
            return true;
        }
        #endregion Command Delegate Method

        #region Constructor & deconstructor
        public ContactExistViewModel(IUnityContainer container)
        {
            Debug.WriteLine("SolveAddContactViewModel constructor");
            this.container = container;
            eventAggregator = this.container.Resolve<IEventAggregator>();
            model = this.container.Resolve<WheesperModel>();

            subevent();
        }

        ~ContactExistViewModel()
        {
            Debug.WriteLine("SolveAddContactViewModel Deconstrutor");
        }
        #endregion Constructor & deconstructor


        #region helper functoin
        private void subevent()
        {
            Debug.WriteLine("SolveAddContactViewModel subscribe event");
        }
        #endregion helper function
    }
}
