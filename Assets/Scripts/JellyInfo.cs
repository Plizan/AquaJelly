using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyInfo : MonoBehaviour
{
    public int score;
    public float heal;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody != null) Destroy(gameObject);
    }
}
