using Suinet.Rpc;
using Suinet.Rpc.Types;

namespace Suinet.NftProtocol.TransactionBuilders
{
    public class BuyNftCertificate : IMoveCallTransactionBuilder
    {
        public string Signer { get; set; }

        public string PackageObjectId { get; set; }

        public string ModuleName { get; set; }

        public string LaunchpadId { get; set; }

        public string CollectionType { get; set; }

        // coin object id
        public string Wallet { get; set; }

        public ulong? TierIndex { get; set; }

        public MoveCallTransaction ToMoveCallTransaction(string gas, ulong gasBudget = 4000)
        {
            return new MoveCallTransaction()
            {
                Signer = Signer,
                PackageObjectId = PackageObjectId,
                Module = "fixed_price",
                Function = "buy_nft_certificate",
                TypeArguments = TransactionUtils.BuildTypeArguments(
                    CollectionType
                    ),
                Arguments = TransactionUtils.BuildArguments(
                    Wallet,
                    LaunchpadId,
                    TierIndex ?? 0
                    ),
                Gas = gas,
                GasBudget = gasBudget
            };
        }    
    }
}
