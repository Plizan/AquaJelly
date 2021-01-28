using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MapManager : MonoBehaviour
{
    private class LevelInfo
    {
        private int id;
        private List<int> mapLevel;

        public LevelInfo(XmlNodeList list)
        {
            mapLevel = new List<int>();

            foreach (XmlNode level in list) ;
            //mapLevel.Add(level.Value);
        }
    }

    [SerializeField] private GameObject firstFloor;
    [SerializeField] private List<Material> matBackground;
    [SerializeField] private Material matFloor;
    [SerializeField] private BackgroundCtrl backgroundCtrl;
    [SerializeField] private int[] testLevel;
    
    private List<GameObject> floors = new List<GameObject>();

    private Texture[,] textures;

    [SerializeField] private GameObject[] prefabMaps;
    [SerializeField] private List<GameObject> maps = new List<GameObject>();
    private GameObject mapObject;
    private List<LevelInfo> levelInfos;

    private void Awake()
    {
        var maxLevel = Managers.Game.maxStageLevel;

        matBackground = new List<Material>();
        matBackground.Add(matFloor);
        matBackground.AddRange(backgroundCtrl.GetMatarials.Reverse().ToList());
        
        textures = new Texture[maxLevel, matBackground.Count];

        for (int i = 0; i < maxLevel; i++)
        {
            var textures = Resources.LoadAll<Texture>($"Image/Background/{i + 1}");

            for (int j = 0; j < matBackground.Count; j++)
                this.textures[i, j] = textures[j];
        }
    }

    public void Init()
    {
        //LoadFloors();
        SetFloors(Managers.Game.stageLevel);
        SetLevel();
    }

    public void SetFloors(int level)
    {
        level = Mathf.Clamp(level, 1, Managers.Game.maxStageLevel);

        for (int i = 0; i < matBackground.Count; i++)
            matBackground[i].mainTexture = textures[level - 1, i];
    }

    public void DeleteLevel()
    {
        if (maps != null && maps.Count > 0)
        {
            foreach (var map in maps)
            {
                Destroy(map.gameObject);
            }
        }
    }
    
    public void SetLevel()
    {
        maps = new List<GameObject>();
        
        var length = 7;
        
        for (int i = 0; i < testLevel.Length; i++)
        {
            var maps = prefabMaps.Where(x => x.GetComponent<MapInfo>().level == testLevel[i]).ToList();
            
            if (maps.Count() < 1) continue;
            
            var obj = Instantiate(maps[Random.Range(0, maps.Count())]);
            obj.transform.position = new Vector3(length, 0, 0);
            length += obj.GetComponent<MapInfo>().floors.transform.childCount * 7;
            
            this.maps.Add(obj);
        }

        {
            var obj = Instantiate(firstFloor);
            obj.transform.position = new Vector3(length, 0, 0);
            maps.Add(obj);
            
            obj = Instantiate(firstFloor);
            obj.transform.position = new Vector3(length + 7, 0, 0);
            maps.Add(obj);
        }
        
        Managers.Game.endXLength = length;
    }

    private void LoadFloors()
    {
        floors = new List<GameObject>();

        floors.Add(firstFloor);

        if (mapObject == null) return;

        var objFloor = mapObject.GetComponent<MapInfo>().floors;

        for (int i = 0; i < objFloor.transform.childCount; i++)
        {
            floors.Add(objFloor.transform.GetChild(i).gameObject);
        }
    }

    private void LoadLevelInfo()
    {
        levelInfos = new List<LevelInfo>();

        var xml = new XmlDocument();
        xml.Load(Application.dataPath + "Xml/LevelInfo.xml");

        foreach (XmlNode table in xml.SelectNodes("LevelInfo/Level"))
        {
            //levelInfos.Add(new LevelInfo(table.ChildNodes("MapLevel")));
        }
    }
}