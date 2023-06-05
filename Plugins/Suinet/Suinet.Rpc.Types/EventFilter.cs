using Newtonsoft.Json;
using System.Collections.Generic;
using System.Numerics;

namespace Suinet.Rpc.Types
{
    public abstract class EventFilter { }

    public class SenderEventFilter : EventFilter
    {
        public SuiAddress Sender { get; set; }
    }

    public class TransactionEventFilter : EventFilter
    {
        public string Transaction { get; set; }
    }

    public class PackageEventFilter : EventFilter
    {
        public ObjectId Package { get; set; }
    }

    public class MoveModuleEventFilter : EventFilter
    {
       
        public MoveModuleType MoveModule { get; set; }

        public class MoveModuleType
        {
            [JsonProperty("module")]  
            public string Module { get; set; }

            [JsonProperty("package")]           
            public ObjectId Package { get; set; }
        }
    }

    public class MoveEventTypeEventFilter : EventFilter
    {
       
        public string MoveEventType { get; set; }
    }

    public class MoveEventFieldEventFilter : EventFilter
    {
       
        public MoveEventFieldType MoveEventField { get; set; }

        public class MoveEventFieldType
        {
            [JsonProperty("path")]
            public string Path { get; set; }

            // `value` in the schema is set to `true`. This might need to be adjusted

            [JsonProperty("value")]
            public bool Value { get; set; }
        }
    }

    public class TimeRangeEventFilter : EventFilter
    {
       
        public TimeRangeType TimeRange { get; set; }

        public class TimeRangeType
        {
            [JsonProperty("endTime")]  
            public BigInteger EndTime { get; set; }

            [JsonProperty("startTime")]
            public BigInteger StartTime { get; set; }
        }
    }

    public class AllEventFilter : EventFilter
    {
       
        public List<EventFilter> All { get; set; }
    }

    public class AnyEventFilter : EventFilter
    {
       
        public List<EventFilter> Any { get; set; }
    }

    public class AndEventFilter : EventFilter
    {
        public List<EventFilter> And { get; set; }
    }

    public class OrEventFilter : EventFilter
    {
        public List<EventFilter> Or { get; set; }
    }

}
