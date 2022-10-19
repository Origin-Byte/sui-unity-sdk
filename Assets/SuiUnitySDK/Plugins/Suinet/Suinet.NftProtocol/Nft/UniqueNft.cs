using Newtonsoft.Json;
using Suinet.Rpc.Types.MoveTypes;
using System.Collections.Generic;

namespace Suinet.NftProtocol.Nft
{
    [MoveType("0x[a-f0-9]{40}::nft::Nft<.*, 0x[a-f0-9]{40}::unique_nft::Unique>")]
    public class UniqueNft : INft<UniqueNftData>
    {
        public UID Id { get; set; }

        public string DataId { get; set; }

        public UniqueNftData Data { get; set; }  
    }

    public class UniqueNftData
    {
        public string Type { get; set; }

        public UniqueNftDataFields Fields { get; set; }       
    }

    public class UniqueNftDataFields
    {
        public Dictionary<string, object> Attributes { get; set; }

        [JsonProperty("collection_id")]
        public string CollectionId { get; set; }

        public string Description { get; set; }

        public UID Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }
    }
}
