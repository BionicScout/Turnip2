using System;
using UnityEngine;

public class GameEvents : MonoBehaviour {
    public static GameEvents current;

    private void Awake() {
        current = this;
    }

    public event Action<PlayerUnit> OnPlayerSelect;
    public void PlayerSelect(PlayerUnit playerUnit) {
        if (OnPlayerSelect != null) {
            OnPlayerSelect(playerUnit);
        }
    }
}
