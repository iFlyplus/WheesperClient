using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using Wheesper.Infrastructure.events;

using System.Diagnostics;

namespace Wheesper.Login.ViewModel
{
    public class WelcomeViewModel : NotificationObject
    {
        #region private member
        private IUnityContainer container = null;
        private IEventAggregator eventAggregator = null;
        #endregion private member

        #region Constructor
        public WelcomeViewModel(IUnityContainer container)
        {
            Debug.WriteLine("SignupViewModel constructor");
            this.container = container;
            eventAggregator = this.container.Resolve<IEventAggregator>();
        }

        public void Initialize(string welcomeMessage_1, string welcomeMessage_2)
        {
            WelcomeMessage_1 = welcomeMessage_1;
            WelcomeMessage_2 = welcomeMessage_2;
        }
        #endregion Constructor

        #region properties
        private string welcomeMessage_1;
        private string welcomeMessage_2;

        public string WelcomeMessage_1
        {
            get
            {
                return welcomeMessage_1;
            }
            set
            {
                welcomeMessage_1 = value;
                RaisePropertyChanged("WelcomeMessage_1");
            }
        }
        public string WelcomeMessage_2
        {
            get
            {
                return welcomeMessage_2;
            }
            set
            {
                welcomeMessage_2 = value;
                RaisePropertyChanged("WelcomeMessage_2");
            }
        }
        #endregion properties

        #region Command
        private DelegateCommand startCommnd;

        public DelegateCommand StartCommand
        {
            get
            {
                if (startCommnd == null)
                {
                    startCommnd = new DelegateCommand(start);
                }
                return startCommnd;
            }
        }
        #endregion Command

        #region event handler
        private void start()
        {
            eventAggregator.GetEvent<ShowWheesperViewEvent>().Publish(0);
        }
        #endregion event handler
    }
}
