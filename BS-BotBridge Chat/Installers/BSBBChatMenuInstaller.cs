using BS_BotBridge_Chat.UI;
using Zenject;

namespace BS_BotBridge_Chat.Installers
{
    internal class BSBBChatMenuInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<BSBBChatViewController>().FromNewComponentAsViewController().AsSingle();
            Container.BindInterfacesAndSelfTo<ChatFloatingScreen>().FromNewComponentAsViewController().AsSingle();
            Container.BindInterfacesAndSelfTo<ViewControllerInjector>().AsSingle();
        }
    }
}
