using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS_BotBridge_Chat.Packets
{
    public class Enums
    {
        public enum ChatPacketType
        {
            TwitchMessage,
            YoutubeMessage,
            TwitchEvent,
            YoutubeEvent,
        }
    }
}
