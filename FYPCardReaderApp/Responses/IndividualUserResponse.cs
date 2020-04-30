using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FYPCardReaderApp.Responses
{
    class IndividualUserResponse
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }
    }
}
