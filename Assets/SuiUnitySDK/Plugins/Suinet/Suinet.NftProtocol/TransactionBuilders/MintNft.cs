using Suinet.Rpc.Types;
using Suinet.Rpc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Suinet.NftProtocol.TransactionBuilders
{
    public class MintNft : IMoveCallTransactionBuilder
    {
        public string Signer { get; set; }

        public string PackageObjectId { get; set; }

        public string ModuleName { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public Dictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();

        public string MintCap { get; set; }

        public string Recipient { get; set; }

        public MoveCallTransaction BuildMoveCallTransaction(string gas, ulong gasBudget = 4000, SuiExecuteTransactionRequestType RequestType = SuiExecuteTransactionRequestType.WaitForEffectsCert)
        {
            return new MoveCallTransaction()
            {
                Signer = Signer,
                PackageObjectId = PackageObjectId,
                Module = ModuleName,
                Function = "airdrop",
                TypeArguments = Array.Empty<string>(),
                Arguments = ArgumentBuilder.BuildArguments(
                    Name,
                    Description,
                    Url,
                    Attributes.Keys.ToArray(),
                    Attributes.Values.ToArray(),
                    MintCap,
                    Recipient
                    ),
                Gas = gas,
                GasBudget = gasBudget,
                RequestType = RequestType
            };
        }


    }
}
