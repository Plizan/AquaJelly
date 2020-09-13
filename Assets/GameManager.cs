using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{   
    public SpriteRenderer backgroundRenderer;

    public TextMeshProUGUI scoreText;
    public Image healthImage;

    [SerializeField] private float maxHealth;
    [SerializeField] private float health;
    public float Health { get => health; set{ health = Mathf.Min(value, maxHealth); healthImage.fillAmount = health / maxHealth; } }

    [SerializeField]private int score;
    public int Score { get => score; set { score = value; scoreText.text = score.ToString(); } }

    public float backgroundSpeed = 1f;

    private float backgroundOffset = 0f;

    private void Awake()
    {
    }

    private void Start()
    {
        backgroundOffset = 0f;
        health = maxHealth;
    }

    private void Update()
    {
        backgroundOffset -= backgroundSpeed * Time.deltaTime;
        backgroundOffset %= 1;
        backgroundRenderer.material.mainTextureOffset = new Vector2(backgroundOffset, 0);

        Health -= Time.deltaTime;

        if (health < 0) GameEnd();
    }

    private void GameEnd()
    {
        Destroy(Managers._gameObject);
        SceneManager.LoadScene(0);
    }
}