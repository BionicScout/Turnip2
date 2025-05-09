using UnityEngine;

public class HexModel {
    public int q, r, s;
    public bool isPassable = true;
    public bool isInteractable = false;
    public HexObject hexObject = null;
    public UnitModel unit = null;

    public void SetCoord(int q, int r, int s) {
        Debug.Assert(q + r + s == 0, "Cube coordinates do not sum to 0");

        this.q = q;
        this.r = r;
        this.s = s;
    }

    public HexModel Copy() {
        return new HexModel { 
            q = q, r = r, s = s,
            isPassable = isPassable, 
            isInteractable = isInteractable,
            hexObject = hexObject, // Consider hexObject?.Copy() if it has a Copy method
            unit = unit // Consider unit?.Copy() if it has a Copy method
        };
    }

    public override string ToString() {
        return $"({q}, {r}, {s}) - {{passable: {isPassable}, interactable: {isInteractable}, unit: {unit?.name ?? "none"}}}";
    }
}
