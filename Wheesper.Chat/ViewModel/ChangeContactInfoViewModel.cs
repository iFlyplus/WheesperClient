using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using System.Diagnostics;
using Wheesper.Chat.Model;
using Wheesper.Login.events;

namespace Wheesper.Chat.ViewModel
{
    public class ChangeContactInfoViewModel : NotificationObject
    {
        #region private menber
        private IUnityContainer container = null;
        private IEventAggregator eventAggregator = null;
        private WheesperModel model = null;
        #endregion private menber

        #region properties
        public string ContactEMail
        {
            get { return contactEMail; }
            set
            {
                contactEMail = value;
                Debug.WriteLine(contactEMail);
                RaisePropertyChanged("ContactEMail");
            }
        }
        public string Remarks
        {
            get { return remarks; }
            set
            {
                remarks = value;
                Debug.WriteLine(remarks);
                RaisePropertyChanged("Remarks");
            }
        }
        public string Group
        {
            get { return group; }
            set
            {
                group = value;
                Debug.WriteLine(group);
                RaisePropertyChanged("Group");
            }
        }
        
        private string contactEMail = null;
        private string remarks = null;
        private string group = null;
        #endregion properties

        #region Commond
        public DelegateCommand ApplyCommand
        {
            get
            {
                if (applyCommond == null)
                {
                    applyCommond = new DelegateCommand(apply, canApply);
                }
                return applyCommond;
            }
            private set { }
        }
        public DelegateCommand CancelCommand
        {
            get
            {
                if (cancelCommand == null)
                {
                    cancelCommand = new DelegateCommand(cancel, canCancel);
                }
                return cancelCommand;
            }
            private set { }
        }

        private DelegateCommand applyCommond = null;
        private DelegateCommand cancelCommand = null;
        #endregion Commond

        #region Command Delegate Method
        private void apply()
        {
            model.sendContactRemarkModifyRequest(ContactEMail, Remarks, Group);
        }
        private bool canApply()
        {
            return !string.IsNullOrWhiteSpace(Group);
        }

        private void cancel()
        {
            eventAggregator.GetEvent<CloseChangeContactInfoViewEvent>().Publish(0);
        }
        private bool canCancel()
        {
            return true;
        }
        #endregion Command Delegate Method

        #region Constructor & deconstructor
        public ChangeContactInfoViewModel(IUnityContainer container)
        {
            Debug.WriteLine("ChangeContactInfoViewModel constructor");
            this.container = container;
            eventAggregator = this.container.Resolve<IEventAggregator>();
            model = this.container.Resolve<WheesperModel>();

            subevent();
        }

        ~ChangeContactInfoViewModel()
        {
            Debug.WriteLine("ChangeContactInfoViewModel Deconstrutor");
        }
        #endregion Constructor & deconstructor

        #region helper functoin
        private void subevent()
        {
            Debug.WriteLine("ChangeContactInfoViewModel subscribe event");
        }
        #endregion helper function
    }
}
