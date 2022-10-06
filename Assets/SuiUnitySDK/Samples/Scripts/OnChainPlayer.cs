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
    private const float SPEED = 9.0f;
    private ulong lastSyncedSequenceNumber = 0;
    private float lastSyncedTimeStamp = 0f;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        // var onChainPositionAsVec3 = GetOnChainPositionAsVec3();
        // lastSyncedTimeStamp = Time.time;
        // _rb.position = onChainPositionAsVec3;
        //
        //Debug.Log("Start onChainPositionAsVec3: " + onChainPositionAsVec3);
    }

    void FixedUpdate()
    {
        if (OnChainStateStore.States.ContainsKey(ownerAddress))
        {
            var playerState = OnChainStateStore.States[ownerAddress];
  
            if (playerState.SequenceNumber != lastSyncedSequenceNumber)
            {
               // Debug.Log($"FixedUpdate position.x:{ playerState.Position.x}, position.y:{ playerState.Position.y} velocity.x:{ playerState.Velocity.x} velocity.y:{ playerState.Velocity.y}");
              //  Debug.Log($"FixedUpdate vec2 position:{ playerState.Position.ToVector2()}, velocity:{ playerState.Velocity.ToVector2()}");

                var onChainPosition = playerState.Position;
                var onChainPositionVec3 = onChainPosition.ToVector3();
                //var newPos = Vector3.MoveTowards(_rb.position, onChainPositionVec3, SPEED * Time.fixedDeltaTime);
                //_rb.MovePosition(newPos);
                var onChainVelocity = playerState.Velocity;
                var onChainVelocityVec3 = onChainVelocity.ToVector3();
  
                if (onChainVelocityVec3 != _rb.velocity)
                {
                    Debug.Log($"FixedUpdate _rb.velocity { _rb.velocity }. correcting");

                    _rb.velocity = onChainVelocityVec3;
                    _rb.position = onChainPositionVec3;
                    transform.rotation  = Quaternion.Euler(new Vector3(0, Vector3.Angle(Vector3.forward, _rb.velocity), 0 ));
                    
                    Debug.Log($"FixedUpdate _rb.velocity { _rb.velocity }. corrected");

                }
                else if (Vector3.Distance(onChainPositionVec3, _rb.position) > 2f)
                {
                    //_rb.position = onChainPositionVec3;
                    // TODO calc elapsed time since last sync etc?
                    var newPos = Vector3.MoveTowards(_rb.position, onChainPositionVec3, 1f);
                    _rb.MovePosition(newPos);
                    Debug.Log($"FixedUpdate _rb.position { _rb.position }. corrected");

                }
  
                lastSyncedSequenceNumber = playerState.SequenceNumber;
                lastSyncedTimeStamp = Time.time;
            }
        }
    }

    private Vector2 GetOnChainPositionAsVec2()
    {
        var result = _rb.position;
        if (OnChainStateStore.States.ContainsKey(ownerAddress))
        {
            var playerState = OnChainStateStore.States[ownerAddress];
            lastSyncedSequenceNumber = playerState.SequenceNumber;
            
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
