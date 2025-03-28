using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour {
    PlayerUnit playerUnit;

    [Header("Player Selection")]
    public Image SelectedPlayerImage;
    public GameObject SelectedPlayerTracker;

    private void Start() {
        GameEvents.current.OnPlayerSelect += SetSelectedPlayer;
    }

    public void SetSelectedPlayer(PlayerUnit pu) {
        playerUnit = pu;

        //Select Player UI
        SelectedPlayerImage.sprite = pu.unitSprite;
        SetPlayerState(pu.state);
    }

    public void SetPlayerState(CombatUnit.State state) {
        if(state == CombatUnit.State.move) {
            SelectedPlayerTracker.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            SelectedPlayerTracker.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
            SelectedPlayerTracker.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
        }
        else if(state == CombatUnit.State.item) {
            SelectedPlayerTracker.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            SelectedPlayerTracker.transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
            SelectedPlayerTracker.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
        }
        else if(state == CombatUnit.State.action) {
            SelectedPlayerTracker.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            SelectedPlayerTracker.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            SelectedPlayerTracker.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
        }
        else {
            SelectedPlayerTracker.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            SelectedPlayerTracker.transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
            SelectedPlayerTracker.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
        }
    }
}
