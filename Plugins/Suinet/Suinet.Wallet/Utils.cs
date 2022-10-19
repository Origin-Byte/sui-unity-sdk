using Chaos.NaCl;
using System.Collections.Generic;

namespace Suinet.Wallet
{
    /// <summary>
    /// Implements utility methods to be used in the wallet.
    /// src: https://github.com/bmresearch/Solnet/blob/master/src/Solnet.Wallet/Utilities/Utils.cs
    /// </summary>
    internal static class Utils
    {
        /// <summary>
        /// Adds or replaces a value in a dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key to add or replace.</param>
        /// <param name="value">The value.</param>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        internal static void AddOrReplace<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
                dictionary[key] = value;
            else
                dictionary.Add(key, value);
        }

        /// <summary>
        /// Attempts to get a value from a dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary to get the value from.</param>
        /// <param name="key">The key to get.</param>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <returns>The value.</returns>
        internal static TValue TryGet<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            dictionary.TryGetValue(key, out TValue value);
            return value;
        }

        /// <summary>
        /// Slices the array, returning a new array starting at <c>start</c> index and ending at <c>end</c> index.
        /// </summary>
        /// <param name="source">The array to slice.</param>
        /// <param name="start">The starting index of the slicing.</param>
        /// <param name="end">The ending index of the slicing.</param>
        /// <typeparam name="T">The array type.</typeparam>
        /// <returns>The sliced array.</returns>
        internal static T[] Slice<T>(this T[] source, int start, int end)
        {
            if (end < 0)
                end = source.Length;

            var len = end - start;

            // Return new array.
            var res = new T[len];
            for (var i = 0; i < len; i++) res[i] = source[i + start];
            return res;
        }

        /// <summary>
        /// Slices the array, returning a new array starting at <c>start</c>.
        /// </summary>
        /// <param name="source">The array to slice.</param>
        /// <param name="start">The starting index of the slicing.</param>
        /// <typeparam name="T">The array type.</typeparam>
        /// <returns>The sliced array.</returns>
        internal static T[] Slice<T>(this T[] source, int start)
        {
            return Slice(source, start, -1);
        }

        /// <summary>
        /// Gets the corresponding ed25519 key pair from the passed seed.
        /// </summary>
        /// <param name="seed">The seed</param>
        /// <returns>The key pair.</returns>
        internal static (byte[] privateKey, byte[] publicKey) EdKeyPairFromSeed(byte[] seed) =>
            (Ed25519.ExpandedPrivateKeyFromSeed(seed), Ed25519.PublicKeyFromSeed(seed));
    }
}
