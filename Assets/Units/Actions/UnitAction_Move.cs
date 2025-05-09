using UnityEngine;

[CreateAssetMenu(fileName = "SO_UA_XXX", menuName = "Combat Unit/Acton/Move")]
public class UnitAction_Move : UnitActionStrategySO {
    public override bool Execute(UnitModel user, object input) {
        Vector3Int inputSpace = (Vector3Int)input;
        if(inputSpace != null) {
            //Get Unit Current Movement
            //Return false if inputSpace not in range
            //Return fasle if tile contains an object (not moveable)
            //Move to new Space
            //Return True
        }

        return false;
    }
}
