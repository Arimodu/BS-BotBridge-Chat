using BS_BotBridge_Chat.UI;
using Zenject;

namespace BS_BotBridge_Chat.Installers
{
    internal class ViewControllerInjector : IInitializable
    {
        private Module _module;
        private BSBBChatViewController _chatViewController;

        public ViewControllerInjector(Module module, BSBBChatViewController viewController) 
        {
            _module = module;
            _chatViewController = viewController;
        }

        public void Initialize()
        {
            _module.ViewController = _chatViewController;
        }
    }
}
