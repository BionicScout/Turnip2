using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelBuilderData  {
    public int gridWidth;
    public int gridHeight;
    public SaveTile[] tiles;

    public void SaveGrid(HexGrid hexGrid) {
        gridWidth = hexGrid.gridWidth;
        gridHeight = hexGrid.gridHeight;

        tiles = new SaveTile[hexGrid.hexTiles.Count];
        int i = 0;
        foreach(KeyValuePair<Vector3Int , HexController> tileInfo in hexGrid.hexTiles) {
            HexController tile = tileInfo.Value;
            tiles[i] = HexController.GetSaveTile(tile);
            i++;
        }
    }
}
