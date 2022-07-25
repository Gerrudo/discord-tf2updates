using System;
using System.Threading;
using System.Threading.Tasks;

namespace discordtf2updates
{
    public class CheckUpdates
    {
        public async Task<Updates> InitalUpdates(ApiHandler ApiHandler)
        {
            CustomConsole.CustomWriteLine("Getting initial updates from Steam...");

            return await ApiHandler.GetAppNewsAsync();
        }

        public async Task<Updates> CheckForUpdatesAsync(ApiHandler apiHandler, Updates storedUpdates)
        {
            CustomConsole.CustomWriteLine("Checking Steam for updates...");

            var updates = await apiHandler.GetAppNewsAsync();

            if (updates.appnews.newsitems[0].date > storedUpdates.appnews.newsitems[0].date)
            {
                CustomConsole.CustomWriteLine("Updates found, posting to Discord.");

                var _embedUpdates = new EmbedUpdates();
                var embed = _embedUpdates.BuildTF2Embed(updates.appnews.newsitems[0]);

                await DiscordBot.PostUpdatesAsync(embed);
                return updates;
            }
            else
            {
                CustomConsole.CustomWriteLine("No updates.");
                return updates;
            }
        }
    }
}