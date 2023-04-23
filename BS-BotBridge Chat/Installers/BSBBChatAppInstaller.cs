using Zenject;
using BSBBChat.Configuration;
using BSBBChat.Managers;

namespace BSBBChat.Installers
{
    internal class BSBBChatAppInstaller : Installer
    {
        private readonly BSBBChatConfig _config;
        private readonly Module _module;

        internal BSBBChatAppInstaller(BSBBChatConfig config, Module module)
        {
            _config = config;
            _module = module;
        }

        public override void InstallBindings()
        {
            Container.BindInstance(_config);
            Container.BindInstance(_module).AsSingle();
            Container.QueueForInject(_module);
        }
    }
}
