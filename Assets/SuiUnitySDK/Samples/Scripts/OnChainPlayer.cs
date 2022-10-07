using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class OnChainPlayer : MonoBehaviour
{
    public string ownerAddress;
    
    private Rigidbody2D _rb;
    private OnChainPlayerState _lastAppliedPlayerState;
    private ulong _lastSyncedSequenceNumber = 0;
    private ExplosionController _explosionController;
    private bool _isLocallyExploded;
    
    void Start()
    {
        _isLocallyExploded = false;
        _rb = GetComponent<Rigidbody2D>();
        _explosionController = GetComponent<ExplosionController>();
    }

    void FixedUpdate()
    {
        if (OnChainStateStore.States.ContainsKey(ownerAddress))
        {
            var playerState = OnChainStateStore.States[ownerAddress];
  
            if (playerState.SequenceNumber != _lastSyncedSequenceNumber && (!_isLocallyExploded || playerState.SequenceNumber == 0))
            {
                
                var onChainPosition = playerState.Position.ToVector2();
                var onChainVelocity = playerState.Velocity.ToVector2();

                if (onChainVelocity != _rb.velocity)
                {
                    _rb.velocity = onChainVelocity;
                    _rb.position = onChainPosition;
                    transform.rotation = Quaternion.Euler(new Vector3(0,0, Vector2.Angle(Vector2.up, _rb.velocity)));
                }

                if (playerState.IsExploded)
                {
                    _explosionController.Explode();
                    _isLocallyExploded = true;
                }
                else
                {
                    _isLocallyExploded = false;
                }
                
                _lastSyncedSequenceNumber = playerState.SequenceNumber;
            }
        }
    }
}
