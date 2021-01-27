using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowSpawn : MonoBehaviour
{
    [SerializeField] public ThrowObstacle throwObstacle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            throwObstacle.isMove = true;
    }
}
