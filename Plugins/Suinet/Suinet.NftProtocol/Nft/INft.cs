using Newtonsoft.Json;
using Suinet.Rpc.Types.MoveTypes;

namespace Suinet.NftProtocol.Nft
{
    public interface INft<T>
    {
        UID Id { get; }

        [JsonProperty("data_id")]
        string DataId { get; }

        T Data { get; }
    }
}
