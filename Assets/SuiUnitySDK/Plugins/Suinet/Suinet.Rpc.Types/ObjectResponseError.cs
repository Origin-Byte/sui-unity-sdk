using Newtonsoft.Json;
using Suinet.Rpc.Types.JsonConverters;
using System.Numerics;

namespace Suinet.Rpc.Types
{
    [JsonConverter(typeof(ObjectResponseErrorJsonConverter))]
    public abstract class ObjectResponseError
    {
        public string Code { get; set; }
    }

    public class NotExistsError : ObjectResponseError
    {
        public ObjectId ObjectId { get; set; }
    }

    public class DynamicFieldNotFoundError : ObjectResponseError
    {
        public ObjectId ParentObjectId { get; set; }
    }

    public class DeletedError : ObjectResponseError
    {
        public string Digest { get; set; }
        public ObjectId ObjectId { get; set; }
        public BigInteger Version { get; set; }
    }

    public class UnknownError : ObjectResponseError
    {
    }

    public class DisplayError : ObjectResponseError
    {
        public string Error { get; set; }
    }

}
