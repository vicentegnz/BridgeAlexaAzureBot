using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using BridgeAlexaAzureBot.Core;
using BridgeAlexaAzureBot.Infrastructure;
using BridgeAlexaAzureBot.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace BridgeAlexaAzureBot
{
    public class Function
    {

        private readonly IPhraseService phraseService = new PhraseService();

        public async Task<SkillResponse> FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            SkillResponse response = new SkillResponse();
            response.Response = new ResponseBody();
            response.Response.ShouldEndSession = false;
            IOutputSpeech innerResponse = null;
            var log = context.Logger;

            if (input.GetRequestType() == typeof(LaunchRequest))
            {
                log.LogLine($"Default LaunchRequest made");
                innerResponse = new PlainTextOutputSpeech();
                (innerResponse as PlainTextOutputSpeech).Text = "Bienvenido a la Universidad de Extremadura, ¿En que te puedo ayudar?";
            }
            else if (input.GetRequestType() == typeof(IntentRequest))
            {
                var intentRequest = (IntentRequest)input.Request;

                switch (intentRequest.Intent.Name)
                {
                    case "AMAZON.CancelIntent":
                    case "AMAZON.StopIntent":
                        log.LogLine($"AMAZON.CancelIntent: enviando mensaje de cancelación.");
                        innerResponse = new PlainTextOutputSpeech();
                        (innerResponse as PlainTextOutputSpeech).Text = "Adios, espero haberte sido de utilidad.";
                        response.Response.ShouldEndSession = true;
                        break;
                    case "AMAZON.HelpIntent":
                        log.LogLine($"AMAZON.HelpIntent: enviando mensaje de ayuda");
                        innerResponse = new PlainTextOutputSpeech();
                        (innerResponse as PlainTextOutputSpeech).Text = "Para pedir un frase solo tienes que decir, dime una frase o cuentame una frase.";
                        break;
                    case "GetUserIntent":
                        log.LogLine($"Enviar Mensaje al Bot de Azure");
                        PhraseServiceModel phrase = new PhraseServiceModel { Content = string.Empty };
                        if (intentRequest.Intent.Slots["phrase"].Value != null)
                        {
                            phrase = await phraseService.GetPhrase(intentRequest.Intent.Slots["phrase"].Value.ToString());
                            innerResponse = new PlainTextOutputSpeech();
                        }
                         (innerResponse as PlainTextOutputSpeech).Text = !String.IsNullOrEmpty(phrase.Content) ? phrase.Content : "En estos momentos Chuck Norris tiene problemas, inténtalo mas tarde o tendras una patada voladora.";
                        break;
                    default:
                        log.LogLine($"Unknown intent: " + intentRequest.Intent.Name);
                        innerResponse = new PlainTextOutputSpeech();
                        (innerResponse as PlainTextOutputSpeech).Text = "No entiendo lo que me estas pidiendo.";
                        break;
                }

            }
            response.Response.OutputSpeech = innerResponse;
            response.Version = "1.0";
            log.LogLine($"Skill Response Object...");
            log.LogLine(JsonConvert.SerializeObject(response));

            return response;
        }
    }
}
