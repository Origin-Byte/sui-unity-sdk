namespace Suinet.Rpc.Types
{
    public class SuiTransactionData
    {
        public object[] Transactions { get; set; }

        public string Sender { get; set; }

        public SuiObjectRef GasPayment { get; set; }

        public ulong GasBudget { get; set; }

    }
}
