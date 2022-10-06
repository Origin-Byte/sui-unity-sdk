using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class OnChainPlayer : MonoBehaviour
{
    public string ownerAddress;
    
    private Rigidbody2D _rb;
    private OnChainPlayerState _lastAppliedPlayerState;
    private ulong _lastSyncedSequenceNumber = 0;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (OnChainStateStore.States.ContainsKey(ownerAddress))
        {
            var playerState = OnChainStateStore.States[ownerAddress];
  
            if (playerState.SequenceNumber != _lastSyncedSequenceNumber)
            {
                var onChainPosition = playerState.Position.ToVector2();
                var onChainVelocity = playerState.Velocity.ToVector2();

                if (onChainVelocity != _rb.velocity)
                {
                    _rb.velocity = onChainVelocity;
                    _rb.position = onChainPosition;
                    transform.rotation  = Quaternion.Euler(new Vector3(0,0, Vector2.Angle(Vector2.up, _rb.velocity) ));
                }

                _lastSyncedSequenceNumber = playerState.SequenceNumber;
            }
        }
    }
}
