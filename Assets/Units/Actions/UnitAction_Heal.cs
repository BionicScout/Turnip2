using UnityEngine;

[CreateAssetMenu(fileName = "SO_UA_XXX", menuName = "Combat Unit/Acton/Heal")]
public class UnitAction_Heal : UnitActionStrategySO {
    public int healAmount;

    public override bool Execute(UnitModel user, object input) {
        //No inputSpace needed
        //Heal unit by health amount
        return true;
    }
}