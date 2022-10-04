using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// we have uints in move
public struct OnChainVector2
{
    public ulong x { get; private set; }
    public ulong y { get; private set; }

    private Vector2 _vector2;

    private const ulong SIGNED_OFFSET = 10000000;
    private const float FLOATING_POINT_SCALE = 10000.0f;
    
    public OnChainVector2(ulong x, ulong y)
    {
        this.x = x;
        this.y = y;
        _vector2 = MakeVector2(x, y);
    }

    public OnChainVector2(Vector2 vector2)
    {
        this.x = Convert.ToUInt64((vector2.x + SIGNED_OFFSET) * FLOATING_POINT_SCALE);
        this.y = Convert.ToUInt64((vector2.y + SIGNED_OFFSET) * FLOATING_POINT_SCALE);
        _vector2 = MakeVector2(this.x, this.y);
    }

    private static Vector2 MakeVector2(ulong x, ulong y)
    {
        return new Vector2(x  / FLOATING_POINT_SCALE - SIGNED_OFFSET, y / FLOATING_POINT_SCALE - SIGNED_OFFSET);
    }
    
    public Vector2 ToVector2()
    {
        return _vector2;
    }
}