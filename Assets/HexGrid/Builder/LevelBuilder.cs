using System;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class LevelBuilder : MonoBehaviour {
    public GameObject hexGridObject;
    public GameObject hexTilePrefab;
    public Sprite defaultHex;
    HexGrid hexGrid;


    private LevelBuilderData data;

    public static LevelBuilder instance { get; private set; }
    private void Awake() {
        Debug.Assert(instance == null, "Found more than one LevelBuilder in the scene");
        instance = this;
    }

    public void NewMap() {
        data = new LevelBuilderData();
        if(hexGridObject.GetComponent<HexGrid>() != null) {
            Destroy(hexGridObject.GetComponent<HexGrid>());
        }
        hexGrid = hexGridObject.AddComponent<HexGrid>();
        hexGrid.Instance(hexTilePrefab, defaultHex);
    }

    public void LoadMap(string mapName) {
        DataSaver dataSaver = new DataSaver();
        data = dataSaver.Load(mapName);

        if(data == null) {
            NewMap();
            Debug.Log("Null");
        }
        else {
            Debug.Log("Load Map");
            if(hexGridObject.GetComponent<HexGrid>() != null) {
                Destroy(hexGridObject.GetComponent<HexGrid>());
            }
            hexGrid = hexGridObject.AddComponent<HexGrid>();
            hexGrid.Instance(hexTilePrefab , defaultHex, data);
        }
    }

    public void SaveMap(string mapName) {
        data = new LevelBuilderData();
        data.SaveGrid(hexGrid);

        DataSaver dataSaver = new DataSaver();
        dataSaver.Save(mapName, data);

    }







}
