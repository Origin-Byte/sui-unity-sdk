using System.Numerics;

namespace Suinet.Rpc.Types
{
    public class TransactionFilter
    {
        public CheckpointFilter Checkpoint { get; set; }

        public MoveFunctionFilter MoveFunction { get; set; }

        public InputObjectFilter InputObject { get; set; }

        public ChangedObjectFilter ChangedObject { get; set; }

        public FromAddressFilter FromAddress { get; set; }

        public ToAddressFilter ToAddress { get; set; }

        public FromAndToAddressFilter FromAndToAddress { get; set; }

        public TransactionKindFilter TransactionKind { get; set; }
    }

    public class CheckpointFilter
    {
        public BigInteger Checkpoint { get; set; }
    }

    public class MoveFunctionFilter
    {
        public MoveFunction MoveFunction { get; set; }
    }

    public class MoveFunction
    {
        public string Function { get; set; }

        public string Module { get; set; }

        public string Package { get; set; }
    }

    public class InputObjectFilter
    {
        public string InputObject { get; set; }
    }

    public class ChangedObjectFilter
    {
        public string ChangedObject { get; set; }
    }

    public class FromAddressFilter
    {
        public string FromAddress { get; set; }
    }

    public class ToAddressFilter
    {
        public string ToAddress { get; set; }
    }

    public class FromAndToAddressFilter
    {
        public FromAndToAddress FromAndToAddress { get; set; }
    }

    public class FromAndToAddress
    {
        public string From { get; set; }

        public string To { get; set; }
    }

    public class TransactionKindFilter
    {
        public string TransactionKind { get; set; }
    }
}
