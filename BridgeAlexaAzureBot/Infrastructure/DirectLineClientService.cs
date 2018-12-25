
using BridgeAlexaAzureBot.Models;
using Microsoft.Bot.Connector.DirectLine;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BridgeAlexaAzureBot.Infrastructure
{
    public class DirectLineClientService
    {
        private static string directLineSecret = "ba-_wDa9llY.cwA.0R0._YA0nSzX5BY0UUa7hGsTEjaghnWatjag2Gj7c06S6wY";
        private static string botId = "nexo-bot";

        // This gives a name to the bot user.
        private static string fromUser = "DirectLineClientSampleUser";

        private Conversation conversation;
        private string watermark;
        
        public async Task<ChatModel> TalkToTheBot(string message)
        {
            DirectLineClient client = new DirectLineClient(directLineSecret);

            if (conversation == null)
            {
                // There is no existing conversation
                // start a new one
                conversation = await client.Conversations.StartConversationAsync();
            }

            if (message.Length > 0)
            {
                // Create a message activity with the text the user entered.
                Activity userMessage = new Activity
                {
                    From = new ChannelAccount(fromUser),
                    Text = message,
                    Type = ActivityTypes.Message
                };

                // Send the message activity to the bot.
                await client.Conversations.PostActivityAsync(conversation.ConversationId, userMessage);
            }
            ChatModel objChat = await ReadBotMessagesAsync(client, conversation.ConversationId, watermark);
            watermark = objChat.watermark;

            return objChat;
        }


        /// <summary>
        /// Polls the bot continuously and retrieves messages sent by the bot to the client.
        /// </summary>
        /// <param name="client">The Direct Line client.</param>
        /// <param name="conversationId">The conversation ID.</param>
        /// <returns></returns>
        public async Task<ChatModel> ReadBotMessagesAsync(DirectLineClient client, string conversationId, string watermark)
        {
            // Create an Instance of the Chat object
            ChatModel objChat = new ChatModel();
            // We want to keep waiting until a message is received
            bool messageReceived = false;

            while (!messageReceived)
            {
                // Retrieve the activity set from the bot.
                var activitySet = await client.Conversations.GetActivitiesAsync(conversationId, watermark);
                watermark = activitySet?.Watermark;

                // Extract the activies sent from our bot.
                var activities = from x in activitySet.Activities
                                 where x.From.Id == botId
                                 select x;


                // Analyze each activity in the activity set.
                foreach (Activity activity in activities)
                {
                    // We have Text
                    if (activity.Text != null)
                    {
                        // Set the text response
                        // to the message text
                        objChat.ChatResponse = activity.Text;
                    }
                }
                // Mark messageReceived so we can break 
                // out of the loop
                messageReceived = true;
            }
            // Set watermark on the Chat object that will be 
            // returned
            objChat.watermark = watermark;

            // Return a response as a Chat object
            return objChat;
        }
    }
}
