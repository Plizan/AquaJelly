using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{


    public GameObject jellyEffectPrefab;

    public List<GameObject> jellyEffectPool = new List<GameObject>();

    private void Start()
    {
        CreateJellyEffcts();
    }


    void CreateJellyEffcts()
    {
        for (int i = 0; i < 50; i++)
        {
            GameObject effect = (GameObject)Instantiate(jellyEffectPrefab);
            jellyEffectPool.Add(effect);
            effect.SetActive(false);
        }
    }

    public GameObject[] warn;


}
