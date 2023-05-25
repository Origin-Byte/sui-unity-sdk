using Suinet.Rpc.Types.MoveTypes;
using System.Linq;
using System.Numerics;

namespace Suinet.Rpc.Types
{
    public static class ObjectDataFilterFactory
    {
        public static MatchAllFilter CreateMatchAllFilter(params ObjectDataFilter[] filters)
        {
            return new MatchAllFilter { MatchAll = filters.ToList() };
        }

        public static MatchAnyFilter CreateMatchAnyFilter(params ObjectDataFilter[] filters)
        {
            return new MatchAnyFilter { MatchAny = filters.ToList() };
        }

        public static MatchNoneFilter CreateMatchNoneFilter(params ObjectDataFilter[] filters)
        {
            return new MatchNoneFilter { MatchNone = filters.ToList() };
        }

        public static PackageFilter CreatePackageFilter(ObjectId objectId)
        {
            return new PackageFilter { Package = objectId };
        }

        public static MoveModuleFilter CreateMoveModuleFilter(MoveModule moveModule)
        {
            return new MoveModuleFilter { MoveModule = moveModule };
        }

        public static StructTypeFilter CreateStructTypeFilter(string structType)
        {
            return new StructTypeFilter { StructType = structType };
        }

        public static AddressOwnerFilter CreateAddressOwnerFilter(string addressOwner)
        {
            return new AddressOwnerFilter { AddressOwner = addressOwner };
        }

        public static ObjectOwnerFilter CreateObjectOwnerFilter(ObjectId objectOwner)
        {
            return new ObjectOwnerFilter { ObjectOwner = objectOwner };
        }

        public static ObjectIdFilter CreateObjectIdFilter(ObjectId objectId)
        {
            return new ObjectIdFilter { ObjectId = objectId };
        }

        public static ObjectIdsFilter CreateObjectIdsFilter(params ObjectId[] objectIds)
        {
            return new ObjectIdsFilter { ObjectIds = objectIds.ToList() };
        }

        public static VersionFilter CreateVersionFilter(BigInteger version)
        {
            return new VersionFilter { Version = version };
        }
    }
}
