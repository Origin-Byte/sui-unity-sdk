using System.Collections;
using System.Collections.Generic;
using Org.BouncyCastle.Crypto.Prng.Drbg;
using UnityEngine;

public struct OnChainPlayerState
{
    // we have uints in move
    public struct OnChainVector2
    {
        public ulong X;
        public ulong Y;

        public OnChainVector2(ulong x, ulong y)
        {
            X = x;
            Y = y;
        }
    }
    
    public OnChainVector2 Position;
    public OnChainVector2 Velocity;

    public OnChainPlayerState(OnChainVector2 position, OnChainVector2 velocity)
    {
        Position = position;
        Velocity = velocity;
    }
}
