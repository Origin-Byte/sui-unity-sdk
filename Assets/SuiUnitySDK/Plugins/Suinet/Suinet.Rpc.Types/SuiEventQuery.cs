using Newtonsoft.Json;
using Suinet.Rpc.Types.Converters;

namespace Suinet.Rpc.Types
{
    public interface ISuiEventQuery
    {
        SuiEventQueryType QueryType { get; }
    }

    [JsonConverter(typeof(SuiAllEventQueryConverter))]
    public class SuiAllEventQuery : ISuiEventQuery
    {
        public SuiEventQueryType QueryType => SuiEventQueryType.All;
    }

    /// <summary>
    /// Return events emitted in a specified Move module
    /// </summary>
    public class SuiMoveModulEventQuery : ISuiEventQuery
    {
        public SuiEventQueryType QueryType => SuiEventQueryType.MoveModule;

        public SuiMoveModule MoveModule { get; set; }

        public class SuiMoveModule
        {
            public string Module { get; set; }

            public string Package { get; set; }
        }
    }

    /// <summary>
    /// Return events with the given move event struct name
    /// </summary>
    public class SuiMoveEventEventQuery : ISuiEventQuery
    {
        public SuiEventQueryType QueryType => SuiEventQueryType.MoveEvent;

        public string MoveEvent { get; set; }
    }

    /// <summary>
    /// Return events with the given event type
    /// </summary>
    public class SuiEventTypeEventQuery : ISuiEventQuery
    {
        public SuiEventQueryType QueryType => SuiEventQueryType.EventType;

        public SuiEventType MoveEvent { get; set; }
    }

    /// <summary>
    /// Query by sender address
    /// </summary>
    public class SuiSenderEventQuery : ISuiEventQuery
    {
        public SuiEventQueryType QueryType => SuiEventQueryType.Sender;

        public string Sender { get; set; }
    }

    /// <summary>
    /// Query by recipient address
    /// </summary>
    public class SuiRecipientEventQuery : ISuiEventQuery
    {
        public SuiEventQueryType QueryType => SuiEventQueryType.Recipient;

        public string Recipient { get; set; }
    }

    /// <summary>
    /// Return events associated with the given object
    /// </summary>
    public class SuiObjectEventQuery : ISuiEventQuery
    {
        public SuiEventQueryType QueryType => SuiEventQueryType.Object;

        public string Object { get; set; }
    }

    /// <summary>
    /// Return events emitted in [start_time, end_time] interval
    /// </summary>
    public class SuiTimeRangeEventQuery : ISuiEventQuery
    {
        public SuiEventQueryType QueryType => SuiEventQueryType.TimeRange;

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
            public ulong StarTime { get; set; }
        }
    }

    public enum SuiEventQueryType
    {
        All,
        Transaction,
        MoveModule,
        MoveEvent,
        EventType,
        Sender,
        Recipient,
        Object,
        TimeRange
    }
}
