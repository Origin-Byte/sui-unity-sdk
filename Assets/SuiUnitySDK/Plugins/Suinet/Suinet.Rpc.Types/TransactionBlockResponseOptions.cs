using Newtonsoft.Json;

namespace Suinet.Rpc.Types
{
    public class TransactionBlockResponseOptions
    {
        [JsonProperty("showBalanceChanges")]
        public bool ShowBalanceChanges { get; set; } = false;

        [JsonProperty("showEffects")]
        public bool ShowEffects { get; set; } = false;

        [JsonProperty("showEvents")]
        public bool ShowEvents { get; set; } = false;

        [JsonProperty("showInput")]
        public bool ShowInput { get; set; } = false;

        [JsonProperty("showObjectChanges")]
        public bool ShowObjectChanges { get; set; } = false;

        [JsonProperty("showRawInput")]
        public bool ShowRawInput { get; set; } = false;

        public static TransactionBlockResponseOptions ShowAll()
        {
            return new TransactionBlockResponseOptions
            {
                ShowBalanceChanges = true,
                ShowEffects = true,
                ShowEvents = true,
                ShowInput = true,
                ShowObjectChanges = true,
                ShowRawInput = true,
            };
        }

        public static TransactionBlockResponseOptions ShowNone()
        {
            return new TransactionBlockResponseOptions
            {
                ShowBalanceChanges = false,
                ShowEffects = false,
                ShowEvents = false,
                ShowInput = false,
                ShowObjectChanges = false,
                ShowRawInput = false,
            };
        }
    }
}
