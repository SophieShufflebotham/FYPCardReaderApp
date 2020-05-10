using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FYPCardReaderApp.Responses
{
    public class LocationsResponse
    {
        [JsonProperty("locationId")]
        public string Location { get; set; }
    }
}
