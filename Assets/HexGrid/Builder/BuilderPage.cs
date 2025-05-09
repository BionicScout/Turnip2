using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuilderPage : TabGroup {
    public enum Layer { hexLayer = 0, backgroundLayer = 1, objectLayer = 2, foregroundLayer = 3 }
    public Layer layer;

    public GameObject hexGridObject;

    public List<Sprite> sprites = new List<Sprite>();

    private void Start() {
        tabButtons.Clear();

        for(int i = 0; i < transform.childCount; i++) {
            Subscribe(transform.GetChild(i).GetComponent<BuilderItem>());
        }

        for(int i = 0; i < Mathf.Min(tabButtons.Count, sprites.Count); i++) {
            ((BuilderItem)tabButtons[i]).UpdateSprite(sprites[i]);
        }

    }

    void OnEnable() {
        TileSelectedEventBinding = new EventBinding<TileSelectedEvent>(SelectTileEvent);
        EventBus<TileSelectedEvent>.Register(TileSelectedEventBinding);
    }

    void OnDisable() {
        EventBus<TileSelectedEvent>.Deregister(TileSelectedEventBinding);
    }

    public void LeftClick(HexController selectedTile) {
        if(selectedTile == null) { return; }
        if(EventSystem.current.IsPointerOverGameObject()) { return; }
        if(selectedTab == null) { return; }

        HexGrid hexGrid = hexGridObject.GetComponent<HexGrid>();
        if(hexGrid == null) {
            return;
        }

        selectedTile.SetSpriteLayer((int)layer, ((BuilderItem)selectedTab).GetSprite());
    }

    public void RightClick(HexController selectedTile) {
        if(selectedTile == null) { return; }
        if(EventSystem.current.IsPointerOverGameObject()) { return; }
        if(selectedTab == null) { return; }

        HexGrid hexGrid = hexGridObject.GetComponent<HexGrid>();
        if(hexGrid == null) {
            return;
        }

        selectedTile.SetSpriteLayer((int)layer, ((BuilderItem)selectedTab).GetSprite());
    }

    /******************************************************************
        Events and Event Bus
    ******************************************************************/
    EventBinding<TileSelectedEvent> TileSelectedEventBinding;

    void SelectTileEvent(TileSelectedEvent @event) {
        if(@event.tile == null) { return; }

        LeftClick(@event.tile);
    }


}