namespace Suinet.Rpc.Types
{
    public static class SuiConstants
    {
        public const string DEVNET_ENDPOINT = "https://gateway.devnet.sui.io:443";

        // Sui uses SHA3-256 hence 32 bytes here
        public const int TRANSACTION_DIGEST_LENGTH = 32;
        public const int OBJECT_DIGEST_LENGTH = 32;

        public const int SUI_ADDRESS_LENGTH = 20;
    }
}
