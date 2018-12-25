using BridgeAlexaAzureBot.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BridgeAlexaAzureBot.Core
{
    public interface IPhraseService
    {
        Task<PhraseServiceModel> GetPhrase(string Message);
    }
}
