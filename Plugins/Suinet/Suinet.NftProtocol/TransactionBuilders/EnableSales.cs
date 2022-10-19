﻿using Suinet.Rpc;
using Suinet.Rpc.Types;

namespace Suinet.NftProtocol.TransactionBuilders
{
    public class EnableSales : IMoveCallTransactionBuilder
    {
        public string Signer { get; set; }

        public string PackageObjectId { get; set; }

        public string ModuleName { get; set; }

        public string LaunchpadId { get; set; }

        public string CollectionType { get; set; }

        public MoveCallTransaction ToMoveCallTransaction(string gas, ulong gasBudget = 4000, SuiExecuteTransactionRequestType RequestType = SuiExecuteTransactionRequestType.WaitForEffectsCert)
        {
            return new MoveCallTransaction()
            {
                Signer = Signer,
                PackageObjectId = PackageObjectId,
                Module = "fixed_price",
                Function = "sale_on",
                TypeArguments = ArgumentBuilder.BuildTypeArguments(
                    CollectionType
                    ),
                Arguments = ArgumentBuilder.BuildArguments(
                    LaunchpadId
                    ),
                Gas = gas,
                GasBudget = gasBudget,
                RequestType = RequestType
            };
        }    
    }
}
