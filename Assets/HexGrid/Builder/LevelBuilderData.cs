using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.U2D.ScriptablePacker;


[System.Serializable]
public class LevelBuilderData  {
    public int gridWidth;
    public int gridHeight;
    public SaveTile[] tiles;

    [System.Serializable]
    public struct SaveTile {
        public int q, r, s;
        public bool isPassable;
        public bool isInteractable;
        public string[] sprites;
        public string defaultHexSprite;

        public SaveTile(HexTile tile) {
            q = tile.q;
            r = tile.r;
            s = tile.s;
            isPassable = tile.isPassable;
            isInteractable = tile.isInteractable;

            sprites = new string[5];
            for(int i = 0; i < tile.spriteLayers.Length; i++) {
                SpriteRenderer renderer = tile.spriteLayers[i];
                if(renderer.sprite != null) {
                    sprites[i] = AssetDatabase.GetAssetPath(renderer.sprite);
                }
                else {
                    sprites[i] = "";
                }
            }

            defaultHexSprite = AssetDatabase.GetAssetPath(tile.defaultHexSprite);
        }

        public Vector3Int GetCupicCoord() {
            return new Vector3Int(q , r , s);
        }

        public Sprite[] GetSprites() {
            Sprite[] newSprites = new Sprite[sprites.Length];

            for(int i = 0; i < sprites.Length; i++) {
                if(sprites[i] != "") {
                    newSprites[i] = AssetDatabase.LoadAssetAtPath<Sprite>(sprites[i]);
                }
                else {
                    newSprites[i] = null;
                }
            }

            return newSprites;
        }

        public Sprite GetDefaultSprite() {
            return AssetDatabase.LoadAssetAtPath<Sprite>(defaultHexSprite);
        }
    }

    public void SaveGrid(HexGrid hexGrid) {
        gridWidth = hexGrid.gridWidth;
        gridHeight = hexGrid.gridHeight;

        tiles = new SaveTile[hexGrid.hexTiles.Count];
        int i = 0;
        foreach(KeyValuePair<Vector3Int , HexTile> tileInfo in hexGrid.hexTiles) {
            HexTile tile = tileInfo.Value;
            tiles[i] = new SaveTile(tile);
            i++;
        }
    }
}
