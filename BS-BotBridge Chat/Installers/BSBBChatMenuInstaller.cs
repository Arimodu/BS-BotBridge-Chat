using BS_BotBridge_Chat.UI;
using Zenject;

namespace BS_BotBridge_Chat.Installers
{
    internal class BSBBChatMenuInstaller : Installer
    {
        Module _module;
        internal BSBBChatMenuInstaller(Module module) 
        { 
            _module = module;
        }

        public override void InstallBindings()
        {
            Container.Bind<BSBBChatViewController>().FromNewComponentAsViewController().AsSingle();
            Container.Bind<BSBBChatFlowCoordinator>().FromNewComponentOnNewGameObject().AsSingle();
            Container.QueueForInject(_module);
        }
    }
}
