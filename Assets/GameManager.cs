using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI Field")]
    [SerializeField] private SpriteRenderer backgroundRenderer;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image healthImage;

    [SerializeField] private RectTransform panelTopGameUI;
    [SerializeField] private RectTransform panelBottomGameUI;
    [SerializeField] private RectTransform panelTopEndUI;
    [SerializeField] private RectTransform panelBottomEndUI;

    [Header("Game Info")]
    [SerializeField] private int score;
    public int Score { get => score; set { score = value; scoreText.text = score.ToString(); } }

    [Header("Player Info")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float health;
    [SerializeField] private int level;

    public float Health { get => health; set { health = Mathf.Min(value, maxHealth); healthImage.fillAmount = health / maxHealth; } }


    public float backgroundSpeed = 1f;

    private float backgroundOffset = 0f;

    private void Awake()
    {
    }

    private void Start()
    {
        backgroundOffset = 0f;
        health = maxHealth;

        panelTopEndUI.position = new Vector2(0, Screen.height / 2);
        panelBottomEndUI.position = new Vector2(0, Screen.height / 2 * -1);
    }

    private void Update()
    {
        backgroundOffset -= backgroundSpeed * Time.deltaTime;
        backgroundOffset %= 1;
        backgroundRenderer.material.mainTextureOffset = new Vector2(backgroundOffset, 0);

        Health -= Time.deltaTime;

        if (health < 0 && !isEnding) StartCoroutine(GameEndCoroutine());
    }

    bool isEnding = false;
    IEnumerator GameEndCoroutine()
    {
        isEnding = true;

        Destroy(Managers._gameObject);

        for (float f = 0; f < Mathf.PI; f += Time.deltaTime * 0.05f)
        {
            panelTopGameUI.position = new Vector2(0, Mathf.Sin(f) * Screen.height / 2);
            panelBottomGameUI.position = new Vector2(0, Mathf.Sin(f) * Screen.height / 2 * -1);
            panelTopEndUI.position = new Vector2(0, Mathf.Sin(Mathf.PI - f) * Screen.height / 2);
            panelBottomEndUI.position = new Vector2(0, Mathf.Sin(Mathf.PI - f) * Screen.height / 2 * -1);
            yield return null;
        }
    }
}