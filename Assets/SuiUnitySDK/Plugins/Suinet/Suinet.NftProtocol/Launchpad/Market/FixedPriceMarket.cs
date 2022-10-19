using Suinet.Rpc.Types.MoveTypes;

namespace Suinet.NftProtocol.Launchpad.Market
{
    [MoveType("(0x[a-f0-9]{40})::slingshot::Slingshot<0x[a-f0-9]{40}::([a-zA-Z_]{1,})::([a-zA-Z_]{1,}), 0x[a-f0-9]{40}::fixed_price::FixedPriceMarket>")]
    public class FixedPriceMarket
    {
        public string Admin { get; set; }

        public string Receiver { get; set; }

        public bool IsEmbedded { get; set; }

        public bool Live { get; set; }

        public UID Id { get; set; }

        public ulong Price { get; set; }
    }
}
