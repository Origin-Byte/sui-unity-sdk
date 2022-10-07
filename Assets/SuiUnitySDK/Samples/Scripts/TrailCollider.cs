using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
public class TrailCollider : MonoBehaviour
{
    public string ownerAddress;
    private ulong _lastSyncedSequenceNumber = 0;
    private EdgeCollider2D _edgeCollider;
    private List<Vector2> _points;
    private bool _isLocalPlayerTrail;
    
    public void Start()
    {
        if (string.IsNullOrWhiteSpace(ownerAddress))
        {
            ownerAddress = SuiWallet.GetActiveAddress();
            _isLocalPlayerTrail = true;
        }
        _edgeCollider = GetComponent<EdgeCollider2D>();
        _points = _edgeCollider.points.ToList();
    }

    void FixedUpdate()
    {
        if (OnChainStateStore.Instance.States.ContainsKey(ownerAddress))
        {
            var playerState = OnChainStateStore.Instance.States[ownerAddress];

            if (playerState.SequenceNumber != _lastSyncedSequenceNumber)
            {
                if (playerState.SequenceNumber == 0)
                {
                    _points = _edgeCollider.points.ToList();
                }
                
                // TODO optimization: merge points over the same line
                _points.Add(playerState.Position.ToVector2());
                _edgeCollider.SetPoints(_points);
                _lastSyncedSequenceNumber = playerState.SequenceNumber;

                if (playerState.IsExploded && !_isLocalPlayerTrail)
                {
                    Destroy(gameObject);
                }
            }
        }
        else if (!_isLocalPlayerTrail)
        {
            Destroy(gameObject);
        }
    }
}
