using Suinet.Rpc.Types.MoveTypes;

namespace Suinet.NftProtocol.Nft
{
    // Base Nft type
    [MoveType("0x[a-f0-9]{40}::nft::Nft<.*>")]
    public class Nft : INft<object>
    {
        public UID Id { get; set; }

        public string DataId { get; set; }

        public object Data { get; set; }
    }
}
