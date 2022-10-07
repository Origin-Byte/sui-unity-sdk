using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(EdgeCollider2D))]
public class TrailCollider : MonoBehaviour
{
    public LineRenderer lineRenderer1;
    public LineRenderer lineRenderer2;
    public string ownerAddress;
    private ulong _lastSyncedSequenceNumber = 0;
    private EdgeCollider2D _edgeCollider;
    private List<Vector2> _points;
    private bool _isEnemyTrail;
    private List<Vector3> _lineRendererPoints;
    
    public void Start()
    {
        if (string.IsNullOrWhiteSpace(ownerAddress))
        {
            ownerAddress = SuiWallet.GetActiveAddress();
        }
        else
        {
            _isEnemyTrail = true;
            _lineRendererPoints = new List<Vector3>();
        }
        _edgeCollider = GetComponent<EdgeCollider2D>();
        _edgeCollider.Reset();
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

                if (_isEnemyTrail)
                {
                    _lineRendererPoints.Add(playerState.Position.ToVector3());
                    lineRenderer1.positionCount = _lineRendererPoints.Count;
                    lineRenderer1.SetPositions(_lineRendererPoints.ToArray());
                    lineRenderer2.positionCount = _lineRendererPoints.Count;
                    lineRenderer2.SetPositions(_lineRendererPoints.ToArray());
                }
            }
        }
    }
}
