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
    
    public void Start()
    {
        if (string.IsNullOrWhiteSpace(ownerAddress))
        {
            ownerAddress = SuiWallet.GetActiveAddress();
        }
        _edgeCollider = GetComponent<EdgeCollider2D>();
        _points = _edgeCollider.points.ToList();
    }

    void FixedUpdate()
    {
        if (OnChainStateStore.States.ContainsKey(ownerAddress))
        {
            var playerState = OnChainStateStore.States[ownerAddress];

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
            }
        }
    }
}
