using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// we have uint64 in move
public struct OnChainVector2
{
    public ulong x { get; private set; }
    public ulong y { get; private set; }

    private Vector2 _vector2;

    private const ulong SIGNED_OFFSET = 10000000;
    private const float FLOATING_POINT_SCALE = 1000.0f;
    
    public OnChainVector2(ulong x, ulong y)
    {
        this.x = x;
        this.y = y;
        _vector2 = MakeVector2(x, y);
        
       // Debug.Log($"ctor1 this.x: {this.x}, this.y: {this.y}, _vector2: {_vector2}");

    }

    public OnChainVector2(Vector2 vector2)
    {
        this.x = Convert.ToUInt64(Convert.ToDouble(vector2.x) * FLOATING_POINT_SCALE + SIGNED_OFFSET);
        this.y = Convert.ToUInt64(Convert.ToDouble(vector2.y) * FLOATING_POINT_SCALE + SIGNED_OFFSET);
        
        _vector2 = MakeVector2(this.x, this.y);
        
        //Debug.Log($"ctor2 input vector2 {vector2}, this.x: {this.x}, this.y: {this.y}, _vector2: {_vector2}");

    }

    private static Vector2 MakeVector2(ulong x, ulong y)
    {
        long signedX = SIGNED_OFFSET > x ? Convert.ToInt64(SIGNED_OFFSET - x) * -1L : Convert.ToInt64(x - SIGNED_OFFSET);
        long signedY = SIGNED_OFFSET > y ? Convert.ToInt64(SIGNED_OFFSET - y) * -1L : Convert.ToInt64(y - SIGNED_OFFSET);
        var vec = new Vector2(  signedX / FLOATING_POINT_SCALE, signedY / FLOATING_POINT_SCALE);
        return vec;
    }
    
    public Vector2 ToVector2()
    {
        return _vector2;
    }
    
    public Vector3 ToVector3()
    {
        return new Vector3(_vector2.x, _vector2.y);
    }
}