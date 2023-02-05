using Suinet.Rpc.Types.MoveTypes;

namespace Suinet.NftProtocol.Domains
{
    [MoveType("0x[a-f0-9]{40}::display::SymbolDomain")]
    public class SymbolDomain : DomainBase
    {
        public string Symbol { get; set; }
    }
}
