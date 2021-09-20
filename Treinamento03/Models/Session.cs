using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;


namespace Treinamento03
{
    public class Session
    {
        [JsonProperty("odata.metadata")]
        public string OdataMetadata { get; set; }
        public string SessionId { get; set; }
        public string Version { get; set; }
        public int SessionTimeout { get; set; }
    }
}
