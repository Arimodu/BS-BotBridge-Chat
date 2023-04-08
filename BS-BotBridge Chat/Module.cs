using BS_BotBridge_Chat.Configuration;
using BS_BotBridge_Chat.UI;
using BSBBLib;
using BSBBLib.Packets;
using BSBBLib.Interfaces;
using HMUI;
using SiraUtil.Logging;
using System;
using Zenject;

namespace BS_BotBridge_Chat
{
    internal class Module : IModule
    {
        private SiraLog _logger;
        private BSBBChatConfig _config;
        private IClient _client;

        public FlowCoordinator FlowCoordinator { get; private set; }
        public string DisplayName => "Chat";
        public string HoverText => null;

        [Inject]
        public void InjectAppDependencies(SiraLog logger, BSBBChatConfig config)
        {
            _logger = logger;
            _config = config;
            logger.Info($"{nameof(Module)} app dependecy injected");
        }

        [Inject]
        public void InjectMenuDependencies(BSBBChatFlowCoordinator flowCoordinator)
        {
            FlowCoordinator = flowCoordinator;
            _logger.Info($"{nameof(Module)} menu dependecy injected");
        }

        public void Initialize(IClient client)
        {
            _logger.Info("BSBB Chat module initializing...");
            _client = client;
            _client.OnStateChanged += Client_OnStateChanged;
        }

        private void Client_OnStateChanged(ConnectionState state)
        {
            switch (state)
            {
                case ConnectionState.Connecting: break;
                case ConnectionState.Connected: break;
                case ConnectionState.Disabled: break;
                case ConnectionState.Errored: break;
            }
        }

        public void RecievePacket(Packet packet)
        {
            throw new NotImplementedException();
        }
    }
}
