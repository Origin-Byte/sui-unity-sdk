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
    }

    void FixedUpdate()
    {
        if (OnChainStateStore.States.ContainsKey(ownerAddress))
        {
            var playerState = OnChainStateStore.States[ownerAddress];

            if (_lastAppliedPlayerState.Position.x == playerState.Position.x &&
                _lastAppliedPlayerState.Position.y == playerState.Position.y &&
                _lastAppliedPlayerState.Velocity.x == playerState.Velocity.x &&
                _lastAppliedPlayerState.Velocity.y == playerState.Velocity.y)
            {
               // return;
            }

            var onChainPosition = playerState.Position;
            var onChainVelocity = playerState.Velocity;
            
            // map from uint storage format
            var position = onChainPosition.ToVector2();
            var velocity = onChainVelocity.ToVector2();
        
           // Debug.Log("position: " + position.ToString() + " velocity: " + velocity.ToString());

           //_rb.velocity = velocity;

            //if ( Vector2.Distance(_rb.position,position) > 3.0f)
            {
                //_rb.position = position;
                var newPos = Vector2.MoveTowards(_rb.position, position, SPEED * Time.deltaTime);
                _rb.MovePosition(newPos);
            }
        }
    }
}
