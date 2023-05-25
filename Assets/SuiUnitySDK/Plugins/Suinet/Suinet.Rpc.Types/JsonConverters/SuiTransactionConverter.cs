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
                return new SuiTransferObjectsTransaction
                {
                    TransferObjects = jo["TransferObjects"].ToObject<List<List<SuiArgument>>>()
                };
            }
            else if (jo["SplitCoins"] != null)
            {
                return new SplitCoinsTransaction
                {
                    SplitCoins = jo["SplitCoins"].ToObject<List<List<SuiArgument>>>()
                };
            }
            else if (jo["MergeCoins"] != null)
            {
                return new MergeCoinsTransaction
                {
                    MergeCoins = jo["MergeCoins"].ToObject<List<List<SuiArgument>>>()
                };
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
                return new SuiMakeMoveVecTransaction
                {
                    MakeMoveVec = jo["MakeMoveVec"].ToObject<List<List<SuiArgument>>>()
                };
            }
            else
            {
                throw new JsonSerializationException();
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

}
