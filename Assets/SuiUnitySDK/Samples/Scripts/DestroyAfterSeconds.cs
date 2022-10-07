using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    public float seconds;
    
    void Start()
    {
        StartCoroutine(DestroyAfterSecondsCoroutine(seconds));
    }

    private IEnumerator DestroyAfterSecondsCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
