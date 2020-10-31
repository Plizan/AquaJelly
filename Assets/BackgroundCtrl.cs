using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class BackgroundCtrl : MonoBehaviour
{

    [SerializeField]private GameObject[] backgroundObjects;
    [SerializeField] private float delay;

    private Renderer[] renderers;

    private void Awake()
    {
        renderers = new Renderer[transform.childCount];

        for (int i = 0; i < transform.childCount - 1; i++)
            renderers[i] = transform.GetChild(i).GetComponent<Renderer>();
    }

    private void Update()
    {
        for(int i = 0; i < renderers.Length; i++)
        {
            //renderers[i].material.mainTextureOffset += new Vector2(Time.deltaTime * i * delay, 0f);
            //backgroundObjects[i].transform.Translate(Vector3.right * i * Time.deltaTime * delay);
        }
    }

}
