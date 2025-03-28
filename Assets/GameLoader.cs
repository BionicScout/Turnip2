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
            .WithPrecondition((CombatUnit actionUnit) => {
                return actionUnit.currentMovement != 0;
            })
            .WithSelectionNeeded(true)
            .WithAction((CombatUnit actionUnit, HexTile tile) => {
                HexGrid grid = FindAnyObjectByType<HexGrid>();
                List<Vector3Int> tileCoordPath = Pathfinding.PathBetweenPoints(grid.GetAllTiles(), actionUnit.currentTile, tile, false);
                if(tileCoordPath != null) {
                    Debug.Log("Code to Move Unit");
                }
            })
            .Build();
    }
}
