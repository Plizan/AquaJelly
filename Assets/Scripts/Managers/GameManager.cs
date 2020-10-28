using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Game Info")]
    public int score;
    //public int Score { get => score; set { score = value; Managers.UI.txtScore.text = score.ToString(); } }

    [Header("Player Info")]
    [SerializeField] private int level;

    public float maxHealth;
    public float health;
    //public float Health { get => health; set { health = Mathf.Min(value, maxHealth); Managers.UI.imgHealth.fillAmount = health / maxHealth; } }

    [Header("Background Field")]
    public GameObject[] backgrounds;

    private void Start()
    {
        Managers.UI.Initialization(this);
    }

    int backgroundIndex = 0;
    private void Update()
    {
        if(Camera.main.transform.position.x > 6.5f * (backgroundIndex + 1))
        {
            var pos = backgrounds[backgroundIndex % 3].transform.position;
            pos.x += 6.5f * backgrounds.Length;
            backgrounds[backgroundIndex % 3].transform.position = pos;

            backgroundIndex++;
        }
    }

    private void LateUpdate()
    {
        Managers.UI.Initialization(this);
    }
}