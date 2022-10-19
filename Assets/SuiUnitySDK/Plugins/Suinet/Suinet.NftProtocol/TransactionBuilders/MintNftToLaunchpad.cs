using Suinet.Rpc;
using Suinet.Rpc.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Suinet.NftProtocol.TransactionBuilders
{
    public class MintNftToLaunchpad : IMoveCallTransactionBuilder
    {
        public string Signer { get; set; }

        public string PackageObjectId { get; set; }

        public string ModuleName { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public Dictionary<string, object> Attributes { get; set; }

        public string MintAuthority { get; set; }

        public string MarketId { get; set; }

        public ulong? TierIndex { get; set; }

        public MoveCallTransaction ToMoveCallTransaction(string gas, ulong gasBudget = 4000)
        {
            return new MoveCallTransaction()
            {
                Signer = Signer,
                PackageObjectId = PackageObjectId,
                Module = ModuleName,
                Function = "mint_nft",
                TypeArguments = Array.Empty<string>(),
                Arguments = TransactionUtils.BuildArguments(
                    Name,
                    Description,
                    Url,
                    Attributes.Keys.ToArray(),
                    Attributes.Values.ToArray(),
                    MintAuthority,
                    TierIndex ?? 0,
                    MarketId
                    ),
                Gas = gas,
                GasBudget = gasBudget
            };
        }

    
    }
}
