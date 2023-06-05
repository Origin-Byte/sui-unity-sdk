using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Suinet.Rpc.Types.MoveTypes;

namespace Suinet.Rpc.Types.JsonConverters
{
    public class SuiTransactionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(SuiTransaction));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);

            if (jo["MoveCall"] != null)
            {
                return new SuiMoveCallTransaction
                {
                    MoveCall = jo["MoveCall"].ToObject<SuiProgrammableMoveCall>()
                };
            }
            else if (jo["TransferObjects"] != null)
            {
                var transferObjectsTransaction = new SuiTransferObjectsTransaction
                {
                    TransferObjects = ConvertTransactionArguments(jo["TransferObjects"], serializer)
                };
                return transferObjectsTransaction;
            }
            else if (jo["SplitCoins"] != null)
            {
                var splitCoinsTransaction = new SplitCoinsTransaction
                {
                    SplitCoins = ConvertTransactionArguments(jo["SplitCoins"], serializer)
                };
                return splitCoinsTransaction;
            }
            else if (jo["MergeCoins"] != null)
            {
                var mergeCoinsTransaction = new MergeCoinsTransaction
                {
                    MergeCoins = ConvertTransactionArguments(jo["MergeCoins"], serializer)
                };
                return mergeCoinsTransaction;
            }
            else if (jo["Publish"] != null)
            {
                return new SuiPublishTransaction
                {
                    Publish = jo["Publish"].ToObject<List<string>>()
                };
            }
            else if (jo["Upgrade"] != null)
            {
                return new SuiUpgradeTransaction
                {
                    Upgrade = jo["Upgrade"].ToObject<List<List<string>>>()
                };
            }
            else if (jo["MakeMoveVec"] != null)
            {
                var makeMoveVecTransaction = new SuiMakeMoveVecTransaction
                {
                    MakeMoveVec = ConvertTransactionArguments(jo["MakeMoveVec"], serializer)
                };
                return makeMoveVecTransaction;
            }
            else
            {
                throw new JsonSerializationException();
            }
        }

        private List<object> ConvertTransactionArguments(JToken argumentsToken, JsonSerializer serializer)
        {
            var arguments = new List<object>();

            foreach (var token in argumentsToken)
            {
                switch (token.Type)
                {
                    case JTokenType.String:
                        arguments.Add(token.ToObject<string>());
                        break;
                    case JTokenType.Array:
                        arguments.Add(token.ToObject<List<SuiArgument>>(serializer));
                        break;
                    case JTokenType.Object:
                        arguments.Add(token.ToObject<SuiArgument>(serializer));
                        break;
                    default:
                        throw new Exception("Unexpected token type: " + token.Type);
                }
            }

            return arguments;
        }


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

}
