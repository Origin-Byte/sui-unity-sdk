using Chaos.NaCl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Suinet.Wallet
{
    //based on https://github.com/MystenLabs/sui/blob/main/sdk/typescript/src/cryptography/signature.ts

    public enum SignatureScheme
    {
        Ed25519,
        Secp256k1
    }

    public class SignaturePubkeyPair
    {
        public SignatureScheme SignatureScheme { get; set; }
        // Base64-encoded signature
        public byte[] Signature { get; set; }
        // Base64-encoded public key
        public string PubKey { get; set; }
    }

    // flag || signature || pubkey bytes, as base-64 encoded string
    // Signature is committed to the intent message of the transaction data, as base-64 encoded string
    public class SerializedSignature
    {
        public string Value { get; set; }
    }

    public static class SignatureUtils
    {
        private static readonly Dictionary<SignatureScheme, byte> SignatureSchemeToFlag = new Dictionary<SignatureScheme, byte>
    {
        { SignatureScheme.Ed25519, 0x00 },
        { SignatureScheme.Secp256k1, 0x01 },
    };

        private static readonly Dictionary<byte, SignatureScheme> SignatureFlagToScheme = new Dictionary<byte, SignatureScheme>
    {
        { 0x00, SignatureScheme.Ed25519 },
        { 0x01, SignatureScheme.Secp256k1 },
    };

        public static SerializedSignature ToSerializedSignature(SignaturePubkeyPair pair)
        {
            var list = new List<byte>();
            list.Add(SignatureSchemeToFlag[pair.SignatureScheme]);
            list.AddRange(pair.Signature);
            list.AddRange(CryptoBytes.FromBase64String(pair.PubKey));

            return new SerializedSignature { Value = CryptoBytes.ToBase64String(list.ToArray()) };
        }

        public static SignaturePubkeyPair FromSerializedSignature(SerializedSignature serializedSignature)
        {
            byte[] bytes = Convert.FromBase64String(serializedSignature.Value);

            var signatureScheme = SignatureFlagToScheme[bytes[0]];

            // Define public key size based on signature scheme
            var pubKeySize = signatureScheme == SignatureScheme.Ed25519 ? 32 : 33;

            // Slice signature bytes from array, skipping the first byte (scheme flag)
            // and stopping before the last pubKeySize bytes (public key)
            var signature = bytes.Skip(1).Take(bytes.Length - 1 - pubKeySize).ToArray();

            // Slice public key bytes from array, skipping the first byte (scheme flag)
            // and the signature bytes
            var pubkeyBytes = bytes.Skip(1 + signature.Length).ToArray();

            return new SignaturePubkeyPair
            {
                SignatureScheme = signatureScheme,
                Signature = signature,
                PubKey = Convert.ToBase64String(pubkeyBytes)
            };
        }
    }
}
