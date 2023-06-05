using System.Collections.Generic;
using Newtonsoft.Json;
using Suinet.Rpc.Types.JsonConverters;
using Suinet.Rpc.Types.MoveTypes;

namespace Suinet.Rpc.Types
{
    public enum SuiTransactionType
    {
        MoveCall,
        TransferObjects,
        SplitCoins,
        MergeCoins,
        Publish,
        Upgrade,
        MakeMoveVec
    }

    [JsonConverter(typeof(SuiTransactionConverter))]
    public abstract class SuiTransaction
    {
        public SuiTransactionType TransactionType { get; set; }
    }

    public class SuiMoveCallTransaction : SuiTransaction
    {
        public SuiProgrammableMoveCall MoveCall { get; set; }

        public SuiMoveCallTransaction()
        {
            TransactionType = SuiTransactionType.MoveCall;
        }
    }

    public class SuiTransferObjectsTransaction : SuiTransaction
    {
        public List<object> TransferObjects { get; set; }

        public SuiTransferObjectsTransaction()
        {
            TransactionType = SuiTransactionType.TransferObjects;
        }
    }

    public class SplitCoinsTransaction : SuiTransaction
    {
        public List<object> SplitCoins { get; set; }

        public SplitCoinsTransaction()
        {
            TransactionType = SuiTransactionType.SplitCoins;
        }
    }

    public class MergeCoinsTransaction : SuiTransaction
    {
        public List<object> MergeCoins { get; set; }

        public MergeCoinsTransaction()
        {
            TransactionType = SuiTransactionType.MergeCoins;
        }
    }

    public class SuiPublishTransaction : SuiTransaction
    {
        public List<string> Publish { get; set; }

        public SuiPublishTransaction()
        {
            TransactionType = SuiTransactionType.Publish;
        }
    }

    public class SuiUpgradeTransaction : SuiTransaction
    {
        public List<List<string>> Upgrade { get; set; }

        public SuiUpgradeTransaction()
        {
            TransactionType = SuiTransactionType.Upgrade;
        }
    }

    public class SuiMakeMoveVecTransaction : SuiTransaction
    {
        public List<object> MakeMoveVec { get; set; }

        public SuiMakeMoveVecTransaction()
        {
            TransactionType = SuiTransactionType.MakeMoveVec;
        }
    }
}
