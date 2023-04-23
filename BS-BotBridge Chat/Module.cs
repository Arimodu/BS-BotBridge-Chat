using BSBBChat.Configuration;
using BSBBChat.UI;
using BSBBLib;
using BSBBLib.Packets;
using BSBBLib.Interfaces;
using HMUI;
using SiraUtil.Logging;
using System;
using Zenject;

namespace BSBBChat
{
    internal class Module : IModule
    {
        private SiraLog _logger;
        private BSBBChatConfig _config;
        private IClient _client;
        private BSBBChatViewController _viewController;

        internal ChatFloatingScreen ChatFloatingScreen;

        public string DisplayName => "Chat";
        public string HoverText => null;
        public ViewController ViewController 
        { 
            get { return _viewController; }
            internal set { _viewController = (BSBBChatViewController)value; }
        }
        public ViewController LeftViewController => null;
        public ViewController RightViewController => null;
        public ViewController TopViewController => null;
        public ViewController BottomViewController => null;

        [Inject]
        public void InjectDependencies(SiraLog logger, BSBBChatConfig config)
        {
            _logger = logger;
            _config = config;
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
