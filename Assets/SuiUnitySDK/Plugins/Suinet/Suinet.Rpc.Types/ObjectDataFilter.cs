using Suinet.Rpc.Types.MoveTypes;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;

namespace Suinet.Rpc.Types
{
    public abstract class ObjectDataFilter
    {
    }

    public class MatchAllFilter : ObjectDataFilter
    {
        public List<ObjectDataFilter> MatchAll { get; set; }
    }

    public class MatchAnyFilter : ObjectDataFilter
    {
        public List<ObjectDataFilter> MatchAny { get; set; }
    }

    public class MatchNoneFilter : ObjectDataFilter
    {
        public List<ObjectDataFilter> MatchNone { get; set; }
    }

    public class PackageFilter : ObjectDataFilter
    {
        [Description("Query by type a specified Package.")]
        public ObjectId Package { get; set; }
    }

    public class MoveModuleFilter : ObjectDataFilter
    {
        [Description("Query by type a specified Move module.")]
        public MoveModule MoveModule { get; set; }
    }

    public class StructTypeFilter : ObjectDataFilter
    {
        [Description("Query by type")]
        public string StructType { get; set; }
    }

    public class AddressOwnerFilter : ObjectDataFilter
    {
        public string AddressOwner { get; set; }
    }

    public class ObjectOwnerFilter : ObjectDataFilter
    {
        public ObjectId ObjectOwner { get; set; }
    }

    public class ObjectIdFilter : ObjectDataFilter
    {
        public ObjectId ObjectId { get; set; }
    }

    public class ObjectIdsFilter : ObjectDataFilter
    {
        public List<ObjectId> ObjectIds { get; set; }
    }

    public class VersionFilter : ObjectDataFilter
    {
        public BigInteger Version { get; set; }
    }

}
