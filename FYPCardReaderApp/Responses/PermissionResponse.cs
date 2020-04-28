using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FYPCardReaderApp.Responses
{
    public class PermissionResponse
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }
        
        public string[] Locations { get; set; }
    }
}
