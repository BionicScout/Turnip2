using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour {
    private void Awake() {
        CreateUnitActions();
    }

    private void CreateUnitActions() {
        new UnitAction.Builder("Move")
            .WithDescription("Move unit from one space to another. Each hexagon cost ######## to move through!")
            .WithType("Move")
            .WithCost(1)
            .WithOnLoad(() => {
                
            })
            .WithPrecondition((UnitModel actionUnit) => {
                return actionUnit.currentMovement != 0;
            })
            .WithSelectionNeeded(true)
            .WithAction((UnitModel actionUnit, HexController tile) => {
                HexGrid grid = FindAnyObjectByType<HexGrid>();
                List<Vector3Int> tileCoordPath = Pathfinding.PathBetweenPoints(grid.GetAllTiles(), actionUnit.HexController, tile, false);
                if(tileCoordPath != null) {
                    Debug.Log("Code to Move Unit");
                }
            })
            .Build();
    }
}
