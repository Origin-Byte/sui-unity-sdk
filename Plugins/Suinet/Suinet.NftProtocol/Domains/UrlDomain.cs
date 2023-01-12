using Suinet.Rpc.Types.MoveTypes;

namespace Suinet.NftProtocol.Domains
{
    [MoveType("0x2::dynamic_field::Field<0x[a-f0-9]{40}::utils::Marker<.*>, 0x[a-f0-9]{40}::display::UrlDomain>")]
    public class UrlDomain : DomainBase
    {
        public string Url
        {
            get
            {
                return GetTypedField<string>("url");
            }
        }
    }
}
