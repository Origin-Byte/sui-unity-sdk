using Suinet.Rpc.Types.MoveTypes;

namespace Suinet.NftProtocol.Domains
{
    [MoveType("0x[a-f0-9]{40}::display::DisplayDomain")]
    public class DisplayDomain : DomainBase
    {
        public string Description { get; set; }

        public string Name { get; set; }

    }
}
