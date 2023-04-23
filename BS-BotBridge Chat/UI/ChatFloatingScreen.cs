using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.FloatingScreen;
using BeatSaberMarkupLanguage.ViewControllers;
using BSBBChat.Configuration;
using BSBBLib;
using System;
using Tweening;
using UnityEngine;
using Zenject;
using BS_Utils.Utilities;

namespace BSBBChat.UI
{
    [HotReload(RelativePathToLayout = @"ChatFloatingScreen.bsml")]
    [ViewDefinition("BS_BotBridge_Chat.UI.ChatFloatingScreen.bsml")]
    internal class ChatFloatingScreen : BSMLAutomaticViewController, IInitializable, IDisposable
    {
        private FloatingScreen _chatScreen;
        private BSBBChatConfig _config;

        MainCamera _mainCamera;
        TimeTweeningManager _timeTweeningManager;

        private ConnectionState _connectionState;
        private Vector3 _chatScreenScale;
        private Vector3 _chatScreenHandleScale;
        private Vector3 _chatScreenHandlePosition;

        private bool _screenEnabled;
        private bool _createHandle;
        private bool _handleWholeScreen;
        private bool _reverseChatOrder;
        private float _screenWidth;
        private float _screenHeight;
        private Vector3 _screenPosition;
        private Quaternion _screenRotation;

        public bool ScreenEnabled
        {
            get => _screenEnabled;
            set
            {
                if (_screenEnabled == value) return;
                _screenEnabled = value;
                if (value) ShowFloatingScreen();
                else HideFloatingScreen();
            }
        }
        public bool ShowHandle
        {
            get => _createHandle;
            set
            {
                if (_createHandle == value) return;
                _createHandle = value;
                NotifyPropertyChanged(nameof(ShowHandle));
                if (value) ShowScreenHandle();
                else HideScreenHandle();
            }
        }
        public bool HandleWholeScreen
        {
            get => _handleWholeScreen;
            set
            {
                if (_handleWholeScreen == value) return;
                _handleWholeScreen = value;
                NotifyPropertyChanged(nameof(HandleWholeScreen));
                if (value) ShowWholeScreenHandle();
                else HideWholeScreenHandle();
            }
        }
        public float ScreenWidth
        {
            get => _screenWidth;
            set
            {
                if (_screenWidth == value) return;
                _screenWidth = value;
                NotifyPropertyChanged(nameof(ScreenWidth));
                UpdateScreenSize();
            }
        }
        public float ScreenHeight
        {
            get => _screenHeight;
            set
            {
                if (_screenHeight == value) return;
                _screenHeight = value;
                NotifyPropertyChanged(nameof(ScreenHeight));
                UpdateScreenSize();
            }
        }
        public Vector3 ScreenPosition
        {
            get => _screenPosition;

            set
            {
                if (_screenPosition == value) return;
                var previousScreenPosition = _screenPosition;
                _screenPosition = value;
                NotifyPropertyChanged(nameof(ScreenPosition));
                UpdateScreenPosition(value, previousScreenPosition);
            }
        }
        public Quaternion ScreenRotation 
        { 
            get => _screenRotation; 
            set
            {
                if (_screenRotation == value) return;
                var previousScreenRotation = _screenRotation;
                _screenRotation = value;
                NotifyPropertyChanged(nameof(ScreenRotation));
                UpdateScreenRotation(value, previousScreenRotation);
            }
        }
        public ChatScreenPosition CurrentPosition { get; private set; } = ChatScreenPosition.Menu;

        public ConnectionState ConnectionState
        {
            get => _connectionState;
            set
            {
                if (_connectionState == value) return;
                _connectionState = value;
                NotifyPropertyChanged(nameof(ConnectionState));
            }
        }

        public enum ChatScreenPosition
        {
            Menu,
            Game,
            Pause,
        }

        [Inject]
        internal void InjectDependencies(BSBBChatConfig config, MainCamera mainCamera, TimeTweeningManager timeTweeningManager)
        {
            _config = config;
            _mainCamera = mainCamera;
            _timeTweeningManager = timeTweeningManager;
            _config.OnChanged += Config_OnChanged;
        }

        private void Config_OnChanged()
        {
            ScreenEnabled = _config.MenuScreenEnabled;
            ShowHandle = _config.HandleEnabled;
            HandleWholeScreen = _config.HandleWholeScreen;

            ScreenWidth = _config.ScreenWidth;
            ScreenHeight = _config.ScreenHeight;
            ScreenPosition = _config.MenuChatPosition;
            ScreenRotation = Quaternion.Euler(_config.MenuChatRotation);
        }

        private void ShowScreenHandle()
        {
            if (_chatScreen == null) return;

            if (HandleWholeScreen)
            {
                ShowWholeScreenHandle();
                return;
            }

            _chatScreen.handle.transform.localPosition = _chatScreenHandlePosition;

            var tween = new Vector3Tween(Vector3.zero, _chatScreenHandleScale, val => _chatScreen.handle.transform.localScale = val, 0.3f, EaseType.OutExpo)
            {
                onStart = () => _chatScreen.ShowHandle = true
            };
            _timeTweeningManager.KillAllTweens(_chatScreen.handle);
            _timeTweeningManager.AddTween(tween, _chatScreen.handle);
        }

        private void ShowWholeScreenHandle()
        {
            if (_chatScreen == null) return;

            Vector3 fromSizeValue;
            if (_chatScreen.ShowHandle) fromSizeValue = _chatScreenHandleScale;
            else fromSizeValue = Vector3.zero;

            Vector3 toSizeValue = new Vector3(ScreenWidth, ScreenHeight, 1f);

            var sizeTween = new Vector3Tween(fromSizeValue, toSizeValue, val => _chatScreen.handle.transform.localScale = val, 0.3f, EaseType.OutExpo)
            {
                onStart = () => _chatScreen.ShowHandle = true
            };

            _timeTweeningManager.KillAllTweens(_chatScreen.handle);
            _timeTweeningManager.AddTween(sizeTween, _chatScreen.handle);

            Vector3 fromPosValue = _chatScreenHandlePosition;
            Vector3 toPosValue = fromPosValue;
            toPosValue.y += ScreenHeight / 2f;

            // If handle is already visible apply tween to position
            // Else just apply position
            if (_chatScreen.ShowHandle)
            {
                var posTween = new Vector3Tween(fromPosValue, toPosValue, val => _chatScreen.handle.transform.localPosition = val, 0.3f, EaseType.OutExpo);
                _timeTweeningManager.AddTween(posTween, _chatScreen.handle);
            }
            else
            {
                _chatScreen.handle.transform.localPosition = toPosValue;
            }
        }

        private void HideScreenHandle()
        {
            if (_chatScreen == null) return;

            var tween = new Vector3Tween(_chatScreen.handle.transform.localScale, Vector3.zero, val => _chatScreen.handle.transform.localScale = val, 0.3f, EaseType.OutExpo)
            {
                onCompleted = () => _chatScreen.ShowHandle = false
            };
            _timeTweeningManager.KillAllTweens(_chatScreen.handle);
            _timeTweeningManager.AddTween(tween, _chatScreen.handle);
        }

        private void HideWholeScreenHandle()
        {
            if (_chatScreen == null) return;

            if (!ShowHandle)
            {
                HideScreenHandle();
                return;
            }

            Vector3Tween sizeTween = new Vector3Tween(_chatScreen.handle.transform.localScale, _chatScreenHandleScale, val => _chatScreen.handle.transform.localScale = val, 0.3f, EaseType.OutExpo); 
            Vector3Tween posTween = new Vector3Tween(_chatScreen.handle.transform.localPosition, _chatScreenHandlePosition, val => _chatScreen.handle.transform.localPosition = val, 0.3f, EaseType.OutExpo);

            _timeTweeningManager.KillAllTweens(_chatScreen.handle);
            _timeTweeningManager.AddTween(sizeTween, _chatScreen.handle);
            _timeTweeningManager.AddTween(posTween, _chatScreen.handle);
        }

        private void ShowFloatingScreen()
        {
            if (_chatScreen == null) CreateFloatingScreen();

            var tween = new Vector3Tween(new Vector3(0f, 0f, _chatScreen.transform.localScale.z), _chatScreenScale, val => _chatScreen.transform.localScale = val, 0.3f, EaseType.OutExpo)
            {
                onStart = () => _chatScreen.gameObject.SetActive(true)
            };
            _timeTweeningManager.KillAllTweens(_chatScreen);
            _timeTweeningManager.AddTween(tween, _chatScreen);
        }

        private void HideFloatingScreen()
        {
            if (_chatScreen == null) return;

            var tween = new Vector3Tween(_chatScreen.transform.localScale, new Vector3(0f, 0f), val => _chatScreen.transform.localScale = val, 0.3f, EaseType.OutExpo)
            {
                onCompleted = () => _chatScreen.gameObject.SetActive(false)
            };
            _timeTweeningManager.KillAllTweens(_chatScreen);
            _timeTweeningManager.AddTween(tween, _chatScreen);
        }

        public void Initialize()
        {
            _screenEnabled = _config.MenuScreenEnabled;
            _createHandle = _config.HandleEnabled;
            _handleWholeScreen = _config.HandleWholeScreen;
            _screenWidth = _config.ScreenWidth;
            _screenHeight = _config.ScreenHeight;
            _screenPosition = _config.MenuChatPosition;
            _screenRotation = Quaternion.Euler(_config.MenuChatRotation);

            if (ScreenEnabled) ShowFloatingScreen();
        }

        private void CreateFloatingScreen()
        {
            _chatScreen = FloatingScreen.CreateFloatingScreen(new Vector2(ScreenWidth, ScreenHeight), true, ScreenPosition, ScreenRotation, 0f, true);
            _chatScreen.name = "BSBBChatScreen";
            _chatScreen.HighlightHandle = true;

            var originalScreenScale = _chatScreen.transform.localScale;
            _chatScreen.transform.localScale = originalScreenScale * 1f;
            _chatScreenScale = _chatScreen.transform.localScale;

            DontDestroyOnLoad(_chatScreen);

            _chatScreen.HandleSide = FloatingScreen.Side.Bottom;
            var originalScreenHandleScale = _chatScreen.handle.transform.localScale;
            _chatScreen.handle.transform.localScale = originalScreenHandleScale * 0.7f;
            _chatScreenHandleScale = _chatScreen.handle.transform.localScale;
            _chatScreenHandlePosition = _chatScreen.handle.transform.localPosition;
            if (!ShowHandle) HideScreenHandle();
            if (HandleWholeScreen && ShowHandle) ShowWholeScreenHandle();

            _chatScreen.SetRootViewController(this, AnimationType.None);
            _chatScreen.HandleReleased += ChatScreen_HandleReleased;
            BSEvents.gameSceneLoaded += OnGameSceneLoaded;
            BSEvents.menuSceneLoaded += OnMenuSceneLoaded;
            BSEvents.songPaused += OnSongPaused;
            BSEvents.songUnpaused += OnSongResumed;
        }

        private void OnSongResumed()
        {
            if (!_config.DifferentPauseScreenPosition) return;
            ScreenPosition = _config.GameChatPosition;
            ScreenRotation = Quaternion.Euler(_config.GameChatRotation);
            CurrentPosition = ChatScreenPosition.Game;
        }

        private void OnSongPaused()
        {
            if (!_config.DifferentPauseScreenPosition) return;
            ScreenPosition = _config.PauseChatPosition;
            ScreenRotation = Quaternion.Euler(_config.PauseChatRotation);
            CurrentPosition = ChatScreenPosition.Pause;
        }

        private void OnMenuSceneLoaded()
        {
            ScreenPosition = _config.MenuChatPosition;
            ScreenRotation = Quaternion.Euler(_config.MenuChatRotation);
            CurrentPosition = ChatScreenPosition.Menu;
        }

        private void OnGameSceneLoaded()
        {
            if (!_config.DifferentGameScreenPosition) return;
            ScreenPosition = _config.GameChatPosition;
            ScreenRotation = Quaternion.Euler(_config.GameChatRotation);
            CurrentPosition = ChatScreenPosition.Game;
        }

        private void ChatScreen_HandleReleased(object sender, FloatingScreenHandleEventArgs e)
        {
            // Set directly so we dont trigger a tween
            _screenRotation = e.Rotation;
            _screenPosition = e.Position;
            NotifyPropertyChanged(nameof(ScreenPosition));
            NotifyPropertyChanged(nameof(ScreenRotation));

            // Update config
            switch (CurrentPosition)
            {
                case ChatScreenPosition.Menu:
                    _config.MenuChatPosition = e.Position;
                    _config.MenuChatRotation = e.Rotation.eulerAngles;
                    break;
                case ChatScreenPosition.Game:
                    _config.GameChatPosition = e.Position;
                    _config.GameChatRotation = e.Rotation.eulerAngles;
                    break; 
                case ChatScreenPosition.Pause:
                    _config.PauseChatPosition = e.Position;
                    _config.PauseChatRotation = e.Rotation.eulerAngles;
                    break; 
                default:
                    break;
            }
        }

        private void UpdateScreenSize()
        {
            if (_chatScreen == null) return;
            _chatScreen.ScreenSize = new Vector2(ScreenWidth, ScreenHeight);
            var originalScreenHandleScale = _chatScreen.handle.transform.localScale;
            _chatScreen.handle.transform.localScale = originalScreenHandleScale * 0.7f;
            _chatScreenHandleScale = _chatScreen.handle.transform.localScale;
        }

        private void UpdateScreenPosition(Vector3 newScreenPosition, Vector3 previousScreenPosition)
        {
            if (_chatScreen == null) return;

            var tween = new Vector3Tween(previousScreenPosition, newScreenPosition, val => _chatScreen.ScreenPosition = val, 0.3f, EaseType.OutExpo);
            _timeTweeningManager.KillAllTweens(_chatScreen.handle);
            _timeTweeningManager.AddTween(tween, _chatScreen.handle);
        }

        private void UpdateScreenRotation(Quaternion newScreenRotation, Quaternion previousScreenRotation)
        {
            if (_chatScreen == null) return;

            var tween = new Vector3Tween(previousScreenRotation.eulerAngles, newScreenRotation.eulerAngles, val => _chatScreen.ScreenRotation = Quaternion.Euler(val), 0.3f, EaseType.OutExpo);
            _timeTweeningManager.KillAllTweens(_chatScreen.handle);
            _timeTweeningManager.AddTween(tween, _chatScreen.handle);
        }

        public void Dispose()
        {
            DestroyImmediate(_chatScreen);
            _chatScreen = null;
        }
    }
}
