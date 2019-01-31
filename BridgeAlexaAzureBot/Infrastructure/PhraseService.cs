
using BridgeAlexaAzureBot.Core;
using BridgeAlexaAzureBot.Models;
using System;
using System.Threading.Tasks;

namespace BridgeAlexaAzureBot.Infrastructure
{
    public class PhraseService : IPhraseService
    {
        private readonly DirectLineClientService directLineClientService;

        public PhraseService()
        {
            directLineClientService = new DirectLineClientService();
        }

        public async Task<PhraseServiceModel> GetPhrase(string message)
        {
    
            PhraseServiceModel phraseResponse =  new PhraseServiceModel { Content = string.Empty };

            try { 
                StartConversationResponse conversationStart = await directLineClientService.StartConversationAsync();

                await directLineClientService.SendMessageAsync(conversationStart.ConversationId, message);

                ConversationMessages conversationMessages = await  directLineClientService.GetMessagesAsync(conversationStart.ConversationId, "");

                foreach (Message messageFromBot in conversationMessages.Messages)
                {
                    if (messageFromBot.Text != null)
                    {
                        phraseResponse.Content = messageFromBot.Text;
                    }
                }
            }
            catch (Exception e)
            {
                phraseResponse.Content = "¿No le he entendido, por favor podría repetir la pregunta?";
            }

            return phraseResponse;
        }
    }
}
