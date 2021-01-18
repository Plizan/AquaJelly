using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MapEditor
{
    public class MapEditor : MonoBehaviour
    {
        public const string mapPath = "Assets/Maps/";
        public InputField inputFloorCount;
        public InputField inputMapName;
        
        public Image imgState;

        public GameObject objFloor;
        public GameObject objMap;
        public GameObject objMenu;
        
        private void Start()
        {
            imgState.DOColor(Color.white, 1).SetEase(Ease.InSine);
        }           

        private Vector2 _oldMousePos;
        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                _oldMousePos = Input.mousePosition;
            }
            if (Input.GetMouseButton(1))
            {
                var mousePosition = Input.mousePosition;
                transform.Translate(new Vector3(mousePosition.x - _oldMousePos.x,mousePosition.y - _oldMousePos.y, 0) * Time.deltaTime);
                _oldMousePos = mousePosition;
            }
        }

        public void ObjectInstantiate()
        {
            
        }
        
        public void OnCreatFloor()
        {
            var value = int.Parse(inputFloorCount.text);

            var objFloors = objMap.transform.Find("Floors");
            
            for (int i = 0; i < objFloors.transform.childCount; i++)
                Destroy(objFloors.GetChild(i).gameObject);
            
            objFloor.SetActive(true);
            
            for (int i = 0; i < value; i++)
            {
                var obj = Instantiate(objFloor, new Vector3(6.5f, 0f, 0f) * i, Quaternion.identity);
                obj.transform.SetParent(objFloors.transform);
            }
            
            objFloor.SetActive(false);
        }

        public void OnMenu()
        {
            objMenu.SetActive(!objMenu.activeSelf);
        }
        
        public void SaveMap()
        {
            var name = inputMapName.text;
            UnityEditor.PrefabUtility.SaveAsPrefabAsset(objMap, mapPath + name, out bool isSuccess);
            UnityEditor.AssetDatabase.Refresh();

            imgState.DOColor(isSuccess ? new Color(0.47f, 1f, 0.41f, 0.5f) : new Color(1f, 0.36f, 0.38f, 0.5f), 1).SetEase(Ease.InSine);
            imgState.DOColor(Color.white, 1).SetDelay(5);
        }

        public void LoadMap()
        {
            var name = inputMapName.text;
            var map = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(mapPath + name);
            
            imgState.DOColor(map != null ? new Color(0.47f, 1f, 0.41f, 0.5f) : new Color(1f, 0.36f, 0.38f, 0.5f), 1).SetEase(Ease.InSine);
            imgState.DOColor(Color.white, 1).SetDelay(5);
            
            if (map == null) return;
            
            objMap = Instantiate(map) as GameObject;
            
        }
    }
}