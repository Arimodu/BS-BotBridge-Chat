using System;
using System.Runtime.CompilerServices;
using BS_BotBridge_Chat.Converters;
using IPA.Config.Stores;
using IPA.Config.Stores.Attributes;
using UnityEngine;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace BS_BotBridge_Chat.Configuration
{
    public class BSBBChatConfig
    {
        public event Action OnChanged;
        public bool Enabled = false;

        public bool EnableHandle = true;

        // Just some arbitrary numbers I pulled out of my ass, probably not great defaults tho
        public float ScreenWidth = 280;
        public float ScreenHeight = 320;

        [UseConverter(typeof(Vector3Converter))]
        public Vector3 MenuChatPosition { get; set; } = new Vector3(0, 3.75f, 2.5f);
        [UseConverter(typeof(Vector3Converter))]
        public Vector3 MenuChatRotation { get; set; } = new Vector3(325, 0, 0);
        [UseConverter(typeof(Vector3Converter))]
        public Vector3 GameChatPosition { get; set; } = new Vector3(0, 3.75f, 2.5f);
        [UseConverter(typeof(Vector3Converter))]
        public Vector3 GameChatRotation { get; set; } = new Vector3(325, 0, 0);

        /// <summary>
        /// This is called whenever BSIPA reads the config from disk (including when file changes are detected).
        /// </summary>
        public virtual void OnReload()
        {
            // Do stuff after config is read from disk.
        }

        /// <summary>
        /// Call this to force BSIPA to update the config file. This is also called by BSIPA if it detects the file was modified.
        /// </summary>
        public virtual void Changed()
        {
            OnChanged?.Invoke();
        }

        /// <summary>
        /// Call this to have BSIPA copy the values from <paramref name="other"/> into this config.
        /// </summary>
        public virtual void CopyFrom(BSBBChatConfig other)
        {
            // This instance's members populated from other
        }
    }
}
