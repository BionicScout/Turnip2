using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitView : MonoBehaviour {
    UnitModel unit;

    public Image SelectedPlayerImage;
    public GameObject TemplateActionButton;
    [SerializeField] Sprite SelectPlayerSprite;

    List<GameObject> actionButtons = new List<GameObject>();

    public void SetSelectedPlayer(UnitModel pu, Action<int> buttonListener) {
        //Unselect Last Player
        unit.HexController.SetSpriteLayer(4, null);

        //Set Selected Image
        unit = pu;
        SelectedPlayerImage.sprite = pu.UnitSprite;
        unit.HexController.SetSpriteLayer(4, SelectPlayerSprite);
        UpdateActions(buttonListener);
    }

    public void UpdateActions(Action<int> buttonListener) {
        //Remove Old Actions
        foreach(GameObject button in actionButtons) { Destroy(button); }
        actionButtons.Clear();

        //Add New Actions
        for(int i = 0; i < unit.actions.Count; i++) {
            IUnitActionStrategy action = unit.actions[i];
            GameObject newActionButton = Instantiate(TemplateActionButton, TemplateActionButton.transform.parent);
            newActionButton.GetComponent<ActionButton>().Initialize(i, action.GetName());
            newActionButton.GetComponent<ActionButton>().RegisterListener(buttonListener);
            newActionButton.SetActive(true);
        }

        //Move Spacer
        Transform spacerTransform = TemplateActionButton.transform.parent.GetChild(2);
        spacerTransform.parent = null;
        spacerTransform.parent = TemplateActionButton.transform.parent;
    }
}
