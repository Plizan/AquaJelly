using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum ProgressType
{
    Lobby = 0,
    Game,

    None,
}

public class GameManager : MonoBehaviour
{
    #region Field Values
    [Header("Game Info")]
    [SerializeField] private ProgressType progressType;
    public ProgressType ProgressType { get => progressType; set { progressType = value; Managers.UI.Initialization(progressType); } }

    [SerializeField] private int score;
    public int Score { get => score; set { score = value; Managers.UI.txtScore.text = score.ToString(); } }

    [Header("Player Info")]
    [SerializeField] private GameObject player;

    [SerializeField] private int level;

    [SerializeField] float maxHealth;
    [SerializeField] private float health;
    public float Health { get => health; set { health = Mathf.Min(value, maxHealth); Managers.UI.imgHealth.fillAmount = health / maxHealth; } }

    [Header("Background Field")]
    public GameObject[] backgrounds;
    #endregion

    #region CallbackFunction
    private void Start()
    {
        ProgressType = progressType;
        beginningCameraPosition = Camera.main.transform.position;
    }

    Vector3 beginningCameraPosition;
    int backgroundIndex = 0;
    private void Update()
    {
        Vector3 temp = beginningCameraPosition;
        temp.x += player.transform.position.x;
        Camera.main.transform.position = temp;

        if (Camera.main.transform.position.x > 6.5f * (backgroundIndex + 1))
        {
            var pos = backgrounds[backgroundIndex % 3].transform.position;
            pos.x += 6.5f * backgrounds.Length;
            backgrounds[backgroundIndex % 3].transform.position = pos;

            backgroundIndex++;
        }
    }
    #endregion
}