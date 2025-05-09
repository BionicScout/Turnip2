using static HexUtil.HexMath;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;


public class Tester : MonoBehaviour {
    public GameObject hexGridObject;
    public Sprite highlight;
    public int range = 2;
    List<HexController> tiles = new List<HexController>();
    HexController directTarget1 = null;
    HexController directTarget2 = null;
    public Sprite UnitSprite;

    [SerializeField] List<SO_UnitModel> unitsToLoad;


    private void Start() {
        hexGridObject = FindAnyObjectByType<HexGrid>().gameObject;


        //testPU.state = UnitModel.State.move;
        //GameEvents.current.PlayerSelect(testPU, ActionButton);
        
        //Create Units
        List<UnitModel> units = new List<UnitModel>();
        for(int i = 0; i < unitsToLoad.Count; i++) {
            SO_UnitModel so_u = unitsToLoad[i];
            HexController tile = hexGridObject.GetComponent<HexGrid>().GetTile(new Vector3Int(1,0, -1) * i);
            UnitModel u = UnitModel.CreateUnit(so_u, tile);
            tile.SetUnit(u);
        }

        //EventBus<SelectUnitEvent>.Raise(new SelectUnitEvent { unit = units[0] });
        EventBus<LoadUnitsOnStartEvent>.Raise(new LoadUnitsOnStartEvent { units = units });

        //GameEvents.current.OnTileSelected += LeftClick;
    }


    void Update() {

        if(Input.GetMouseButtonDown(1)) {
            foreach(HexController tile in tiles) {
                tile.SetSpriteLayer(4, null);
            }
            tiles.Clear();

            HexGrid hexGrid = hexGridObject.GetComponent<HexGrid>();
            if(hexGrid == null) {
                return;
            }

            UnityEngine.Debug.Log("Click");

            Vector3 mousePoint = Input.mousePosition;
            mousePoint.z = -Camera.main.transform.position.z;
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mousePoint);
            Vector2 gridPoint = new Vector2(worldPoint.x, worldPoint.y);
            UnityEngine.Debug.Log(gridPoint);
            Vector3Int hexCoords = WorldToCubic(gridPoint, hexGrid.hexDimensions);

            List<HexController> hexTiles = hexGrid.GetAllTiles();
            HexController selectedTile = hexTiles.FirstOrDefault(tile => tile.Coord == hexCoords);

            if(selectedTile != null) {
                directTarget1 = directTarget2;
                directTarget2 = selectedTile;

                if(directTarget1 == null || directTarget2 == null) { return; }

                var timer = new Stopwatch();
                timer.Start();
                List<Vector3Int> temp = Pathfinding.PathBetweenPoints(hexTiles, directTarget1, directTarget2, false);
                timer.Stop();
                TimeSpan timeTaken = timer.Elapsed;
                string foo = "Time taken: " + timeTaken.ToString(@"m\:ss\.fff");
                UnityEngine.Debug.Log(foo);

                foreach(Vector3Int coord in temp) {
                    HexController tile2 = hexTiles.FirstOrDefault(t => t.Coord == coord);
                    tiles.Add(tile2);
                    tile2.SetSpriteLayer(4, highlight);
                }

            }

        }

    }

    public void LeftClick(HexController selectedTile) {
        //foreach(HexController tile in tiles) {
        //    tile.SetSpriteLayer(4, null);
        //}
        //tiles.Clear();

        //if(selectedTile == null) {
        //    return;
        //}

        //HexGrid hexGrid = hexGridObject.GetComponent<HexGrid>();
        //if(hexGrid == null) {
        //    return;
        //}

        //UnityEngine.Debug.Log("Click");

        //if(selectedTile != null) {
        //    List<HexController> hexTiles = hexGrid.GetAllTiles();
        //    var timer = new Stopwatch();
        //    timer.Start();
        //    tiles = Pathfinding.AllTilesInRange(hexTiles, selectedTile, range, false);
        //    timer.Stop();
        //    TimeSpan timeTaken = timer.Elapsed;
        //    string foo = "Time taken: " + timeTaken.ToString(@"m\:ss\.fff");
        //    UnityEngine.Debug.Log(foo);


        //    foreach(HexController tile in tiles) {
        //        tile.SetSpriteLayer(4, highlight);
        //    }
        //}


        UnitModel tileUnit = selectedTile.Unit;
        if(tileUnit == null) { return; }
        EventBus<SelectUnitEvent>.Raise(new SelectUnitEvent { unit = tileUnit });
    }


}
