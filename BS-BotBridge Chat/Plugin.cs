using IPA;
using IPA.Config;
using IPA.Config.Stores;
using IPALogger = IPA.Logging.Logger;
using BS_BotBridge_Core.Managers;
using SiraUtil.Zenject;
using BS_BotBridge_Chat.Installers;
using BS_BotBridge_Chat.Configuration;

namespace BS_BotBridge_Chat
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        [Init]
        public void Init(IPALogger logger, Config conf, BSBBModuleManager moduleManager, Zenjector zenject)
        {
            Module module = new Module();

            moduleManager.RegisterModule("Chat", module);

            zenject.UseLogger(logger);
            zenject.UseMetadataBinder<Plugin>();

            zenject.Install<BSBBChatAppInstaller>(Location.App, conf.Generated<BSBBChatConfig>(), module);
            zenject.Install<BSBBChatMenuInstaller>(Location.Menu);
        }
    }
}
