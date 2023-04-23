using BSBBChat.UI;
using Zenject;

namespace BSBBChat.Installers
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
