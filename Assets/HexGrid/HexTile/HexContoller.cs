using System.Drawing;
using UnityEditor;
using UnityEngine;

public class HexController : MonoBehaviour {
    HexModel model;
    HexView view;

    [SerializeField] Sprite UnitHighlightSprite;

    public Vector3Int Coord => new Vector3Int(model.q, model.r, model.s);
    public bool Passable => model.isPassable;
    public UnitModel Unit => model.unit;
    public HexModel Model => model != null ? model.Copy() : null;
    public void SetSpriteLayer(int layer, Sprite sprite) => view.SetSpriteLayer(layer, sprite);

    // Initialization method
    public HexController Initialize(HexModel model, HexView view) {
        this.model = model;
        this.view = view;
        return this;
    }

    /*********************************
        Objects and Units
    *********************************/

    public void SetUnit(UnitModel unit) {
        model.unit = unit;
        view.SetObjectLayer(unit != null ? unit.UnitSprite : null);
        view.SetHighlightLayer(unit != null ? UnitHighlightSprite : null);
    }

    /*********************************
        Utility
    *********************************/

    public override string ToString() {
        return model.ToString();
    }

    private void OnDestroy() {
        if(view != null && view.spriteLayers != null) {
            foreach(var spriteRenderer in view.spriteLayers) {
                if(spriteRenderer != null && spriteRenderer.gameObject != null) {
                    Destroy(spriteRenderer.gameObject);
                }
            }
        }
    }

    /*********************************
        Saving/Loading
    *********************************/
    public static SaveTile GetSaveTile(HexController controller) {
        SaveTile newSave = new SaveTile();

        //Model Info
        HexModel model = controller.model;
        newSave.q = model.q;
        newSave.r = model.r;
        newSave.s = model.s;
        newSave.isPassable = model.isPassable;
        newSave.isInteractable = model.isInteractable;

        //View Info
        HexView view = controller.view;
        int spriteCount = view.spriteLayers.Length;

        newSave.sprites = new string[spriteCount];
        for(int i = 0; i < spriteCount; i++) {
            SpriteRenderer renderer = view.spriteLayers[i];
            if(renderer.sprite != null) {
                newSave.sprites[i] = AssetDatabase.GetAssetPath(renderer.sprite);
            }
            else {
                newSave.sprites[i] = "";
            }
        }

        return newSave;
    }


}    

[System.Serializable]
public struct SaveTile {
    public int q, r, s;
    public bool isPassable;
    public bool isInteractable;
    public string[] sprites;

    public Vector3Int GetCupicCoord() {
        return new Vector3Int(q, r, s);
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
}