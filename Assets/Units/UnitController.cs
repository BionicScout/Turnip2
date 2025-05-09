using System.Collections.Generic;
using UnityEngine;

public class UnitController {
    UnitModel model;
    UnitView view;

    public UnitController(UnitView view, UnitModel model) {
        this.view = view;
        this.model = model;

        ConnectModel();
        ConnectView();
    }

    void ConnectModel() {

    }

    void ConnectView() {
        view.SetSelectedPlayer(model, OnActionButtonPressed);
    }

    void OnActionButtonPressed(int index) {
        Debug.Log(model.actions[index].GetName());
    }
}
