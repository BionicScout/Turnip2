using UnityEngine;

public class HexTileBuilder {
    HexModel tile;
    HexView view;
    GameObject gameObject;

    /*********************************
        Constructors
    *********************************/
    public HexTileBuilder(int x, int y, int z, Sprite baseLayer) {
        Debug.Assert(baseLayer != null, "HexTileBuilder: baseLayer is null!");

        //Create Model
        tile = new HexModel();
        SetCoord(x, y, z);

        //Create Game Object
        gameObject = new GameObject($"({tile.q}, {tile.r}, {tile.s})");

        //Create View
        view = HexView.InitializeHexView(gameObject.transform, baseLayer);
    }

    public HexTileBuilder(Vector3Int coords, Sprite baseLayer) 
        : this(coords.x, coords.y, coords.z, baseLayer) { }    
    public HexTileBuilder(int x, int y, Sprite baseLayer) 
        : this(x, y, -x - y, baseLayer) { }


    void SetCoord(int q, int r, int s) {
        Debug.Assert(q + r + s == 0, "HexTileBuilder[SetCoord]: Cube coordinates do not sum to 0");

        tile.q = q;
        tile.r = r;
        tile.s = s;
    }
    /*********************************
        Building
    *********************************/
    public HexTileBuilder WithWorldLocation(Vector3 loc) {
        gameObject.transform.position = loc;
        return this;
    }

    public HexTileBuilder WithPassability(bool pass) {
        tile.isPassable = pass;
        return this;
    }

    public HexTileBuilder WithInteractability(bool interact) {
        tile.isInteractable = interact;
        return this;
    }

    public HexTileBuilder WithObject(HexObject hexObject) {
        tile.hexObject = hexObject;
        //view.SetObjectLayer(hexObject.sprite);
        return this;
    }

    public HexTileBuilder WithUnit(UnitModel unit) {
        tile.unit = unit;
        view.SetObjectLayer(unit != null ? unit.UnitSprite : null);
        return this;
    }

    public HexTileBuilder WithSprites(Sprite[] sprites) {
        view.SetAllSpriteLayers(sprites);
        return this;
    }

    public HexController Build() {      
        HexController controller = gameObject.AddComponent<HexController>().Initialize(tile, view);
        return controller;
    }
}