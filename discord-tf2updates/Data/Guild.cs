using Discord;
using Discord.WebSocket;

namespace discordtf2updates
{
    public class Guild
    {
        public SocketMessage Message { get; set; }
        public IMessageChannel Channel { get; set; }
    }
}