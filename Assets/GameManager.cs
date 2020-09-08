using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{   
    public SpriteRenderer backgroundRenderer;

    public int score;

    public float backgroundSpeed = 1f;

    private float backgroundOffset = 0f;

    private void Awake()
    {
    }

    private void Start()
    {
        backgroundOffset = 0f;
    }

    private void Update()
    {
        backgroundOffset -= backgroundSpeed * Time.deltaTime;
        backgroundOffset %= 1;
        backgroundRenderer.material.mainTextureOffset = new Vector2(backgroundOffset, 0);
    }
}