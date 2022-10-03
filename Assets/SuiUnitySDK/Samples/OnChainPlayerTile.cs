using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnChainPlayerTile : MonoBehaviour
{
    public string ownerAddress;
    
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (OnChainStateStore.States.ContainsKey(ownerAddress))
        {
            var onChainPosition = OnChainStateStore.States[ownerAddress].Position;
            var onChainVelocity = OnChainStateStore.States[ownerAddress].Velocity;
            // map from uint storage format
            var position = new Vector2((onChainPosition.X - 100000) / 1000.0f, (onChainPosition.Y - 100000) / 1000.0f);
            var velocity = new Vector2((onChainVelocity.X - 100000) / 1000.0f, (onChainVelocity.Y - 100000) / 1000.0f);
            
            rb.velocity = velocity;
            rb.position = Vector2.Lerp(rb.position, position, Time.deltaTime);
        }
    }
}
