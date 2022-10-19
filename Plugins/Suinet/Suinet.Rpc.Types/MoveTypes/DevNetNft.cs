namespace Suinet.Rpc.Types.MoveTypes
{
    [MoveType("0x2::devnet_nft::DevNetNFT")]
    public class DevNetNft
    {
        public UID Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }

    }
}
