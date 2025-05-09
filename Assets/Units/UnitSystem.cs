using System.Collections.Generic;
using UnityEngine;

public class UnitSystem : MonoBehaviour {
    [SerializeField] UnitView view;
    List<UnitModel> units;
    UnitController controller;

    EventBinding<SelectUnitEvent> selectUnitEventBinding;
    EventBinding<LoadUnitsOnStartEvent> LoadUnitsOnStartEventBinding;
    void OnEnable() {
        selectUnitEventBinding = new EventBinding<SelectUnitEvent>(SelectUnitEvent);
        EventBus<SelectUnitEvent>.Register(selectUnitEventBinding);

        LoadUnitsOnStartEventBinding = new EventBinding<LoadUnitsOnStartEvent>(LoadUnitsEvent);
        EventBus<LoadUnitsOnStartEvent>.Register(LoadUnitsOnStartEventBinding);
    }

    void OnDisable() {
        EventBus<SelectUnitEvent>.Deregister(selectUnitEventBinding);
        EventBus<LoadUnitsOnStartEvent>.Deregister(LoadUnitsOnStartEventBinding);
    }

    void SelectUnitEvent(SelectUnitEvent unitEvent) {
        Debug.Log($"Unit event received! Name: {unitEvent.unit.name}");
    }
 
    void LoadUnitsEvent(LoadUnitsOnStartEvent unitEvent) {
        foreach(UnitModel u in unitEvent.units) {
            Debug.Log($"Unit event received! Name: {u.name}");
        }
        units = unitEvent.units;
    }
}