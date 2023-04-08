using BeatSaberMarkupLanguage;
using BS_BotBridge_Core.Managers;
using HMUI;
using System;
using Zenject;

namespace BS_BotBridge_Chat.UI
{
    internal class BSBBChatFlowCoordinator : FlowCoordinator
    {
        protected BSBBChatViewController _bSBBChatViewController;
        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            if (firstActivation)
            {
                SetTitle("Bot Bridge Chat Settings");
                showBackButton = true;
                ProvideInitialViewControllers(_bSBBChatViewController);
            }
        }

        [Inject]
        internal void InjectDependencies(BSBBChatViewController chatViewController)
        {
            _bSBBChatViewController = chatViewController;
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            //if (topViewController == _bSBBCoreViewController)
            //{
            //    DismissViewController(topViewController);
            //    return;
            //}
            BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this);
        }
    }
}
