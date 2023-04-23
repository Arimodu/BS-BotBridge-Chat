using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components.Settings;
using BeatSaberMarkupLanguage.ViewControllers;
using BSBBChat.Configuration;
using IPA.Config.Data;
using SiraUtil.Logging;
using Zenject;

namespace BSBBChat.UI
{
    [HotReload(RelativePathToLayout = @"BSBBChatViewController.bsml")]
    [ViewDefinition("BS_BotBridge_Chat.UI.BSBBChatViewController.bsml")]
    internal class BSBBChatViewController : BSMLAutomaticViewController
    {
        private SiraLog _logger;
        private BSBBChatConfig _config;

        public bool MenuScreenEnabled
        {
            get => _config.MenuScreenEnabled;
            set
            {
                if (_config.MenuScreenEnabled == value) return;
                _config.MenuScreenEnabled = value;
                NotifyPropertyChanged(nameof(MenuScreenEnabled));
            }
        }
        public bool GameScreenEnabled
        {
            get => _config.GameScreenEnabled;
            set
            {
                if (_config.GameScreenEnabled == value) return;
                _config.GameScreenEnabled = value;
                NotifyPropertyChanged(nameof(GameScreenEnabled));
            }
        }
        public bool PauseScreenEnabled
        {
            get => _config.PauseScreenEnabled;
            set
            {
                if (_config.PauseScreenEnabled == value) return;
                _config.PauseScreenEnabled = value;
                NotifyPropertyChanged(nameof(PauseScreenEnabled));
            }
        }
        public bool DifferentGameScreenPosition
        {
            get => _config.DifferentGameScreenPosition;
            set
            {
                if (_config.DifferentGameScreenPosition == value) return;
                _config.DifferentGameScreenPosition = value;
                NotifyPropertyChanged(nameof(DifferentGameScreenPosition));
            }
        }
        public bool DifferentPauseScreenPosition
        {
            get => _config.DifferentPauseScreenPosition;
            set
            {
                if (_config.DifferentPauseScreenPosition == value) return;
                _config.DifferentPauseScreenPosition = value;
                NotifyPropertyChanged(nameof(DifferentPauseScreenPosition));
            }
        }
        public bool HandleEnabled
        {
            get => _config.HandleEnabled;
            set
            {
                if (_config.HandleEnabled == value) return;
                _config.HandleEnabled = value;
                NotifyPropertyChanged(nameof(HandleEnabled));
            }
        }
        public bool HandleWholeScreen
        {
            get => _config.HandleWholeScreen;
            set
            {
                if (_config.HandleWholeScreen == value) return;
                _config.HandleWholeScreen = value;
                NotifyPropertyChanged(nameof(HandleWholeScreen));
            }
        }
        public bool ReverseChatOrder
        {
            get => _config.ReverseChatOrder;
            set
            {
                if (_config.ReverseChatOrder == value) return;
                _config.ReverseChatOrder = value;
                NotifyPropertyChanged(nameof(ReverseChatOrder));
            }
        }
        public float ScreenWidth
        {
            get => _config.ScreenWidth;
            set
            {
                if (_config.ScreenWidth == value) return;
                _config.ScreenWidth = value;
                NotifyPropertyChanged(nameof(ScreenWidth));
            }
        }
        public float ScreenHeight
        {
            get => _config.ScreenHeight;
            set
            {
                if (_config.ScreenHeight == value) return;
                _config.ScreenHeight = value;
                NotifyPropertyChanged(nameof(ScreenHeight));
            }
        }

        [Inject]
        internal void InjectDependencies(BSBBChatConfig config, SiraLog logger)
        {
            _logger = logger;
            _config = config;
        }

        private void BSBBChatViewController_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(MenuScreenEnabled):
                    UpdateInteractivity();
                    break;
                case nameof(GameScreenEnabled):
                    UpdateInteractivity();
                    break;
                case nameof(PauseScreenEnabled):
                    PauseScreenPositionToggle.interactable = PauseScreenEnabled;
                    break;
                case nameof(DifferentGameScreenPosition): 
                    // Nothing here
                    break;
                case nameof(DifferentPauseScreenPosition): 
                    // Nothing here
                    break;
                case nameof(HandleEnabled):
                    HandleWholeScreenToggle.interactable = HandleEnabled;
                    break;
                case nameof(HandleWholeScreen): 
                    // Nothing here
                    break;
                case nameof(ReverseChatOrder): 
                    // Nothing here
                    break;
                case nameof(ScreenWidth): 
                    // Nothing here
                    break;
                case nameof(ScreenHeight): 
                    // Nothing here
                    break;
                default:
                    _logger.Warn("Unknown property value: " + e.PropertyName);
                    return;
            }

            // Update config no matter the type of change
            // Except when its an unknown property
            _config.Changed();
        }

        private void UpdateInteractivity()
        {
            if (!MenuScreenEnabled && !GameScreenEnabled && !PauseScreenEnabled)
            {
                GameScreenPositionToggle.interactable = false;
                PauseScreenPositionToggle.interactable = false;
                HandleEnabledToggle.interactable = false;
                HandleWholeScreenToggle.interactable = false;
                ReverseChatOrderToggle.interactable = false;
                ScreenWidthIncrementSetting.interactable = false;
                ScreenHeightIncrementSetting.interactable = false;
            }
            else
            {
                GameScreenPositionToggle.interactable = GameScreenEnabled;
                PauseScreenEnabledToggle.interactable = !GameScreenEnabled;
                PauseScreenPositionToggle.interactable = GameScreenEnabled || (!GameScreenEnabled && PauseScreenEnabled);

                HandleEnabledToggle.interactable = true;
                ReverseChatOrderToggle.interactable = true;
                ScreenWidthIncrementSetting.interactable = true;
                ScreenHeightIncrementSetting.interactable = true;
            }
        }

        [UIAction("#post-parse")]
        internal void PostParse()
        {
            PropertyChanged += BSBBChatViewController_PropertyChanged;

            HandleWholeScreenToggle.interactable = HandleEnabled;
            ScreenWidthIncrementSetting.interactable = !HandleWholeScreen;
            ScreenHeightIncrementSetting.interactable = !HandleWholeScreen;

            UpdateInteractivity();
        }

#pragma warning disable IDE0044 // Add readonly modifier

        [UIComponent("DifferentGameScreenPosition")]
        private ToggleSetting GameScreenPositionToggle;

        [UIComponent("PauseScreenEnabled")]
        private ToggleSetting PauseScreenEnabledToggle;

        [UIComponent("DifferentPauseScreenPosition")]
        private ToggleSetting PauseScreenPositionToggle;

        [UIComponent("HandleEnabled")]
        private ToggleSetting HandleEnabledToggle;

        [UIComponent("HandleWholeScreen")]
        private ToggleSetting HandleWholeScreenToggle;

        [UIComponent("ReverseChatOrder")]
        private ToggleSetting ReverseChatOrderToggle;

        [UIComponent("ScreenWidth")]
        private IncrementSetting ScreenWidthIncrementSetting;

        [UIComponent("ScreenHeight")]
        private IncrementSetting ScreenHeightIncrementSetting;
    }
}
