using Newtonsoft.Json;
using Suinet.Rpc.Types.MoveTypes;
using System.Collections.Generic;

namespace Suinet.NftProtocol.Launchpad
{
    public interface ISlingshot
    {
        UID Id { get; }

        [JsonProperty("collection_id")]
        string CollectionId { get; }

        bool Live { get; }

        string Admin { get; }

        string Receiver { get; }

        IEnumerable<Sale> Sales { get; }

        [JsonProperty("is_embedded")]
        bool IsEmbedded { get; }
    }
}
