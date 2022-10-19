using Suinet.Rpc.Types;

namespace Suinet.NftProtocol.TransactionBuilders
{
    public interface IMoveCallTransactionBuilder
    {
        // signer address
        string Signer { get; }

        MoveCallTransaction BuildMoveCallTransaction(string gas, ulong gasBudget = 4000, SuiExecuteTransactionRequestType RequestType = SuiExecuteTransactionRequestType.WaitForEffectsCert);
    }
}
