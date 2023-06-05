using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Numerics;

namespace Suinet.Rpc.Types
{
    [JsonConverter(typeof(TransactionFilterConverter))]
    public abstract class TransactionFilter
    {
    }

    public class CheckpointFilter : TransactionFilter
    {
        public BigInteger Checkpoint { get; set; }
    }

    public class MoveFunctionFilter : TransactionFilter
    {
        public MoveFunction MoveFunction { get; set; }
    }

    public class MoveFunction : TransactionFilter
    {
        [JsonProperty("function")]
        public string Function { get; set; }

        [JsonProperty("module")]
        public string Module { get; set; }

        [JsonProperty("package")]
        public string Package { get; set; }
    }

    public class InputObjectFilter : TransactionFilter
    {
        public string InputObject { get; set; }
    }

    public class ChangedObjectFilter : TransactionFilter
    {
        public string ChangedObject { get; set; }
    }

    public class FromAddressFilter : TransactionFilter      
    {
        public string FromAddress { get; set; }
    }

    public class ToAddressFilter : TransactionFilter
    {
        public string ToAddress { get; set; }
    }

    public class FromAndToAddressFilter : TransactionFilter
    {
        public FromAndToAddress FromAndToAddress { get; set; }
    }

    public class FromAndToAddress
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }
    }

    public class TransactionKindFilter : TransactionFilter  
    {
        public string TransactionKind { get; set; }
    }
}
