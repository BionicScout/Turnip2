//using UnityEngine;

//public class HexController : MonoBehaviour {
//    public int q, r, s;
//    public bool isPassable = true;
//    public bool isInteractable = false;
//    public SpriteRenderer[] spriteLayers; //Layers from bottom to top: tile base, background decoration, object, forground decoration, highlight
//    public Sprite defaultHexSprite;
//    public HexObject hexObject = null;
//    public UnitModel unit = null;

//    public static readonly Vector3Int[] directions = {
//        new Vector3Int(-1, 1, 0), new Vector3Int(0, 1, -1), new Vector3Int(1, 0, -1), 
//        new Vector3Int(1, -1, 0), new Vector3Int(0, -1, 1), new Vector3Int(-1, 0, 1)
//    };

//    /*********************************
//        Set Methods
//    *********************************/

//    // Cube Coord Constructor
//    public void SetHexController(int x , int y , int z) {
//        if(x + y + z != 0) {
//            Debug.LogError("Cube coordinates do not sum to 0");
//        }
//        q = x;
//        r = y;
//        s = z;
//        Initialize();
//    }

//    // Cube Coord Constructor using Vector3Int
//    public void SetHexController(Vector3Int coords) {
//        if(coords.x + coords.y + coords.z != 0) {
//            Debug.LogError("Cube coordinates do not sum to 0");
//        }
//        q = coords.x;
//        r = coords.y;
//        s = coords.z;
//        Initialize();
//    }

//    // Axial Coord Constructor
//    public void SetHexController(int x , int y) {
//        q = x;
//        r = y;
//        s = -x - y;
//        Initialize();
//    }

//    // Initialization method
//    public void Initialize() {
//        InitializeSpriteLayers(5);
//        SetSpriteLayer(0 , defaultHexSprite);
//        gameObject.name = "(" + q + ", " + r + ", " + s + ")";
//    }

//    /******************************************************************
//        SpriteRender and Sprite Layers
//    ******************************************************************/

//    public void InitializeSpriteLayers(int layerCount) {
//        spriteLayers = new SpriteRenderer[layerCount];
//        for(int i = 0; i < layerCount; i++) {
//            GameObject layerObject = new GameObject("Layer" + i);
//            layerObject.transform.SetParent(this.transform , false);
//            spriteLayers[i] = layerObject.AddComponent<SpriteRenderer>();
//            spriteLayers[i].sortingOrder = i; // Ensures correct rendering order
//        }
//    }

//    public void SetSpriteLayer(int layer , Sprite sprite) {
//        Debug.Assert(sprite != null || layer != 0, "HexController[SetSpriteLayer]: sprite is null for background hex");
//        Debug.Assert(layer >= 0 && layer < 5, "HexController[SetSpriteLayer]: layer is invalid value \"" + layer + "\"");

//        spriteLayers[layer].sprite = sprite;

//    }

//    public void SetAllSpriteLayers(Sprite[] sprites) {
//        Debug.Assert(sprites != null, "HexController[SetAllSpriteLayers]: sprites array is null");
//        Debug.Assert(sprites.Length == 5, "HexController[SetAllSpriteLayers]: sprites array is not length of 5");

//        for(int i = 0; i < sprites.Length; i++) {
//            SetSpriteLayer(i, sprites[i]);
//        }
//    }

//    public Sprite GetSpriteLayer(int layer) {
//        if(layer < spriteLayers.Length) {
//            return spriteLayers[layer].sprite;
//        }
//        Debug.LogWarning("Layer index out of range");
//        return null;
//    }

//    /*********************************
//        Utiliy
//    *********************************/

//    public Vector3Int GetCoords() {
//        return new Vector3Int(q , r , s);
//    }

//    public void SetHexObject(UnitModel newUnit) {
//        unit = newUnit;

//        if(unit == null) {
//            SetSpriteLayer(2, null);
//            return;
//        }

//        SetSpriteLayer(2, unit.UnitSprite);
//    }
//}