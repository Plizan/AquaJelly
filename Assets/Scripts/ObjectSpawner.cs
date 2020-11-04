using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] objectPrefabs;
    
    List<GameObject> objects = new List<GameObject>();
    
    public IEnumerator ObjectSpawn(float waitSecond)
    {
        yield return new WaitForSeconds(3);


        while (true)
        {
            yield return new WaitForSeconds(Random.Range(waitSecond - 1, waitSecond + 1));

            objects.Add(Instantiate(objectPrefabs[Random.Range(0, objectPrefabs.Length)], transform.TransformPoint(Vector3.zero), Quaternion.identity));
        }
    }

    public void objectRemove()
    {
        objects.ForEach((i) =>
        {
            if (i != null) Destroy(i);
        });
    }
}
