using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;

using System.Text.RegularExpressions;
using System.Diagnostics;
using Prism.Mvvm;
using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Data;

using Wheesper.Chat.Model;
using Wheesper.Infrastructure.events;

namespace Wheesper.Chat.ViewModel
{
    public class ChatViewModel : BindableBase
    {
        #region private menber
        private IUnityContainer container = null;
        private IEventAggregator eventAggregator = null;
        private WheesperModel model = null;
        #endregion private menber

        #region private method
        
        #endregion private method

        #region properties
        public ListCollectionView Contacts { get; private set; }
        #endregion properties

        #region Commond
        //private DelegateCommand 
        #endregion Commond

        #region Constructor & deconstructor
        public ChatViewModel(IUnityContainer container)
        {
            Debug.WriteLine("ChatViewModel constructor");
            this.container = container;
            eventAggregator = this.container.Resolve<IEventAggregator>();
            model = this.container.Resolve<WheesperModel>();

            subevent();
        }

        ~ChatViewModel()
        {
            Debug.WriteLine("ChatViewModel Deconstrutor");
        }
        #endregion Constructor & deconstructor

        #region helper functoin
        private void subevent()
        {
            Debug.WriteLine("ChatViewModel subscribe event");
            // Contacts.CurrentChanged += contactSelectedItemChanged;
            //eventAggregator.GetEvent<ShowWheesperViewEvent>().Subscribe(showWheesperViewEventHandler, true);
            eventAggregator.GetEvent<LoginEvent>().Subscribe(loginEventHandler, true);
        }
        #endregion helper function

        #region Command Delegate Method

        #endregion Command Delegate Method

        #region event handler
        private void contactSelectedItemChanged(object sender, EventArgs e)
        {

        }

        private void loginEventHandler(string email)
        {
            Debug.Write("From loginEventHandler in ChatViewModel: ", email);
            model.sendUserInfoQueryRequest(email);
        }
        #endregion event handler
    }
}
