﻿using BSBBLib;
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
        private IClient _client;

        public FlowCoordinator FlowCoordinator => null;
        public string DisplayName => null;
        public string HoverText => null;

        [Inject]
        public void InjectDependencies(SiraLog logger)
        {
            _logger = logger;
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
