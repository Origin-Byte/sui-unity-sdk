using Newtonsoft.Json;
using Suinet.Rpc.Types.JsonConverters;

namespace Suinet.Rpc.Types
{
    [JsonConverter(typeof(SuiObjectReadConverter))]
    public class SuiObjectRead
    {
        public SuiObjectReadStatus Status { get; set; }

        /// <summary>
        /// SuiObjectReadStatus.Exists
        /// </summary>
        public SuiObject Object { get; set; }

        /// <summary>
        /// SuiObjectReadStatus.Deleted
        /// </summary>
        public SuiObjectRef ObjectRef { get; set; }

        /// <summary>
        /// SuiObjectReadStatus.NotExists
        /// </summary>
        public string ObjectId { get; set; }

        public SuiObjectRead(SuiObjectReadStatus status, SuiObject @object, SuiObjectRef objectRef, string objectId)
        {
            Status = status;
            Object = @object;
            ObjectRef = objectRef;
            ObjectId = objectId;
        }

        public SuiObjectRead(SuiObjectReadStatus status)
        {
            Status = status;
        }
    }

    public enum SuiObjectReadStatus
    {
        None,
        Exists,
        NotExists,
        Deleted
    }
}
