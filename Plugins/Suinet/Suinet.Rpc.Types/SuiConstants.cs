namespace Suinet.Rpc.Types
{
    public static class SuiConstants
    {
        public const string SUI_COIN_TYPE = "0x2::coin::Coin<0x2::sui::SUI>";
        public const string DEVNET_ENDPOINT = "https://fullnode.devnet.sui.io";

        // Sui uses SHA3-256 hence 32 bytes here
        public const int TRANSACTION_DIGEST_LENGTH = 32;
        public const int OBJECT_DIGEST_LENGTH = 32;

        public const int SUI_ADDRESS_LENGTH = 20;
    }
}
