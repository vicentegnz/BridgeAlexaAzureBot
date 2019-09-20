using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BridgeAlexaAzureBot.Models;
using Newtonsoft.Json;

namespace BridgeAlexaAzureBot.Infrastructure
{
    public class DirectLineClientService
    {
        private const string host = "https://directline.botframework.com";

        private static string conversationsAPI = $"{host}/api/conversations";

        private static string botSecret = "ba-_wDa9llY.cwA.0R0._YA0nSzX5BY0UUa7hGsTEjaghnWatjag2Gj7c06S6wY";

        private static string fromUser = "DirectLineClientSampleUser";


        public async Task<StartConversationResponse> StartConversationAsync()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("BotConnector", botSecret);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await httpClient.PostAsync(conversationsAPI, null);

                response.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<StartConversationResponse>(await response.Content.ReadAsStringAsync());
            }
        }

        public async Task SendMessageAsync(string conversationId, string text)
        {
            string url = $"{host}/api/conversations/{conversationId}/messages";

            var message = new
            {
                fromUser,
                text
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("BotConnector", botSecret); 
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await httpClient.PostAsync(url, content);

                if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
                {
                    response.EnsureSuccessStatusCode();
                }
            }
        }

        public async Task<ConversationMessages> GetMessagesAsync(string conversationId, string watermark)
        {
            string url = $"{host}/api/conversations/{conversationId}/messages?watermark={watermark}";

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("BotConnector", botSecret);
                var response = await httpClient.GetStringAsync(url);

                return JsonConvert.DeserializeObject<ConversationMessages>(response);
            }
        }   
    }
}