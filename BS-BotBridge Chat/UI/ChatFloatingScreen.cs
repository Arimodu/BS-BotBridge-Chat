using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.FloatingScreen;
using BeatSaberMarkupLanguage.ViewControllers;
using BS_BotBridge_Chat.Configuration;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BS_BotBridge_Chat.UI
{
    [HotReload(RelativePathToLayout = @"ChatFloatingScreen.bsml")]
    [ViewDefinition("BS_BotBridge_Chat.UI.ChatFloatingScreen.bsml")]
    internal class ChatFloatingScreen : BSMLAutomaticViewController, IInitializable, IDisposable
    {
        private FloatingScreen _chatScreen;
        private BSBBChatConfig _config;

        private bool _createHandle;
        private float _screenWidth;
        private float _screenHeight;
        private Vector3 _screenPosition;
        private Vector3 _screenRotation;

        public bool CreateHandle
        {
            get { return _createHandle; }
            set
            {
                if (_createHandle == value) return;
                _createHandle = value;
                NotifyPropertyChanged(nameof(CreateHandle));
                _chatScreen.ShowHandle = _createHandle; // Just in case
            }
        }
        public float ScreenWidth
        {
            get { return _screenWidth; }
            set
            {
                if (_screenWidth == value) return;
                _screenWidth = value;
                NotifyPropertyChanged(nameof(ScreenWidth));
                UpdateScreen();
            }
        }
        public float ScreenHeight
        {
            get { return _screenHeight; }
            set
            {
                if (_screenHeight == value) return;
                _screenHeight = value;
                NotifyPropertyChanged(nameof(ScreenHeight));
                UpdateScreen();
            }
        }
        public Vector3 ScreenPosition
        {
            get { return _screenPosition; }

            set
            {
                if (_screenPosition == value) return;
                _screenPosition = value;
                NotifyPropertyChanged(nameof(ScreenPosition));
                UpdateScreen();
            }
        }
        public Vector3 ScreenRotationVector 
        { 
            get { return _screenRotation; } 
            set
            {
                if (_screenRotation == value) return;
                _screenRotation = value;
                NotifyPropertyChanged(nameof(ScreenRotationVector));
                UpdateScreen();
            }
        }
        public Quaternion ScreenRotation
        {
            get 
            { 
                return Quaternion.Euler(ScreenRotationVector); 
            }
            set
            {
                _config.MenuChatRotation = new Vector3(value.x, value.y, value.z);
            }
        }

        [Inject]
        internal void InjectDependencies(BSBBChatConfig config)
        {
            _config = config;
            _config.OnChanged += Config_OnChanged;
        }

        private void Config_OnChanged()
        {
            CreateHandle = _config.EnableHandle;
            ScreenWidth = _config.ScreenWidth;
            ScreenHeight = _config.ScreenHeight;
            ScreenPosition = _config.MenuChatPosition;
            ScreenRotationVector = _config.MenuChatRotation;
            _chatScreen.enabled = _config.Enabled;
        }

        public void Initialize()
        {
            _createHandle = _config.EnableHandle;
            _screenWidth = _config.ScreenWidth;
            _screenHeight = _config.ScreenHeight;
            _screenPosition = _config.MenuChatPosition;
            _screenRotation = _config.MenuChatRotation;

            _chatScreen = FloatingScreen.CreateFloatingScreen(new Vector2(ScreenWidth, ScreenHeight), CreateHandle, ScreenPosition, ScreenRotation, 0f, true);
            _chatScreen.gameObject.layer = 3;
            _chatScreen.GetComponent<Canvas>().worldCamera = Camera.main;
            _chatScreen.SetRootViewController(this, AnimationType.None);
            _chatScreen.HandleReleased += ChatScreen_HandleReleased;

            _chatScreen.enabled = _config.Enabled;
        }

        private void ChatScreen_HandleReleased(object sender, FloatingScreenHandleEventArgs e)
        {
            ScreenRotation = e.Rotation;
            _config.MenuChatPosition = e.Position;
        }

        private void UpdateScreen()
        {
            _chatScreen.ScreenPosition = ScreenPosition;
            _chatScreen.ScreenRotation = ScreenRotation;
        }

        [UIAction("#post-parse")]
        internal void PostParse()
        {
            // Code to run after BSML finishes
        }

        public void Dispose()
        {
            DestroyImmediate(_chatScreen);
            _chatScreen = null;
        }
    }
}
