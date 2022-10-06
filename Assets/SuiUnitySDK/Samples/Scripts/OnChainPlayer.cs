using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class OnChainPlayer : MonoBehaviour
{
    public string ownerAddress;
    
    private Rigidbody2D _rb;
    private OnChainPlayerState _lastAppliedPlayerState;
    private ulong lastSyncedSequenceNumber = 0;
    private float lastSyncedTimeStamp = 0f;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (OnChainStateStore.States.ContainsKey(ownerAddress))
        {
            var playerState = OnChainStateStore.States[ownerAddress];
  
            //if (playerState.SequenceNumber != lastSyncedSequenceNumber)
            {
               // Debug.Log($"FixedUpdate position.x:{ playerState.Position.x}, position.y:{ playerState.Position.y} velocity.x:{ playerState.Velocity.x} velocity.y:{ playerState.Velocity.y}");
               // Debug.Log($"FixedUpdate vec2 position:{ playerState.Position.ToVector2()}, velocity:{ playerState.Velocity.ToVector2()}");

                var onChainPosition = playerState.Position.ToVector2();
                var onChainVelocity = playerState.Velocity.ToVector2();

                var compensatedOnChainPosition = onChainPosition + onChainVelocity * ((Time.time - lastSyncedTimeStamp) / 2f);
                
                if (onChainVelocity != _rb.velocity)
                {
                  //  Debug.Log($"FixedUpdate _rb.velocity { _rb.velocity }. correcting");

                    _rb.velocity = onChainVelocity;
                    _rb.position = compensatedOnChainPosition;
                    transform.rotation  = Quaternion.Euler(new Vector3(0,0, Vector2.Angle(Vector2.up, _rb.velocity) ));
                    Debug.Log($"FixedUpdate _rb.velocity { _rb.velocity }. corrected");

                }
                else if (Vector2.Distance(compensatedOnChainPosition, _rb.position) > 1f)
                {
                    //_rb.position = onChainPositionVec3;
                    // TODO calc elapsed time since last sync etc?
                    var newPos = Vector2.MoveTowards(_rb.position, compensatedOnChainPosition, 8.0f * Time.deltaTime);
                    _rb.MovePosition(newPos);
                    Debug.Log($"FixedUpdate _rb.position { _rb.position }. corrected. 8.0f * Time.deltaTime: {8.0f * Time.deltaTime}");
                }
  
                lastSyncedSequenceNumber = playerState.SequenceNumber;
                lastSyncedTimeStamp = Time.time;
            }
        }
    }
}
