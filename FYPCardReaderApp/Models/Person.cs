using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FYPCardReaderApp.Models
{
    public class Person
    {
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string LocationName { get; set; }
        public string Id { get; set; }

        [JsonProperty("primaryLocation")]
        public bool LocationIsPrimary { get; set; }
    }
}
