using System;
using System.Runtime.CompilerServices;
using BSBBChat.Converters;
using IPA.Config.Stores;
using IPA.Config.Stores.Attributes;
using UnityEngine;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace BSBBChat.Configuration
{
    public class BSBBChatConfig
    {
        public event Action OnChanged;
        public bool MenuScreenEnabled = true;
        public bool GameScreenEnabled = true;
        public bool DifferentGameScreenPosition = false;
        public bool PauseScreenEnabled = true;
        public bool DifferentPauseScreenPosition = false;

        public bool HandleEnabled = true;
        public bool HandleWholeScreen = false;

        public bool ReverseChatOrder = false;

        public float ScreenWidth = 210;
        public float ScreenHeight = 260;

        [UseConverter(typeof(Vector3Converter))]
        public Vector3 MenuChatPosition { get; set; } = new Vector3(0, 3.75f, 2.5f);
        [UseConverter(typeof(Vector3Converter))]
        public Vector3 MenuChatRotation { get; set; } = new Vector3(325, 0, 0);
        [UseConverter(typeof(Vector3Converter))]
        public Vector3 GameChatPosition { get; set; } = new Vector3(0, 3.75f, 2.5f);
        [UseConverter(typeof(Vector3Converter))]
        public Vector3 GameChatRotation { get; set; } = new Vector3(325, 0, 0);
        [UseConverter(typeof(Vector3Converter))]
        public Vector3 PauseChatPosition { get; set; } = new Vector3(0, 3.75f, 2.5f);
        [UseConverter(typeof(Vector3Converter))]
        public Vector3 PauseChatRotation { get; set; } = new Vector3(325, 0, 0);

        /// <summary>
        /// Call this to force BSIPA to update the config file. This is also called by BSIPA if it detects the file was modified.
        /// </summary>
        public virtual void Changed(bool broadcastChange = true) 
        {
            if (broadcastChange) 
                OnChanged?.Invoke();
        }
    }
}
