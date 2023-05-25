using Newtonsoft.Json;
using Suinet.Rpc.Types.MoveTypes;
using System.Collections.Generic;

namespace Suinet.Rpc.Types.Nfts
{
    public class CapySuiFren
    {
        public string ObjectId { get; set; }

        public string Type { get; set; }

        public DisplayData Display { get; set; }

        public CapySuiFrenProperties Properties { get; set; }

        public List<SuiFrenAccessory> Accessories { get; set; }
    }

    public class CapySuiFrenProperties
    {
        [JsonProperty("attributes")]
        public List<string> Attributes { get; set; }

        [JsonProperty("birth_location")]
        public string BirthLocation { get; set; }

        [JsonProperty("birthdate")]
        public string Birthdate { get; set; }

        [JsonProperty("cohort")]
        public int Cohort { get; set; }

        [JsonProperty("generation")]
        public string Generation { get; set; }

        [JsonProperty("genes")]
        public List<int> Genes { get; set; }
    }

    public class SuiFrenAccessory
    {
        public string Name { get; set; }
        public UID Id { get; set; }
        public string Type { get; set; }
    }
}
