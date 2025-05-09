using System;
using System.Collections.Generic;

public class UnitAction {
    public static List<UnitAction> allActions { private set; get; } = new List<UnitAction>();

    string name;
    string description;
    string type;
    int cost;
    bool needsSelectionTarget;

    Action LoadAction;
    Func<UnitModel, bool> CheckPreconditions = (UnitModel u) => true;
    Action<UnitModel, HexController> PerformAction;


    public class Builder {
        UnitAction action;

        public Builder(string name) {
            UnitAction temp = allActions.Find(a => a.name == name);
            if(temp != null){
                throw new System.Exception("The UnitAction \"" + name + "\" has already been defined!");
            }

            action = new UnitAction();
            action.name = name;
        }

        public Builder WithDescription(string description) {
            action.description = description;
            return this;
        }

        public Builder WithType(string type) {
            action.type = type;
            return this;
        }

        public Builder WithCost(int cost) {
            action.cost = cost;
            return this;
        }

        public Builder WithSelectionNeeded(bool selectionNeeded) {
            action.needsSelectionTarget = selectionNeeded;
            return this;
        }

        public Builder WithOnLoad(Action onLoad) {
            action.LoadAction = onLoad;
            return this;
        }

        public Builder WithAction(Action<UnitModel, HexController> performAction) {
            action.PerformAction = performAction;
            return this;
        }

        public Builder WithPrecondition(Func<UnitModel, bool> func) {
            action.CheckPreconditions = func;
            return this;
        }

        public UnitAction Build() {
            allActions.Add(action);
            return action;
        }
    }
}