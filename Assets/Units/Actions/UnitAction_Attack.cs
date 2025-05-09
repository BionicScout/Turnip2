using UnityEngine;

[CreateAssetMenu(fileName = "SO_UA_XXX", menuName = "Combat Unit/Acton/Attack")]
public class UnitAction_Attack : UnitActionStrategySO {
    public int damage;

    public override bool Execute(UnitModel user, object input) {
        if(input is Vector3Int) {
            Vector3Int inputSpace = (Vector3Int)input;
            //Get Unit Current Movement
            //Return fasle if inputSpace not in range
            //Return false if tile contains an object that can't be attacked
            //Attack unit/object
            //return true
        }

        return false;
    }
}
