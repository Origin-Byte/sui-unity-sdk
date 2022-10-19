using Chaos.NaCl;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using Suinet.Bip39;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Suinet.Wallet
{
    /// <summary>
    /// Based on https://github.com/bmresearch/Solnet/blob/master/src/Solnet.Wallet/Ed25519Bip32.cs
    /// </summary>
    public class Mnemonics
    {
        private const BIP39Wordlist DefaultWordList = BIP39Wordlist.English;
        private const uint HardenedOffset = 0x80000000;
        /// <summary>
        /// The seed for the Ed25519 BIP32 HMAC-SHA512 master key calculation.
        /// </summary>
        private const string Curve = "ed25519 seed";

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

        public static IKeyPair GetKeypairFromMnemonic(string mnemonic)
        {
            var normalizedMnemo = SanitizeMnemonic(mnemonic).Normalize(NormalizationForm.FormKD);
            var bip39 = new BIP39();
            var seed = bip39.MnemonicToSeed(normalizedMnemo, string.Empty);

            var derivedPath = DerivePath(seed);

            var publicKey = new byte[Ed25519.PublicKeySizeInBytes];
            var privateKey = new byte[Ed25519.ExpandedPrivateKeySizeInBytes];
            Ed25519.KeyPairFromSeed(new ArraySegment<byte>(publicKey), new ArraySegment<byte>(privateKey), new ArraySegment<byte>(derivedPath.Key, 0,32));
            return new Ed25519KeyPair(publicKey, privateKey);
        }

        public static string SanitizeMnemonic(string mnemonic)
        {
            var words = Regex.Split(mnemonic.Trim(), @"\s+");
            return string.Join(" ", words).ToLower();
        }

        private static bool IsValidPath(string path)
        {
            Regex regex = new Regex("^m(\\/[0-9]+')+$");

            if (!regex.IsMatch(path))
                return false;

            bool valid = !(path.Split('/')
                .Slice(1)
                .Select(a => a.Replace("'", ""))
                .Any(a => !int.TryParse(a, out _)));

            return valid;
        }

        private static (byte[] Key, byte[] ChainCode) HmacSha512(byte[] keyBuffer, byte[] data)
        {
            byte[] i = new byte[64];
            var digest = new Sha512Digest();
            HMac hmac = new HMac(digest);

            hmac.Init(new KeyParameter(keyBuffer));
            hmac.BlockUpdate(data, 0, data.Length);
            hmac.DoFinal(i, 0);

            byte[] il = i.Slice(0, 32);
            byte[] ir = i.Slice(32);

            return (Key: il, ChainCode: ir);
        }

        private static (byte[] Key, byte[] ChainCode) GetChildKeyDerivation(byte[] key, byte[] chainCode, uint index)
        {
            using (var buffer = new MemoryStream())
            {

                buffer.Write(new byte[] { 0 }, 0, 1);
                buffer.Write(key, 0, key.Length);
                byte[] indexBytes = new byte[4];
                BinaryPrimitives.WriteUInt32BigEndian(indexBytes, index);
                buffer.Write(indexBytes, 0, indexBytes.Length);

                return HmacSha512(chainCode, buffer.ToArray());
            }
        }

        /// <summary>
        /// Gets the master key used for key generation from the passed seed.
        /// </summary>
        /// <param name="seed">The seed used to calculate the master key.</param>
        /// <returns>A tuple consisting of the key and corresponding chain code.</returns>
        private static (byte[] Key, byte[] ChainCode) GetMasterKeyFromSeed(byte[] seed)
            => HmacSha512(Encoding.UTF8.GetBytes(Curve), seed);

        public static (byte[] Key, byte[] ChainCode) DerivePath(byte[] seed, string path = "m/44'/784'/0'/0'/0'")
        {
            if (!IsValidPath(path))
                throw new FormatException("Invalid derivation path");

            var (masterKey, chainCode) = GetMasterKeyFromSeed(seed);

            IEnumerable<uint> segments = path
                .Split('/')
                .Slice(1)
                .Select(a => a.Replace("'", ""))
                .Select(a => Convert.ToUInt32(a, 10));

            (byte[] _masterKey, byte[] _chainCode) results = segments
                .Aggregate(
                    (masterKey, chainCode),
                    (masterKeyFromSeed, next) =>
                        GetChildKeyDerivation(masterKeyFromSeed.masterKey, masterKeyFromSeed.chainCode, next + HardenedOffset));

            return results;
        }
    }
}
