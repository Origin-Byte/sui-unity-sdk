using Newtonsoft.Json;

namespace Suinet.Faucet
{
    public class AirdropRequest
    {
        public FixedAmountRequest FixedAmountRequest { get; set; }

        public AirdropRequest(string recipient)
        {
            FixedAmountRequest = new FixedAmountRequest()
            {
                Recipient = recipient
            };
        }
    }

    public class FixedAmountRequest
    {
        [JsonProperty("recipient")]
        public string Recipient { get; set; }
    }
}
