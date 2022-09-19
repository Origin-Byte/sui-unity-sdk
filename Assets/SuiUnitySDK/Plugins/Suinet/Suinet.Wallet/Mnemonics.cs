using Chaos.NaCl;
using Suinet.Bip39;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Suinet.Wallet
{
    public static class Mnemonics
    {
        private const BIP39Wordlist DefaultWordList = BIP39Wordlist.English;

        public static string GenerateMnemonic(int? entropy = 128, BIP39Wordlist? wordList = DefaultWordList)
        {
            var bip39 = new BIP39();
            return bip39.GenerateMnemonic(entropy.Value, wordList.Value);
        }

        public static bool ValidateMnemonic(string mnemonic, BIP39Wordlist? wordList = DefaultWordList)
        {
            var bip39 = new BIP39();
            return bip39.ValidateMnemonic(mnemonic, wordList.Value);
        }

        public static Ed25519KeyPair GetKeypairFromMnemonic(string mnemonic)
        {
            var normalizedMnemo = SanitizeMnemonic(mnemonic).Normalize(NormalizationForm.FormKD);
            var bip39 = new BIP39();
            var seed = bip39.MnemonicToSeed(normalizedMnemo, string.Empty);
            var publicKey = new byte[Ed25519.PublicKeySizeInBytes];
            var privateKey = new byte[Ed25519.ExpandedPrivateKeySizeInBytes];
            Ed25519.KeyPairFromSeed(new ArraySegment<byte>(publicKey), new ArraySegment<byte>(privateKey), new ArraySegment<byte>(seed,0,32));
            return new Ed25519KeyPair(publicKey, privateKey);
        }

        public static string SanitizeMnemonic(string mnemonic)
        {
            var words = Regex.Split(mnemonic.Trim(), @"\s+");
            return string.Join(" ", words).ToLower();
        }
    }
}
