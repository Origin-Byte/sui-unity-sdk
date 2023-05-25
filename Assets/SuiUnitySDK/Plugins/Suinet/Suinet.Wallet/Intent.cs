using Chaos.NaCl;
using System;

namespace Suinet.Wallet
{
    public static class Intent
    {
        public static byte[] GetMessageWithIntent(string message)
        {
            return GetMessageWithIntent(CryptoBytes.FromBase64String(message));
        }

        public static byte[] GetMessageWithIntent(byte[] message)
        {
            // See: sui/crates/sui-types/src/intent.rs 
            // This is currently hardcoded with [IntentScope::TransactionData = 0, Version::V0 = 0, AppId::Sui = 0]
            var INTENT_BYTES = new byte[] { 0, 0, 0 };

            var messageWithIntent = new byte[INTENT_BYTES.Length + message.Length];
            Buffer.BlockCopy(INTENT_BYTES, 0, messageWithIntent, 0, INTENT_BYTES.Length);
            Buffer.BlockCopy(message, 0, messageWithIntent, INTENT_BYTES.Length, message.Length);
            return messageWithIntent;
        }
    }
}
