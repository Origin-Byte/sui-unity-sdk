using Newtonsoft.Json;
using System;

namespace Suinet.Rpc.Types.Converters
{
    public class SuiExecuteTransactionResponseConverter : JsonConverter<SuiExecuteTransactionResponse>
    {
        public override SuiExecuteTransactionResponse ReadJson(JsonReader reader, Type objectType, SuiExecuteTransactionResponse existingValue, bool hasExistingValue, JsonSerializer serializer)
        {    
            SuiExecuteTransactionResponse response = new SuiExecuteTransactionResponse();
            serializer.Populate(reader, response);

            var suiExecuteTransactionRequestType = SuiExecuteTransactionRequestType.None;
            if (response.Effects != null)
            {
                suiExecuteTransactionRequestType = SuiExecuteTransactionRequestType.WaitForEffectsCert;
            }

            response.ExecuteTransactionRequestType = suiExecuteTransactionRequestType;

            return response;
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, SuiExecuteTransactionResponse value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
