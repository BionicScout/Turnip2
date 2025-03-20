using UnityEngine;

public class HexTile : MonoBehaviour {
    public int q, r, s;
    public bool isPassable = true;
    public bool isInteractable = false;
    public SpriteRenderer[] spriteLayers; //Layers from bottom to top: tile base, background decoration, object, forground decoration, highlight
    public Sprite defaultHexSprite;

    /*********************************
        Set Methods
    *********************************/

    // Cube Coord Constructor
    public void SetHexTile(int x , int y , int z) {
        if(x + y + z != 0) {
            Debug.LogError("Cube coordinates do not sum to 0");
        }
        q = x;
        r = y;
        s = z;
        Initialize();
    }

    // Cube Coord Constructor using Vector3Int
    public void SetHexTile(Vector3Int coords) {
        if(coords.x + coords.y + coords.z != 0) {
            Debug.LogError("Cube coordinates do not sum to 0");
        }
        q = coords.x;
        r = coords.y;
        s = coords.z;
        Initialize();
    }

    // Axial Coord Constructor
    public void SetHexTile(int x , int y) {
        q = x;
        r = y;
        s = -x - y;
        Initialize();
    }

    // Initialization method
    public void Initialize() {
        InitializeSpriteLayers(5);
        SetSpriteLayer(0 , defaultHexSprite);
        gameObject.name = "(" + q + ", " + r + ", " + s + ")";
    }

    /******************************************************************
        SpriteRender and Sprite Layers
    ******************************************************************/

    public void InitializeSpriteLayers(int layerCount) {
        spriteLayers = new SpriteRenderer[layerCount];
        for(int i = 0; i < layerCount; i++) {
            GameObject layerObject = new GameObject("Layer" + i);
            layerObject.transform.SetParent(this.transform , false);
            spriteLayers[i] = layerObject.AddComponent<SpriteRenderer>();
            spriteLayers[i].sortingOrder = i; // Ensures correct rendering order
        }
    }

    public void SetSpriteLayer(int layer , Sprite sprite) {
        Debug.Assert(sprite != null, "HexTile[SetSpriteLayer]: sprite is null");
        Debug.Assert(layer >= 0 && layer < 5, "HexTile[SetSpriteLayer]: layer is invalid value \"" + layer + "\"");

        if(layer < spriteLayers.Length) {
            spriteLayers[layer].sprite = sprite;
        }
        else {
            Debug.LogWarning("Layer index out of range");
        }
    }

    public void SetAllSpriteLayers(Sprite[] sprites) {
        Debug.Assert(sprites != null, "HexTile[SetAllSpriteLayers]: sprites array is null");
        Debug.Assert(sprites.Length == 5, "HexTile[SetAllSpriteLayers]: sprites array is not length of 5");

        for(int i = 0; i < sprites.Length; i++) {
            SetSpriteLayer(i, sprites[i]);
        }
    }

    public Sprite GetSpriteLayer(int layer) {
        if(layer < spriteLayers.Length) {
            return spriteLayers[layer].sprite;
        }
        Debug.LogWarning("Layer index out of range");
        return null;
    }

    /*********************************
        Utiliy
    *********************************/

    public Vector3Int getCoords() {
        return new Vector3Int(q , r , s);
    }
}
