﻿using Newtonsoft.Json;

namespace BridgeAlexaAzureBot.Models
{
    public class Attachement
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("contentType")]
        public string ContentType { get; set; }
    }
}
