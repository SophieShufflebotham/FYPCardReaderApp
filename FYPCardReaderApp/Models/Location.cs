using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FYPCardReaderApp.Models
{
    public class Location
    {
        [JsonProperty("Id")]
        public string LocationId { get; set; }
        public string LocationName { get; set; }
    }
}
