
using BridgeAlexaAzureBot.Core;
using BridgeAlexaAzureBot.Models;
using System.Threading.Tasks;

namespace BridgeAlexaAzureBot.Infrastructure
{
    public class PhraseService : IPhraseService
    {
        private readonly DirectLineClientAPIService directLineClientAPIService;

        public PhraseService()
        {
            directLineClientAPIService = new DirectLineClientAPIService();
        }

        public async Task<PhraseServiceModel> GetPhrase(string message)
        {
    
            PhraseServiceModel phraseResponse =  new PhraseServiceModel { Content = string.Empty };

            StartConversationResponse conversationStart = await directLineClientAPIService.StartConversationAsync();

            await directLineClientAPIService.SendMessageAsync(conversationStart.ConversationId, message);

            ConversationMessages conversationMessages = await directLineClientAPIService.GetMessagesAsync(conversationStart.ConversationId, "");

            foreach (Message messageFromBot in conversationMessages.Messages)
            {
                if (messageFromBot.Text != null)
                {
                    phraseResponse.Content = messageFromBot.Text;
                }
            }

            return phraseResponse;
        }
    }
}
