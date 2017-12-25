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
using Wheesper.Chat.Model;

namespace Wheesper.Chat
{
    public class ChatController
    {
        #region private member
        IUnityContainer container = null;
        IEventAggregator eventAggregator = null;
        IRegionManager regionManager = null;
        IRegion mainRegion = null;
        IRegion regionOne = null;
        IRegion regionTwo = null;
        #endregion private member

        #region view
        WheesperView wheesperView = null;
        SystemMessageView systemMessageView = null;
        SolveContactApplyView solveContactApplyView = null;
        ContactView contactView = null;
        ChatView chatView = null;
        FaceView faceView = null;
        ChangeContactInfoView changeContactInfoView = null;
        ContactNotExistView contactNotExistView = null;
        ContactExist contactExist = null;
        #endregion view

        #region constructor & deconstructor
        public ChatController(IUnityContainer container)
        {
            Debug.WriteLine("ChatController constructor");
            this.container = container;
            eventAggregator = this.container.Resolve<IEventAggregator>();
            regionManager = this.container.Resolve<IRegionManager>();

            // delay regionOne init until ShowWheesperViewEvent handler
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
            eventAggregator.GetEvent<LoadChatViewEvent>().Subscribe(loadChatViewEventHandler, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<ShowSystemMessageViewEvent>().Subscribe(showSystemMessageViewEventHandler, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<CloseSystemMessageViewEvent>().Subscribe(closeSystemMessageViewEventHandler, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<ShowSolveContactApplyViewEvent>().Subscribe(showSolveContactApplyViewEventHandler, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<CloseSolveContactApplyViewEvent>().Subscribe(closeSolveContactApplyViewEventHandler, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<ShowChangeContactInfoViewEvent>().Subscribe(showChangeContactInfoViewEventHandler, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<CloseChangeContactInfoViewEvent>().Subscribe(closeChangeContactInfoViewEventHandler, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<ShowUserExistViewEvent>().Subscribe(showUserExistViewEventHandler, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<ShowUserNotExistViewEvent>().Subscribe(showUserNotExistViewEventHandler, ThreadOption.UIThread, true);
            eventAggregator.GetEvent<CloseUserExistOrNotExistViewEvent>().Subscribe(closeUserExistOrNotExistViewEventHandler, ThreadOption.UIThread, true);
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
            if (wheesperView == null)
            {
                wheesperView = (WheesperView)container.Resolve(typeof(WheesperView));
                var viewModel = (ChatViewModel)container.Resolve(typeof(ChatViewModel));
                wheesperView.DataContext = viewModel;
            }
            loadView(wheesperView);

            // only now can region manager find region define in wheesperView: region one & regiono two
            regionOne = regionManager.Regions[ChatRegionNames.RegionOne];
            regionTwo = regionManager.Regions[ChatRegionNames.RegionTwo];
            eventAggregator.GetEvent<LoadContactViewEvent>().Publish(0);

            if (faceView == null)
            {
                faceView = (FaceView)container.Resolve(typeof(FaceView));
                var viewModel = (ChatViewModel)container.Resolve(typeof(ChatViewModel));
                faceView.DataContext = viewModel;
            }
            Debug.Write("add view to RegionTwo ");
            Debug.WriteLine(faceView);
            foreach (var v in regionTwo.Views)
            {
                regionTwo.Remove(v);
                Debug.Write("remove view ");
                Debug.WriteLine(v);
            }
            regionTwo.Add(faceView);
            regionTwo.Activate(faceView);
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
            foreach (var v in regionOne.Views)
            {
                regionOne.Remove(v);
                Debug.Write("remove view ");
                Debug.WriteLine(v);
            }
            regionOne.Add(contactView);
            regionOne.Activate(contactView);
        }

        private void loadChatViewEventHandler(object o)
        {
            if (chatView == null)
            {
                chatView = (ChatView)container.Resolve(typeof(ChatView));
                var viewModel = (ChatViewModel)container.Resolve(typeof(ChatViewModel));
                chatView.DataContext = viewModel;
            }
            Debug.Write("add view to RegionTwo ");
            Debug.WriteLine(chatView);
            foreach (var v in regionTwo.Views)
            {
                regionTwo.Remove(v);
                Debug.Write("remove view ");
                Debug.WriteLine(v);
            }
            regionTwo.Add(chatView);
            regionTwo.Activate(chatView);
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
            foreach (var v in regionOne.Views)
            {
                regionOne.Remove(v);
                Debug.Write("remove view ");
                Debug.WriteLine(v);
            }
            regionOne.Add(systemMessageView);
            regionOne.Activate(systemMessageView);
        }
        private void closeSystemMessageViewEventHandler(object o)
        {
            Debug.WriteLine("CloseSystemMessageViewEvent handler from ChatController");

            Debug.Write("remove view from RegionOne ");
            Debug.WriteLine(systemMessageView);
            regionOne.Remove(systemMessageView);
            eventAggregator.GetEvent<LoadContactViewEvent>().Publish(0);
        }

        private void showSolveContactApplyViewEventHandler(object o)
        {
            Debug.WriteLine("ShowSolveContactApplyViewEvent handler from ChatController");
            // Notice: create view once, but always refresh its viewmodel.
            if (solveContactApplyView == null)
            {
                solveContactApplyView = (SolveContactApplyView)container.Resolve(typeof(SolveContactApplyView));
            }
            var viewModel = (SolveContactApplyViewModel)container.Resolve(typeof(SolveContactApplyViewModel));
            viewModel.ApplierEMail = ((ProtoMessage)o).ContactApplyingInfoPushMessage.ApplyerMailAddress;
            viewModel.TargetEMail = ((ProtoMessage)o).ContactApplyingInfoPushMessage.TargetMailAddress;
            viewModel.Discription = ((ProtoMessage)o).ContactApplyingInfoPushMessage.AdditionalMsg;
            solveContactApplyView.DataContext = viewModel;

            Debug.Write("add view to RegionOne ");
            Debug.WriteLine(solveContactApplyView);

            foreach (var v in regionOne.Views)
            {
                regionOne.Remove(v);
                Debug.Write("remove view ");
                Debug.WriteLine(v);
            }
            regionOne.Add(solveContactApplyView);
            regionOne.Activate(solveContactApplyView);
        }
        private void closeSolveContactApplyViewEventHandler(object o)
        {
            Debug.WriteLine("CloseSolveContactApplyViewEvent handler from ChatController");

            Debug.Write("remove view from RegionOne ");
            Debug.WriteLine(solveContactApplyView);
            regionOne.Remove(solveContactApplyView);
        }

        private void showChangeContactInfoViewEventHandler(object o)
        {
            Debug.WriteLine("ShowChangeContactInfoViewEvent handler from ChatController");
            if (changeContactInfoView == null)
            {
                changeContactInfoView = (ChangeContactInfoView)container.Resolve(typeof(ChangeContactInfoView));
            }
            var viewModel = (ChangeContactInfoViewModel)container.Resolve(typeof(ChangeContactInfoViewModel));
            viewModel.ContactEMail = ((Contact)o).EMail;
            viewModel.Remarks = ((Contact)o).Remarks;
            viewModel.Group = ((Contact)o).Group;
            changeContactInfoView.DataContext = viewModel;

            foreach (var v in regionOne.Views)
            {
                regionOne.Remove(v);
                Debug.Write("Remove view from RegionOne ");
                Debug.WriteLine(v);
            }
            regionOne.Add(changeContactInfoView);
            regionOne.Activate(changeContactInfoView);
            Debug.Write("add view to RegionOne ");
            Debug.WriteLine(solveContactApplyView);
        }
        private void closeChangeContactInfoViewEventHandler(object o)
        {
            Debug.WriteLine("CloseChangeContactInfoViewEvent handler from ChatController");

            Debug.Write("remove view from RegionOne ");
            Debug.WriteLine(changeContactInfoView);
            regionOne.Remove(changeContactInfoView);
            eventAggregator.GetEvent<LoadContactViewEvent>().Publish(0);
        }

        private void showUserExistViewEventHandler(string mail)
        {
            Debug.WriteLine("ShowUserExistViewEvent handler from ChatController");
            // Notice: create view once, but always refresh its viewmodel.
            if (contactExist == null)
            {
                contactExist = (ContactExist)container.Resolve(typeof(ContactExist));
            }
            var viewModel = (ContactExistViewModel)container.Resolve(typeof(ContactExistViewModel));
            viewModel.EMail = mail;
            contactExist.DataContext = viewModel;

            Debug.Write("add view to RegionOne ");
            Debug.WriteLine(contactExist);

            foreach (var v in regionOne.Views)
            {
                regionOne.Remove(v);
                Debug.Write("remove view ");
                Debug.WriteLine(v);
            }
            regionOne.Add(contactExist);
            regionOne.Activate(contactExist);
        }
        private void showUserNotExistViewEventHandler(string mail)
        {
            Debug.WriteLine("ShowUserNotExistViewEvent handler from ChatController");
            // Notice: create view once, but always refresh its viewmodel.
            if (contactNotExistView == null)
            {
                contactNotExistView = (ContactNotExistView)container.Resolve(typeof(ContactNotExistView));
            }
            var viewModel = (ContactExistViewModel)container.Resolve(typeof(ContactExistViewModel));
            viewModel.EMail = mail;
            contactNotExistView.DataContext = viewModel;

            Debug.Write("add view to RegionOne ");
            Debug.WriteLine(contactNotExistView);

            foreach (var v in regionOne.Views)
            {
                regionOne.Remove(v);
                Debug.Write("remove view ");
                Debug.WriteLine(v);
            }
            regionOne.Add(contactNotExistView);
            regionOne.Activate(contactNotExistView);
        }
        private void closeUserExistOrNotExistViewEventHandler(bool isExist)
        {
            Debug.WriteLine("CloseUserExistOrNotExistViewEvent handler from ChatController");

            foreach (var v in regionOne.Views)
            {
                regionOne.Remove(v);
                Debug.Write("remove view ");
                Debug.WriteLine(v);
            }
            /*
            if (isExist)
            {
                Debug.Write("remove view from RegionOne ");
                Debug.WriteLine(contactExist);
                regionOne.Remove(contactExist);
            }
            else
            {
                Debug.Write("remove view from RegionOne ");
                Debug.WriteLine(contactNotExistView);
                regionOne.Remove(contactNotExistView);
            }
            */
            eventAggregator.GetEvent<LoadContactViewEvent>().Publish(0);
        }
        #endregion event handler
    }
}
