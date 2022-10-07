using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ExplosionController : MonoBehaviour
{
    public GameObject objectToSetInactive;
    public Transform explosionRoot;
    public GameObject explosionPrefab;
    public float initialCollisionDetectionDelay;
    public bool useCollisionDetection;
    private bool _isCollisionDetectionEnabled;
    
    public bool IsExploded { get; private set; }

    public void Start()
    {
        _isCollisionDetectionEnabled = false;
        if (useCollisionDetection)
        {
            StartCoroutine(EnableDetectionAfterDelay(initialCollisionDetectionDelay));
        }
    }

    private IEnumerator EnableDetectionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _isCollisionDetectionEnabled = true;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (_isCollisionDetectionEnabled)
        {
            Explode();
        }
    }

    public void Explode()
    {
        var explosionEffect = Instantiate(explosionPrefab, explosionRoot);
        explosionEffect.transform.position = transform.position;
        explosionEffect.gameObject.SetActive(true);
        IsExploded = true;

        // TODO rework explosion to event based
        StartCoroutine(SetInactiveAfter(2f));
    }

    private IEnumerator SetInactiveAfter(float delay)
    {
        for(var i=0; i<objectToSetInactive.transform.childCount; i++)
        {
            objectToSetInactive.transform.GetChild(i).gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(delay);
        objectToSetInactive.SetActive(false);
    }
}
