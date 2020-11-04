using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleInfo : MonoBehaviour
{
    public float damage;
    public int level;

    private void Start()
    {
        Invoke("RemoveObstacle", 5);
    }

    private void RemoveObstacle()
    {
        if (gameObject != null)
            Destroy(gameObject);
    }
}
