using Suinet.Rpc;
using Suinet.Rpc.Types;

namespace Suinet.NftProtocol.TransactionBuilders
{
    public class ClaimNftCertificate : IMoveCallTransactionBuilder
    {
        public string Signer { get; set; }

        public string PackageObjectId { get; set; }

        public string ModuleName { get; set; }

        public string LaunchpadId { get; set; }

        public string NftId { get; set; }

        public string CertificateId { get; set; }

        public string Recipient { get; set; }

        public string NftType { get; set; }

        public string CollectionType { get; set; }

        public ulong? TierIndex { get; set; }

        public MoveCallTransaction ToMoveCallTransaction(string gas, ulong gasBudget = 4000)
        {
            return new MoveCallTransaction()
            {
                Signer = Signer,
                PackageObjectId = PackageObjectId,
                Module = "slingshot",
                Function = "claim_nft_embedded",
                TypeArguments = TransactionUtils.BuildTypeArguments(
                    CollectionType,
                    $"{PackageObjectId}::fixed_price::FixedPriceMarket",
                    $"{PackageObjectId}::{NftType}"
                    ),
                Arguments = TransactionUtils.BuildArguments(
                    LaunchpadId,
                    NftId,
                    CertificateId,
                    Recipient
                    ),
                Gas = gas,
                GasBudget = gasBudget
            };
        }    
    }
}
