using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Field")]
    public Text txtScore;
    public Image imgHealth;

    public void Initialization(GameManager game)
    {
        if(txtScore)
            txtScore.text = game.score.ToString();

        if(imgHealth)
            imgHealth.fillAmount = game.health / game.maxHealth;
    }
}
