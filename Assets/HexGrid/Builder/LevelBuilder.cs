using UnityEngine;
using UnityEngine.U2D;

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

        GameEvents.current.SetCameraBounds(hexGrid);
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

            GameEvents.current.SetCameraBounds(hexGrid);
        }
    }

    public void SaveMap(string mapName) {
        data = new LevelBuilderData();
        data.SaveGrid(hexGrid);

        DataSaver dataSaver = new DataSaver();
        dataSaver.Save(mapName, data);

    }


    void Update() {

        if(Input.GetKeyDown(KeyCode.Equals)) {
            hexGrid.AddRow();
            GameEvents.current.SetCameraBounds(hexGrid);
        }
        if(Input.GetKeyDown(KeyCode.Minus)) {
            hexGrid.RemoveRow();
            GameEvents.current.SetCameraBounds(hexGrid);
        }
        if(Input.GetKeyDown(KeyCode.RightBracket)) {
            hexGrid.AddColumn();
            GameEvents.current.SetCameraBounds(hexGrid);
        }
        if(Input.GetKeyDown(KeyCode.LeftBracket)) {
            hexGrid.RemoveColumn();
            GameEvents.current.SetCameraBounds(hexGrid);
        }
    }




}
