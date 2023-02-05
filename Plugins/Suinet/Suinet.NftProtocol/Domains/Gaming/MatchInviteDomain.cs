using Suinet.Rpc.Types.MoveTypes;

namespace Suinet.NftProtocol.Domains.Gaming
{
    [MoveType("0x2::dynamic_field::Field<0x[a-f0-9]{40}::utils::Marker<.*>, 0x[a-f0-9]{40}::gaming::MatchInviteDomain>")]
    public class MatchInviteDomain : DomainBase
    {
        public string MatchId
        {
            get
            {
                return null;
            }
        }
    }
}
