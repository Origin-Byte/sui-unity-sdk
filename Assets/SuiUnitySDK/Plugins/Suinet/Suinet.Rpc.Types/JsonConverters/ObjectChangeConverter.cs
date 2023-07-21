using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Suinet.Rpc.Types.JsonConverters
{
    public class ObjectChangeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(ObjectChange));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);

            switch (jo["type"]?.ToObject<ObjectChangeType>())
            {
                case ObjectChangeType.Published:
                    return new PublishedObjectChange
                    {
                        Digest = jo["digest"].Value<string>(),
                        Type = ObjectChangeType.Published,
                        Version = jo["version"].Value<string>(),
                        Modules = jo["modules"]?.ToObject<List<string>>(serializer),
                        PackageId = jo["packageId"].Value<string>()
                    };
                case ObjectChangeType.Transferred:
                    return new TransferredObjectChange
                    {
                        Digest = jo["digest"].Value<string>(),
                        Type = ObjectChangeType.Transferred,
                        Version = jo["version"].Value<string>(),
                        ObjectId = jo["objectId"].ToObject<ObjectId>(serializer),
                        ObjectType = jo["objectType"].Value<string>(),
                        Recipient = jo["recipient"].Value<string>(),
                        Sender = jo["sender"].Value<string>()
                    };
                case ObjectChangeType.Mutated:
                    return new MutatedObjectChange
                    {
                        Digest = jo["digest"].Value<string>(),
                        Type = ObjectChangeType.Mutated,
                        Version = jo["version"].Value<string>(),
                        ObjectId = jo["objectId"].ToObject<ObjectId>(serializer),
                        ObjectType = jo["objectType"].Value<string>(),
                        Owner = jo["owner"].ToObject<Owner>(serializer),
                        PreviousVersion = BigInteger.Parse(jo["previousVersion"].Value<string>()),
                        Sender = jo["sender"].Value<string>()
                    };
                case ObjectChangeType.Deleted:
                    return new DeletedObjectChange
                    {
                        Digest = jo["digest"].Value<string>(),
                        Type = ObjectChangeType.Deleted,
                        Version = jo["version"].Value<string>(),
                        ObjectId = jo["objectId"].ToObject<ObjectId>(serializer),
                        ObjectType = jo["objectType"].Value<string>(),
                        Sender = jo["sender"].Value<string>()
                    };
                case ObjectChangeType.Wrapped:
                    return new WrappedObjectChange
                    {
                        Digest = jo["digest"].Value<string>(),
                        Type = ObjectChangeType.Wrapped,
                        Version = jo["version"].Value<string>(),
                        ObjectId = jo["objectId"].ToObject<ObjectId>(serializer),
                        ObjectType = jo["objectType"].Value<string>(),
                        Sender = jo["sender"].Value<string>()
                    };
                case ObjectChangeType.Created:
                    return new CreatedObjectChange
                    {
                        Digest = jo["digest"].Value<string>(),
                        Type = ObjectChangeType.Created,
                        Version = jo["version"].Value<string>(),
                        ObjectId = jo["objectId"].ToObject<ObjectId>(serializer),
                        ObjectType = jo["objectType"].Value<string>(),
                        Owner = jo["owner"].ToObject<Owner>(serializer),
                        Sender = jo["sender"].Value<string>()
                    };
                default:
                    throw new InvalidOperationException("Type field not found.");
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
        }
    }

}
