using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TitleManager : MonoBehaviour
{

    public void OnStart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}