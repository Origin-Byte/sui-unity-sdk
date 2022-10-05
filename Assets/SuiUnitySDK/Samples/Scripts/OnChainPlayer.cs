using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OnChainPlayer : MonoBehaviour
{
    public string ownerAddress;
    
    private Rigidbody _rb;

    private OnChainPlayerState _lastAppliedPlayerState;
    private const float SPEED = 4.0f;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        var onChainPosition = GetOnChainPositionAsVec3();
        _rb.position = onChainPosition;
    }

    void FixedUpdate()
    {
        var onChainPosition = GetOnChainPositionAsVec3();
        var newPos = Vector3.MoveTowards(_rb.position, onChainPosition, SPEED * Time.fixedDeltaTime);
        _rb.MovePosition(newPos);
    }

    private Vector2 GetOnChainPositionAsVec2()
    {
        var result = _rb.position;
        if (OnChainStateStore.States.ContainsKey(ownerAddress))
        {
            var playerState = OnChainStateStore.States[ownerAddress];

            var onChainPosition = playerState.Position;
           // var onChainVelocity = playerState.Velocity;

            // map from uint storage format
            result = onChainPosition.ToVector2();
           // var velocity = onChainVelocity.ToVector2();
        }

        return result;
    }

    private Vector3 GetOnChainPositionAsVec3()
    {
        var vec2 = GetOnChainPositionAsVec2();
        return new Vector3(vec2.x, 0f, vec2.y);
    }
}
