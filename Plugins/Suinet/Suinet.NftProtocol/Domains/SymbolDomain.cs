using Suinet.Rpc.Types.MoveTypes;

namespace Suinet.NftProtocol.Domains
{
    [MoveType("0x2::dynamic_field::Field<0x[a-f0-9]{40}::utils::Marker<.*>, 0x[a-f0-9]{40}::display::SymbolDomain>")]
    public class SymbolDomain : DomainBase
    {
        public string Symbol
        {
            get
            {
                return GetTypedField<string>("symbol");
            }
        }
    }
}
