using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_U_XXX", menuName = "Combat Unit/Unit")]
public class SO_UnitModel : ScriptableObject {
    public string UnitName;
    public Sprite Sprite;
    public List<UnitActionStrategySO> actions;
}