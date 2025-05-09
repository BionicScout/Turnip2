using UnityEngine;

public class HexView {
    //Layers from bottom to top: tile base, background decoration, object, foreground decoration, highlight
    public SpriteRenderer[] spriteLayers;
    const int LAYER_COUNT = 5;

    private HexView() { }

    public static HexView InitializeHexView(Transform transform, Sprite baseTile) {
        HexView view = new HexView();

        //Create all sprite Layers
        view.spriteLayers = new SpriteRenderer[LAYER_COUNT];
        for(int i = 0; i < LAYER_COUNT; i++) {
            GameObject layerObject = new GameObject("Layer" + i);
            layerObject.transform.SetParent(transform, false);
            view.spriteLayers[i] = layerObject.AddComponent<SpriteRenderer>();
            view.spriteLayers[i].sortingOrder = i; // Ensures correct rendering order
        }

        //Set Base layer
        view.SetBaseLayer(baseTile);

        return view;
    }

    public void SetSpriteLayer(int layer, Sprite sprite) {
        Debug.Assert(sprite != null || layer != 0, "HexView[SetSpriteLayer]: sprite is null for background hex");
        Debug.Assert(layer >= 0 && layer < 5, "HexView[SetSpriteLayer]: layer is invalid value \"" + layer + "\"");

        spriteLayers[layer].sprite = sprite;
    }

    public void SetAllSpriteLayers(Sprite[] sprites) {
        Debug.Assert(sprites != null, "HexView[SetAllSpriteLayers]: sprites array is null");
        Debug.Assert(sprites.Length == 5, "HexView[SetAllSpriteLayers]: sprites array is not length of 5");

        for(int i = 0; i < sprites.Length; i++) {
            SetSpriteLayer(i, sprites[i]);
        }
    }

    public void SetBaseLayer(Sprite baseSprite) {
        Debug.Assert(baseSprite != null, "HexView[SetBaseLayer]: base sprite is null");
        SetSpriteLayer(0, baseSprite);
    }

    public void SetBackgroundLayer(Sprite backgroundSprite) {
        SetSpriteLayer(1, backgroundSprite);
    }

    public void SetObjectLayer(Sprite objectSprite) {
        SetSpriteLayer(2, objectSprite);
    }

    public void SetForegroundLayer(Sprite foregroundSprite) {
        SetSpriteLayer(3, foregroundSprite);
    }

    public void SetHighlightLayer(Sprite highlightSprite) {
        SetSpriteLayer(4, highlightSprite);
    }
}
