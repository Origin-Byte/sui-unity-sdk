using Newtonsoft.Json;
using System.Collections.Generic;

namespace Suinet.NftProtocol.Nft
{
    public class ArtNft : Nft
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        // TODO support attributes, parse correctly
        //[JsonProperty("attributes")]
       // public Dictionary<string,string> Attributes { get; set; }
    }
}
