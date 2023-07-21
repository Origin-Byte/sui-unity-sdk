namespace Suinet.Rpc.Types
{
    public static class SuiConstants
    {
        public const string SUI_COIN_TYPE = "0x2::coin::Coin<0x2::sui::SUI>";
        public const string TESTNET_FULLNODE = "https://rpc-testnet.suiscan.xyz:443/";
        public const string DEVNET_FULLNODE = "https://fullnode.devnet.sui.io:443/";

        // Sui uses SHA3-256 hence 32 bytes here
        public const int TRANSACTION_DIGEST_LENGTH = 32;
        public const int OBJECT_DIGEST_LENGTH = 32;

        public const int SUI_ADDRESS_LENGTH = 32;

        public const string KIOSK_TYPE = "0x2::kiosk::Kiosk";
        public const string KIOSK_ITEM_TYPE = "0x2::kiosk::Item";
        
        public const string SUIPLAY_API_URL = "https://suiplay-api.originbyte.io/v1";
    }
}
