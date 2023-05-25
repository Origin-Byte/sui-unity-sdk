using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Suinet.Rpc.Types.JsonConverters;
using System.Collections.Generic;
using System.Numerics;

namespace Suinet.Rpc.Types
{
    [JsonConverter(typeof(StringEnumConverter), converterParameters: typeof(CamelCaseNamingStrategy))]
    public enum ObjectChangeType
    {
        Published,
        Transferred,
        Mutated,
        Deleted,
        Wrapped,
        Created
    }

    [JsonConverter(typeof(ObjectChangeConverter))]
    public abstract class ObjectChange
    {
        public string Digest { get; set; }
        public ObjectChangeType Type { get; set; }
        public string Version { get; set; }
    }

    public class PublishedObjectChange : ObjectChange
    {
        public List<string> Modules { get; set; }
        public string PackageId { get; set; }

        public PublishedObjectChange()
        {
            Type = ObjectChangeType.Published;
        }
    }

    public class TransferredObjectChange : ObjectChange
    {
        public ObjectId ObjectId { get; set; }
        public string ObjectType { get; set; }
        public string Recipient { get; set; }
        public string Sender { get; set; }

        public TransferredObjectChange()
        {
            Type = ObjectChangeType.Transferred;
        }
    }

    public class MutatedObjectChange : ObjectChange
    {
        public ObjectId ObjectId { get; set; }
        public string ObjectType { get; set; }
        public Owner Owner { get; set; }
        public BigInteger PreviousVersion { get; set; }
        public string Sender { get; set; }

        public MutatedObjectChange()
        {
            Type = ObjectChangeType.Mutated;
        }
    }

    public class DeletedObjectChange : ObjectChange
    {
        public ObjectId ObjectId { get; set; }
        public string ObjectType { get; set; }
        public string Sender { get; set; }

        public DeletedObjectChange()
        {
            Type = ObjectChangeType.Deleted;
        }
    }

    public class WrappedObjectChange : ObjectChange
    {
        public ObjectId ObjectId { get; set; }
        public string ObjectType { get; set; }
        public string Sender { get; set; }

        public WrappedObjectChange()
        {
            Type = ObjectChangeType.Wrapped;
        }
    }

    public class CreatedObjectChange : ObjectChange
    {
        public ObjectId ObjectId { get; set; }
        public string ObjectType { get; set; }
        public Owner Owner { get; set; }
        public string Sender { get; set; }

        public CreatedObjectChange()
        {
            Type = ObjectChangeType.Created;
        }
    }
}
