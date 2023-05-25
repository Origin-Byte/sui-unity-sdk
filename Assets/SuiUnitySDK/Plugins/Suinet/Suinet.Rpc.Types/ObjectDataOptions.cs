using Newtonsoft.Json;

namespace Suinet.Rpc.Types
{
    public class ObjectDataOptions
    {
        [JsonProperty("showBcs")]
        public bool ShowBcs { get; set; } = false;

        [JsonProperty("showContent")]
        public bool ShowContent { get; set; } = false;

        [JsonProperty("showDisplay")]
        public bool ShowDisplay { get; set; } = false;

        [JsonProperty("showOwner")]
        public bool ShowOwner { get; set; } = false;

        [JsonProperty("showPreviousTransaction")]
        public bool ShowPreviousTransaction { get; set; } = false;

        [JsonProperty("showStorageRebate")]
        public bool ShowStorageRebate { get; set; } = false;

        [JsonProperty("showType")]
        public bool ShowType { get; set; } = false;

        // Factory
        public static ObjectDataOptions ShowAll()
        {
            return new ObjectDataOptions
            {
                ShowBcs = true,
                ShowContent = true,
                ShowDisplay = true,
                ShowOwner = true,
                ShowPreviousTransaction = true,
                ShowStorageRebate = true,
                ShowType = true
            };
        }

        public static ObjectDataOptions ShowNone()
        {
            return new ObjectDataOptions
            {
                ShowBcs = false,
                ShowContent = false,
                ShowDisplay = false,
                ShowOwner = false,
                ShowPreviousTransaction = false,
                ShowStorageRebate = false,
                ShowType = false
            };
        }
    }
}
