using IPA;
using IPA.Config.Stores;
using IPALogger = IPA.Logging.Logger;
using Config = IPA.Config.Config;
using BS_BotBridge_Core.Managers;
using SiraUtil.Zenject;
using BSBBChat.Installers;
using BSBBChat.Configuration;
using BeatSaberMarkupLanguage;
using BSBBChat.BSMLTags;

namespace BSBBChat
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    [NoEnableDisable]
    public class Plugin
    {
        [Init]
        public void Init(IPALogger logger, Config conf, BSBBModuleManager moduleManager, Zenjector zenject)
        {
            // Stashed for the time being, might use eventually.
            //// Register custom tags
            //BSMLParser.instance.RegisterTag(new TextInputField());

            // Create a new instance of Module and register it to the ModuleManager
            Module module = new Module();
            moduleManager.RegisterModule("Chat", module);

            // Zenject stuff
            zenject.UseLogger(logger);
            zenject.UseMetadataBinder<Plugin>();

            // Lets let zenject do the rest of the work from here
            zenject.Install<BSBBChatAppInstaller>(Location.App, conf.Generated<BSBBChatConfig>(), module);
            zenject.Install<BSBBChatMenuInstaller>(Location.Menu);
        }
    }
}
