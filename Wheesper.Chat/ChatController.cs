using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Wheesper.Infrastructure;
using Wheesper.Infrastructure.events;

using System.Diagnostics;
using Wheesper.Chat.View;
using Wheesper.Chat.ViewModel;
using Wheesper.Login.events;
using Wheesper.Chat.region;
using ProtocolBuffer;

namespace Wheesper.Chat
{
    public class ChatController
    {
        #region private member
        IUnityContainer container = null;
        IEventAggregator eventAggregator = null;
        IRegionManager regionManager = null;
        IRegion mainRegion = null;
        IRegion systemMessageRegion = null;
        #endregion private member

        #region view
        ChatView chatView = null;
        SystemMessageView systemMessageView = null;
        SolveContactApplyView solveContactApplyView = null;
        ContactView contactView = null;
        #endregion view

        #region constructor & deconstructor
        public ChatController(IUnityContainer container)
        {
            Debug.WriteLine("ChatController constructor");
            this.container = container;
            eventAggregator = this.container.Resolve<IEventAggregator>();
            regionManager = this.container.Resolve<IRegionManager>();

            // delay systemMessageRegion init until ShowWheesperViewEvent handler
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
            eventAggregator.GetEvent<LoadContactViewEvent>().Subscribe(loadContactViewEventHandler, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<ShowSystemMessageViewEvent>().Subscribe(showSystemMessageViewEventHandler, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<CloseSystemMessageViewEvent>().Subscribe(closeSystemMessageViewEventHandler, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<ShowSolveContactApplyViewEvent>().Subscribe(solveContactApplyViewEventHandler, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<CloseSolveContactApplyViewEvent>().Subscribe(closeSolveContactApplyViewEventHandler, ThreadOption.UIThread, true);
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

            // only now can region find ChatRegionNames.SystemMessageName
            systemMessageRegion = regionManager.Regions[ChatRegionNames.RegionOne];
            eventAggregator.GetEvent<LoadContactViewEvent>().Publish(0);
        }
        private void loadContactViewEventHandler(object o)
        {
            if (contactView == null)
            {
                contactView = (ContactView)container.Resolve(typeof(ContactView));
                var viewModel = (ChatViewModel)container.Resolve(typeof(ChatViewModel));
                contactView.DataContext = viewModel;
            }
            Debug.Write("add view to RegionOne ");
            Debug.WriteLine(contactView);
            foreach (var v in systemMessageRegion.Views)
            {
                systemMessageRegion.Remove(v);
                Debug.Write("remove view ");
                Debug.WriteLine(v);
            }
            systemMessageRegion.Add(contactView);
            systemMessageRegion.Activate(contactView);
        }
        private void showSystemMessageViewEventHandler(object o)
        {
            Debug.WriteLine("ShowSystemMessageViewEvent handler from ChatController");
            if (systemMessageView == null)
            {
                systemMessageView = (SystemMessageView)container.Resolve(typeof(SystemMessageView));
                var viewModel = (ChatViewModel)container.Resolve(typeof(ChatViewModel));
                systemMessageView.DataContext = viewModel;
            }
            Debug.Write("add view to RegionOne ");
            Debug.WriteLine(systemMessageView);
            foreach (var v in systemMessageRegion.Views)
            {
                systemMessageRegion.Remove(v);
                Debug.Write("remove view ");
                Debug.WriteLine(v);
            }
            systemMessageRegion.Add(systemMessageView);
            systemMessageRegion.Activate(systemMessageView);
        }
        private void closeSystemMessageViewEventHandler(object o)
        {
            Debug.WriteLine("CloseSystemMessageViewEvent handler from ChatController");

            Debug.Write("remove view from RegionOne ");
            Debug.WriteLine(systemMessageView);
            systemMessageRegion.Remove(systemMessageView);
            eventAggregator.GetEvent<LoadContactViewEvent>().Publish(0);
        }
        private void solveContactApplyViewEventHandler(object o)
        {
            Debug.WriteLine("ShowSolveContactApplyViewEvent handler from ChatController");
            if (solveContactApplyView == null)
            {
                solveContactApplyView = (SolveContactApplyView)container.Resolve(typeof(SolveContactApplyView));
                var viewModel = (SolveContactApplyViewModel)container.Resolve(typeof(SolveContactApplyViewModel));

                viewModel.ApplierEMail = ((ProtoMessage)o).ContactApplyingInfoPushMessage.ApplyerMailAddress;
                viewModel.TargetEMail = ((ProtoMessage)o).ContactApplyingInfoPushMessage.TargetMailAddress;
                viewModel.Discription = "Hi";
                solveContactApplyView.DataContext = viewModel;
            }
            Debug.Write("add view to RegionOne ");
            Debug.WriteLine(solveContactApplyView);

            foreach (var v in systemMessageRegion.Views)
            {
                systemMessageRegion.Remove(v);
                Debug.Write("remove view ");
                Debug.WriteLine(v);
            }
            systemMessageRegion.Add(solveContactApplyView);
            systemMessageRegion.Activate(solveContactApplyView);
        }
        private void closeSolveContactApplyViewEventHandler(object o)
        {
            Debug.WriteLine("CloseSolveContactApplyViewEvent handler from ChatController");

            Debug.Write("remove view from RegionOne ");
            Debug.WriteLine(solveContactApplyView);
            systemMessageRegion.Remove(solveContactApplyView);
        }
        #endregion event handler
    }
}
