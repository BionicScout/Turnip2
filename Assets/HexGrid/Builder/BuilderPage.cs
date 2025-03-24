using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuilderPage : TabGroup {
    public enum Layer { hexLayer = 0, backgroundLayer = 1, objectLayer = 2, foregroundLayer = 3 }
    public Layer layer;

    public GameObject hexGridObject;

    public List<Sprite> sprites = new List<Sprite>();

    private void Start() {
        for(int i = 0; i < Mathf.Min(tabButtons.Count, sprites.Count); i++) {
            ((BuilderItem)tabButtons[i]).UpdateSprite(sprites[i]);
        }
    }

    void Update() {
        if(Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && selectedTab != null) {

            HexGrid hexGrid = hexGridObject.GetComponent<HexGrid>();
            if(hexGrid == null) {
                return;
            }
            
            Debug.Log("Click");

            Vector3 mousePoint = Input.mousePosition;
            mousePoint.z = -Camera.main.transform.position.z;
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mousePoint);
            Vector2 gridPoint = new Vector2(worldPoint.x , worldPoint.y);
            Debug.Log(gridPoint);
            Vector3Int hexCoords = hexGrid.WorldToCubic(gridPoint);
            if(hexGrid.hexTiles.ContainsKey(hexCoords)) {
                HexTile hexTile = hexGrid.hexTiles[hexCoords];
                hexTile.SetSpriteLayer((int)layer, ((BuilderItem)selectedTab).GetSprite());
            }

        }
    }


}







//public class Input_MapSelection : MonoBehaviour {
//    public SO_TileInfo data;
//    public HexGrid hexGrid;

//    public void HandleTileClick(GameObject tileObj) {
//        HexUIList hexUIList = tileObj.GetComponent<HexUIList>();

//        if(hexUIList != null) {
//            // Example of accessing data
//            Debug.Log("Tile data: " + hexUIList.data.name);
//            data = hexUIList.data;
//            // Call other methods on hexUIList if needed
//        }
//        else {
//            Debug.LogWarning("HexUIList component not found on tile object.");
//        }
//    }


//}

