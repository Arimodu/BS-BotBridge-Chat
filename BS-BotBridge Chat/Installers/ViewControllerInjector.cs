using BSBBChat.UI;
using Zenject;

namespace BSBBChat.Installers
{
    internal class ViewControllerInjector : IInitializable
    {
        private Module _module;
        private BSBBChatViewController _chatViewController;
        private ChatFloatingScreen _chatFloatingScreen;

        public ViewControllerInjector(Module module, BSBBChatViewController viewController, ChatFloatingScreen chatFloatingScreen) 
        {
            _module = module;
            _chatViewController = viewController;
            _chatFloatingScreen = chatFloatingScreen;
        }

        public void Initialize()
        {
            _module.ViewController = _chatViewController;
            _module.ChatFloatingScreen = _chatFloatingScreen;
        }
    }
}
