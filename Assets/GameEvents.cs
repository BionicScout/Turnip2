using System;
using UnityEngine;

public class GameEvents : MonoBehaviour {
    public static GameEvents current;

    private void Awake() {
        current = this;
    }


    /**********************************
        HUD
    **********************************/ 

    public event Action<UnitModel, Action<int>> OnPlayerSelect;
    public void PlayerSelect(UnitModel playerUnit, Action<int> listener) {
        if (OnPlayerSelect != null) {
            OnPlayerSelect(playerUnit, listener);
        }
    }


    /**********************************
        Camera
    **********************************/
    public event Action<HexGrid> OnSetCameraBounds;
    public void SetCameraBounds(HexGrid grid) {
        if(OnSetCameraBounds != null) {
            OnSetCameraBounds(grid);
        }
    }

    /**********************************
        Map Inputs
    **********************************/

}
