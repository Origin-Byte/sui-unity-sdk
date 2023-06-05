using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Numerics;

namespace Suinet.Rpc.Types
{
    public class TransactionFilterConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(TransactionFilter));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            TransactionFilter filter;

            if (jo["Checkpoint"] != null)
                filter = new CheckpointFilter { Checkpoint = jo["Checkpoint"].Value<BigInteger>() };
            else if (jo["MoveFunction"] != null)
                filter = new MoveFunctionFilter
                {
                    MoveFunction = new MoveFunction
                    {
                        Function = (string)jo["MoveFunction"]["function"],
                        Module = (string)jo["MoveFunction"]["module"],
                        Package = (string)jo["MoveFunction"]["package"]
                    }
                };
            else if (jo["InputObject"] != null)
                filter = new InputObjectFilter { InputObject = (string)jo["InputObject"] };
            else if (jo["ChangedObject"] != null)
                filter = new ChangedObjectFilter { ChangedObject = (string)jo["ChangedObject"] };
            else if (jo["FromAddress"] != null)
                filter = new FromAddressFilter { FromAddress = (string)jo["FromAddress"] };
            else if (jo["ToAddress"] != null)
                filter = new ToAddressFilter { ToAddress = (string)jo["ToAddress"] };
            else if (jo["FromAndToAddress"] != null)
                filter = new FromAndToAddressFilter
                {
                    FromAndToAddress = new FromAndToAddress
                    {
                        From = (string)jo["FromAndToAddress"]["from"],
                        To = (string)jo["FromAndToAddress"]["to"]
                    }
                };
            else if (jo["TransactionKind"] != null)
                filter = new TransactionKindFilter { TransactionKind = (string)jo["TransactionKind"] };
            else
                throw new JsonException();

            return filter;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is CheckpointFilter checkpointFilter)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Checkpoint");
                serializer.Serialize(writer, checkpointFilter.Checkpoint);
                writer.WriteEndObject();
            }
            else if (value is MoveFunctionFilter moveFunctionFilter)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("MoveFunction");
                serializer.Serialize(writer, moveFunctionFilter.MoveFunction);
                writer.WriteEndObject();
            }
            else if (value is InputObjectFilter inputObjectFilter)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("InputObject");
                serializer.Serialize(writer, inputObjectFilter.InputObject);
                writer.WriteEndObject();
            }
            else if (value is ChangedObjectFilter changedObjectFilter)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("ChangedObject");
                serializer.Serialize(writer, changedObjectFilter.ChangedObject);
                writer.WriteEndObject();
            }
            else if (value is FromAddressFilter fromAddressFilter)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("FromAddress");
                serializer.Serialize(writer, fromAddressFilter.FromAddress);
                writer.WriteEndObject();
            }
            else if (value is ToAddressFilter toAddressFilter)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("ToAddress");
                serializer.Serialize(writer, toAddressFilter.ToAddress);
                writer.WriteEndObject();
            }
            else if (value is FromAndToAddressFilter fromAndToAddressFilter)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("FromAndToAddress");
                serializer.Serialize(writer, fromAndToAddressFilter.FromAndToAddress);
                writer.WriteEndObject();
            }
            else if (value is TransactionKindFilter transactionKindFilter)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("TransactionKind");
                serializer.Serialize(writer, transactionKindFilter.TransactionKind);
                writer.WriteEndObject();
            }
            else
            {
                throw new Exception("Unknown subclass of TransactionFilter.");
            }
        }

    }

}
