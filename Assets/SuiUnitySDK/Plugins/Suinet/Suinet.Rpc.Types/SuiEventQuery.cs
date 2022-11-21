using Newtonsoft.Json;
using Suinet.Rpc.Types.Converters;

namespace Suinet.Rpc.Types
{
    public interface ISuiEventQuery
    {
    }

    [JsonConverter(typeof(SuiAllEventQueryConverter))]
    public class SuiAllEventQuery : ISuiEventQuery
    {
    }

    /// <summary>
    /// Return events emitted in a specified Move module
    /// </summary>
    public class SuiMoveModuleEventQuery : ISuiEventQuery
    {
        public SuiMoveModule MoveModule { get; set; }

        public class SuiMoveModule
        {
            [JsonProperty("module")]
            public string Module { get; set; }

            [JsonProperty("package")]
            public string Package { get; set; }
        }
    }

    /// <summary>
    /// Return events with the given move event struct name
    /// </summary>
    public class SuiMoveEventEventQuery : ISuiEventQuery
    {
        public string MoveEvent { get; set; }
    }

    /// <summary>
    /// Return events with the given event type
    /// </summary>
    public class SuiEventTypeEventQuery : ISuiEventQuery
    {
        public SuiEventType MoveEvent { get; set; }
    }

    /// <summary>
    /// Query by sender address
    /// </summary>
    public class SuiSenderEventQuery : ISuiEventQuery
    {
        public string Sender { get; set; }
    }

    /// <summary>
    /// Query by recipient address
    /// </summary>
    public class SuiRecipientEventQuery : ISuiEventQuery
    {
        public string Recipient { get; set; }
    }

    /// <summary>
    /// Return events associated with the given object
    /// </summary>
    public class SuiObjectEventQuery : ISuiEventQuery
    {
        public string Object { get; set; }
    }

    /// <summary>
    /// Return events emitted in [start_time, end_time] interval
    /// </summary>
    public class SuiTimeRangeEventQuery : ISuiEventQuery
    {
        public SuiTimeRange TimeRange { get; set; }

        public class SuiTimeRange
        {
            /// <summary>
            /// right endpoint of time interval, exclusive
            /// </summary>
            [JsonProperty("end_time")]
            public ulong EndTime { get; set; }

            /// <summary>
            /// left endpoint of time interval, inclusive
            /// </summary>
            [JsonProperty("start_time")]
            public ulong StartTime { get; set; }
        }
    }
}
