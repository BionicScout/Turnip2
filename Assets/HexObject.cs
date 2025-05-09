using UnityEngine;

public class HexObject {
    string name = "Un-named";
    public HexController currentTile;

    public Vector3Int HexCoord => currentTile.Coord;
}
