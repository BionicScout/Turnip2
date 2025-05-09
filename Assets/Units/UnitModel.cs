using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitModel {
    public enum State { move = 0, item = 1, action = 2, done = 3 };

    public string name { get; private set; }
    public Sprite UnitSprite { get; private set; }
    HexController currentTile;

    public Vector3Int HexCoord => currentTile.Coord;
    public HexController HexController => currentTile;

    public State state;

    //Stats
    public int startMovement;
    public int currentMovement;

    //UnitStats stats = null;
    //UnitClass unitClass = null;
    //UnitEquipment equipment = null;
    //List<UnitItem> items = new List<UnitItem>();

    public List<IUnitActionStrategy> actions = new List<IUnitActionStrategy>();

    UnitModel(SO_UnitModel unit, HexController tile) { 
        name = unit.UnitName;
        UnitSprite = unit.Sprite;

        currentTile = tile;
    }

    public static UnitModel CreateUnit(SO_UnitModel unitInfo, HexController tile) {
        UnitModel unit = new UnitModel(unitInfo, tile);
        unit.LoadActions(unitInfo);
        return unit;
    }

    public void LoadActions(SO_UnitModel unitInfo) {
        foreach(var action in unitInfo.actions) {
            actions.Add(action);
        }
    }
}


//public class UnitClass {
//    public List<IUnitActionStrategy> GetActions() { return null; }
//}

//public class UnitEquipment {
//    public List<IUnitActionStrategy> GetActions() { return null; }
//}

//public class UnitItem {
//    public List<IUnitActionStrategy> GetActions() { return null; }
//}