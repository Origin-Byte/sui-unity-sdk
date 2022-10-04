using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OnChainPlayer : MonoBehaviour
{
    public string ownerAddress;
    
    private Rigidbody2D _rb;

    private OnChainPlayerState _lastAppliedPlayerState;
    private const float SPEED = 4.0f;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        var onChainPosition = GetOnChainPositionAsVec2();
        _rb.position = onChainPosition;
    }

    void FixedUpdate()
    {
        var onChainPosition = GetOnChainPositionAsVec2();
        var newPos = Vector2.MoveTowards(_rb.position, onChainPosition, SPEED * Time.fixedDeltaTime);
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
}
