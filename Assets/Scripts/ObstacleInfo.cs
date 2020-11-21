using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleInfo : MonoBehaviour
{
    public float damage = 100;
    public int minLevel;
    public int maxLevel;

    private void RemoveObstacle()
    {
        if (gameObject != null)
            Destroy(gameObject);
    }
}
