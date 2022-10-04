using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnChainPlayer : MonoBehaviour
{
    public string ownerAddress;
    
    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (OnChainStateStore.States.ContainsKey(ownerAddress))
        {
            var onChainPosition = OnChainStateStore.States[ownerAddress].Position;
            var onChainVelocity = OnChainStateStore.States[ownerAddress].Velocity;
            // map from uint storage format
            var position = onChainPosition.ToVector2();
            var velocity = onChainVelocity.ToVector2();
            
           // Debug.Log("position: " + position.ToString() + " velocity: " + velocity.ToString());

            _rb.velocity = velocity;

            if ( Vector2.Distance(_rb.position,position) > 4.0f)
            {
                _rb.MovePosition(position);
            }
        }
    }
}
