using System.Threading.Tasks;
using RestSharp;

namespace discordtf2updates
{
    public class ApiHandler
    {
        public async Task<Updates> GetAppNewsAsync()
        {
            var client = new RestClient(Program.config.SteamWebApiUri);

            var request = new RestRequest(Program.config.NewsEndpoint);

            request.AddHeader("token", Program.config.SteamToken);

            request.AddParameter("appid", Program.config.AppId);
            request.AddParameter("feeds", Program.config.Feeds);
            request.AddParameter("count", 1);

            var response = await client.GetAsync<Updates>(request);

            return response;
        }
    }
}