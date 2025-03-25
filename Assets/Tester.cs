using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;


public class Tester : MonoBehaviour {
    public GameObject hexGridObject;
    public Sprite highlight;
    public int range = 2;
    List<HexTile> tiles = new List<HexTile>();
    HexTile directTarget1 = null;
    HexTile directTarget2 = null;


    private void Start() {
        hexGridObject = FindAnyObjectByType<HexGrid>().gameObject;     
    }


    void Update() {
        if(Input.GetMouseButtonDown(0)) {
            foreach(HexTile tile in tiles) {
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
            Vector3Int hexCoords = hexGrid.WorldToCubic(gridPoint);

            List<HexTile> hexTiles = hexGrid.GetAllTiles();
            HexTile selectedTile = hexTiles.FirstOrDefault(tile => tile.GetCoords() == hexCoords);

            if(selectedTile != null) {
                foreach(HexTile tile in tiles) {
                    tile.SetSpriteLayer(4, null);
                }

                var timer = new Stopwatch();
                timer.Start();
                tiles = Pathfinding.AllTilesInRange(hexTiles, selectedTile, range, false);
                timer.Stop();
                TimeSpan timeTaken = timer.Elapsed;
                string foo = "Time taken: " + timeTaken.ToString(@"m\:ss\.fff");
                UnityEngine.Debug.Log(foo);

                
                foreach(HexTile tile in tiles) {
                    tile.SetSpriteLayer(4, highlight);
                }
                
            }

        }


        if(Input.GetMouseButtonDown(1)) {
            foreach(HexTile tile in tiles) {
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
            Vector3Int hexCoords = hexGrid.WorldToCubic(gridPoint);

            List<HexTile> hexTiles = hexGrid.GetAllTiles();
            HexTile selectedTile = hexTiles.FirstOrDefault(tile => tile.GetCoords() == hexCoords);

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
                    HexTile tile2 = hexTiles.FirstOrDefault(t => t.GetCoords() == coord);
                    tiles.Add(tile2);
                    tile2.SetSpriteLayer(4, highlight);
                }

            }

        }
    }
}
