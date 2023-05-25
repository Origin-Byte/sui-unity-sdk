﻿using Suinet.Rpc.Types;

namespace Suinet.NftProtocol.TransactionBuilders
{
    public interface IMoveCallTransactionBuilder
    {
        // signer address
        string Signer { get; }

        MoveCallTransaction BuildMoveCallTransaction(string gas = null, ulong gasBudget = 10000000, ExecuteTransactionRequestType RequestType = ExecuteTransactionRequestType.WaitForLocalExecution);
    }
}
