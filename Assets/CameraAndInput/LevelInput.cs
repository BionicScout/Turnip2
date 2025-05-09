using static HexUtil.HexMath;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelInput : MonoBehaviour {
    HexGrid hexGrid;
    HexController selectedTile;

    bool hoveringOnUI = false;

    private void Start() {
        hexGrid = FindAnyObjectByType<HexGrid>();
    }

    // Update is called once per frame
    void Update() {
        if(hexGrid == null) {
            hexGrid = FindAnyObjectByType<HexGrid>();
            return;
        }

        if(Input.GetMouseButtonDown(0)) {
            Debug.Log("Click");

            hoveringOnUI = IsPointerOverUIElement();
            if(hoveringOnUI) { return; }

            selectedTile = GetTileFromMousePos();
            EventBus<TileSelectedEvent>.Raise(new TileSelectedEvent { tile = selectedTile });
        }
    }

    HexController GetTileFromMousePos() {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = -Camera.main.transform.position.z;
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mousePoint);
        Vector2 gridPoint = new Vector2(worldPoint.x, worldPoint.y);

        Vector3Int hexCoords = WorldToCubic(gridPoint, hexGrid.hexDimensions);
        return hexGrid.GetTile(hexCoords);
    }

    public static bool IsPointerOverUIElement() {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);

        for(int index = 0; index < raysastResults.Count; index++) {
            RaycastResult curRaysastResult = raysastResults[index];

            if(curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
                return true;
        }

        return false;
    }


}
