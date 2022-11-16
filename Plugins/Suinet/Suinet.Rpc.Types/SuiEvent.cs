using Suinet.Rpc.Types.MoveTypes;
using System.Collections.Generic;

namespace Suinet.Rpc.Types
{
    public class SuiEvent
    {
        public SuiEventType SuiEventType { get; set; }

        public SuiMoveEvent MoveEvent { get; set; }

        public SuiPublishedEvent PublishedEvent { get; set; }

        public SuiTransferObjectEvent TransferObject { get; set; }

        public SuiDeleteObjectEvent DeleteObject { get; set; }

        public SuiNewObjectEvent NewObject { get; set; }

        public SuiEpochChangeEvent EpochChange { get; set; }

        public SuiCheckpointEvent Checkpoint { get; set; }

        public class SuiMoveEvent
        {
            public string Bcs { get; set; }

            public Dictionary<string, object> Fields { get; set; }

            public string PackageId { get; set; }

            public string Sender { get; set; }

            public string TransactionModule { get; set; }

            public string Type { get; set; }
        }

        public class SuiPublishedEvent
        {
            public string PackageId { get; set; }

            public string Sender { get; set; }
        }

        public class SuiTransferObjectEvent
        {
            public string ObjectId { get; set; }

            public string PackageId { get; set; }

            public SuiOwner Recipient { get; set; }

            public string Sender { get; set; }

            public string TransactionModule { get; set; }

            public SuiTransferType Type { get; set; }

            public ulong Version { get; set; }
        }

        public class SuiDeleteObjectEvent
        {
            public string ObjectId { get; set; }

            public string PackageId { get; set; }

            public string Sender { get; set; }

            public string TransactionModule { get; set; }
        }

        public class SuiNewObjectEvent
        {
            public string ObjectId { get; set; }

            public string PackageId { get; set; }

            public SuiOwner Recipient { get; set; }

            public string Sender { get; set; }

            public string TransactionModule { get; set; }
        }

        public class SuiEpochChangeEvent
        {
            public ulong EpochChange { get; set; }
        }

        public class SuiCheckpointEvent
        {
            public ulong Checkpoint { get; set; }
        }
    }

    public enum SuiEventType
    {
        None,
        MoveEvent,
        Publish,
        TransferObject,
        DeleteObject,
        NewObject,
        EpochChange,
        Checkpoint,
        MutateObject,
        CoinBalanceChange
    }

    public enum SuiTransferType
    {
        None = 0,
        Coin = 1,
        ToAddress = 2,
        ToObject = 3
    }

}