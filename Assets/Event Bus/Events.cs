using System.Collections.Generic;

public interface IEvent { }

public struct TestEvent : IEvent { }

public struct SelectUnitEvent : IEvent {
    public UnitModel unit;
}

public struct LoadUnitsOnStartEvent : IEvent {
    public List<UnitModel> units;
}

public struct UpdateHexGridEvent : IEvent {
    public HexGrid grid;
}
public struct TileSelectedEvent : IEvent {
    public HexController tile;
}