using Suinet.Rpc;
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

        public MoveCallTransaction BuildMoveCallTransaction(string gas, ulong gasBudget, ExecuteTransactionRequestType RequestType)
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
