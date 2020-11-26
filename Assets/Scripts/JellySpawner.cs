using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellySpawner : MonoBehaviour
{
    public GameObject jelly;
    public float waitSecond;

    private void Start()
    {
        //StartCoroutine("JellySpawn");
    }

    IEnumerator JellySpawn()
    {
        yield return new WaitForSeconds(waitSecond);
        Instantiate(jelly, transform.position, transform.rotation);
        StartCoroutine("JellySpawn");
    }
}
