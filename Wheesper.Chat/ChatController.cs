using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Wheesper.Infrastructure;
using Wheesper.Infrastructure.events;

using System.Diagnostics;
using Wheesper.Chat.View;
using Wheesper.Chat.ViewModel;

namespace Wheesper.Chat
{
    public class ChatController
    {
        #region private member
        IUnityContainer container = null;
        IEventAggregator eventAggregator = null;
        IRegionManager regionManager = null;
        IRegion mainRegion = null;
        #endregion private member

        #region view
        ChatView chatView = null;
        #endregion view

        #region constructor & deconstructor
        public ChatController(IUnityContainer container)
        {
            Debug.WriteLine("ChatController constructor");
            this.container = container;
            eventAggregator = this.container.Resolve<IEventAggregator>();
            regionManager = this.container.Resolve<IRegionManager>();

            mainRegion = regionManager.Regions[RegionNames.MainRegion];

            subevent();
        }

        ~ChatController()
        {
            Debug.WriteLine("ChatController deconstructor");
        }
        #endregion constructor & deconstructor

        #region helper function
        private void subevent()
        {
            Debug.WriteLine("ChatController sub event");
            eventAggregator.GetEvent<ShowWheesperViewEvent>().Subscribe(showWheesperViewEventHandler, ThreadOption.UIThread, true);
        }
        private void loadView(object view)
        {
            foreach (var v in mainRegion.Views)
            {
                mainRegion.Remove(v);
                Debug.Write("remove view ");
                Debug.WriteLine(v);
            }
            Debug.Write("add view ");
            Debug.WriteLine(view);
            mainRegion.Add(view);
            mainRegion.Activate(view);
        }
        #endregion helpfunction

        #region event handler
        private void showWheesperViewEventHandler(object o)
        {
            Debug.WriteLine("ShowWheesperViewEvent handler from ChatController");
            if (chatView == null)
            {
                chatView = (ChatView)container.Resolve(typeof(ChatView));
                var viewModel = (ChatViewModel)container.Resolve(typeof(ChatViewModel));
                chatView.DataContext = viewModel;
            }
            loadView(chatView);
        }
        #endregion event handler
    }
}
