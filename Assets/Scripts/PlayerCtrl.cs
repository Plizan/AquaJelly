using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public float power = 1f;
    private Rigidbody2D rig;

    private int jumpCount = 2;

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && jumpCount > 0)
        {
            jumpCount--;
            rig.velocity = new Vector2(0, power);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Floor")) jumpCount = 2;
    }
}
