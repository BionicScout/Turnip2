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

    public event Action<PlayerUnit> OnPlayerSelect;
    public void PlayerSelect(PlayerUnit playerUnit) {
        if (OnPlayerSelect != null) {
            OnPlayerSelect(playerUnit);
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
}
