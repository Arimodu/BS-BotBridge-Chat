using Zenject;
using BS_BotBridge_Chat.Configuration;

namespace BS_BotBridge_Chat.Installers
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
