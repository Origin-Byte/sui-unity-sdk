using Suinet.Rpc.Types;
using Suinet.Rpc.Types.MoveTypes;
using System.Collections.Generic;

namespace Suinet.NftProtocol.Domains
{
    [MoveType("0x2::dynamic_field::Field<0x[a-f0-9]{40}::utils::Marker<.*>, 0x[a-f0-9]{40}::display::AttributesDomain>")]
    public class AttributesDomain : DomainBase
    {
        public Dictionary<string, string> Attributes
        {
            get
            {
                return Value.Fields.ToObject<AttributesMap>().Map.ToDictionary();
            }
        }
    }

    public class AttributesMap
    {
        public VecMap<string, string> Map { get; set; }
    }
}
