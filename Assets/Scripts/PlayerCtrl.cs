using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private float power = 1;

    private Rigidbody2D _rigidbody;
    private int jumpCount = 0;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        transform.Translate(Vector2.right * Time.deltaTime * speed);

        if (Input.GetMouseButtonDown(0) && jumpCount < 2)
        {
            jumpCount++;
            _rigidbody.velocity = Vector2.up * power;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Floor"))
            jumpCount = 0;
    }
}
