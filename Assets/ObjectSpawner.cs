using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectPrefab;
    public float waitSecond;

    private void Start()
    {
        StartCoroutine(ObjectSpawn());
    }

    IEnumerator ObjectSpawn()
    {
        yield return new WaitForSeconds(waitSecond);

        Instantiate(objectPrefab, transform.position, transform.rotation);

        StartCoroutine(ObjectSpawn());
    }
}
