using Suinet.Rpc.Types.MoveTypes;

namespace Suinet.NftProtocol.Domains
{
    [MoveType("0x2::dynamic_field::Field<0x[a-f0-9]{40}::utils::Marker<.*>, 0x[a-f0-9]{40}::display::DisplayDomain>")]
    public class DisplayDomain : DomainBase
    {
        public string Description
        {
            get
            {
                return GetTypedField<string>("description");
            }
        }

        public string DisplayName
        {
            get
            {
                return GetTypedField<string>("name");
            }
        }

    }
}
