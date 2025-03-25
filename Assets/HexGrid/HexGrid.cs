using UnityEngine;
using System.Collections.Generic;
using UnityEngine.U2D;
using System.Linq;

public class HexGrid : MonoBehaviour {
    public GameObject hexTilePrefab = null;
    public Sprite exampleSpriteSize = null;

    public int gridWidth = 1; // Number of columns
    public int gridHeight = 1; // Number of rows

    private float hexWidth, hexHeight;


    public Dictionary<Vector3Int , HexTile> hexTiles = new Dictionary<Vector3Int , HexTile>();

    bool instantiated = false;



    /******************************************************************
        Create Grid
    ******************************************************************/
    public void Instance(GameObject prefab , Sprite exampleSprite) {
        hexTilePrefab = prefab;
        exampleSpriteSize = exampleSprite;

        hexWidth = exampleSpriteSize.bounds.size.x;
        hexHeight = exampleSpriteSize.bounds.size.y;

        GenerateGrid();

        instantiated = true;
    }

    public void Instance(GameObject prefab, Sprite exampleSprite, LevelBuilderData data) {
        hexTilePrefab = prefab;
        exampleSpriteSize = exampleSprite;
        
        gridWidth = data.gridWidth;
        gridHeight = data.gridHeight;

        hexWidth = exampleSpriteSize.bounds.size.x;
        hexHeight = exampleSpriteSize.bounds.size.y;

        foreach(var dataTile in data.tiles) {
            Vector3Int cubicCoords = dataTile.GetCupicCoord();
            Vector3 pos = CubicToWorld(cubicCoords);

            HexTile hexTile = CreateHexTile(pos , cubicCoords);
            hexTile.isPassable = dataTile.isPassable;
            hexTile.isInteractable = dataTile.isInteractable;

            Sprite[] loadSprites = dataTile.GetSprites();
            for(int i = 0; i < hexTile.spriteLayers.Length; i++) {
                SpriteRenderer renderer = hexTile.spriteLayers[i];
                renderer.sprite = loadSprites[i];
            }

            hexTile.defaultHexSprite = dataTile.GetDefaultSprite();

            hexTiles.Add(cubicCoords , hexTile);
        }

        instantiated = true;
    }

    void Start() {
        if(instantiated) { return; }

        hexWidth = exampleSpriteSize.bounds.size.x;
        hexHeight = exampleSpriteSize.bounds.size.y;

        GenerateGrid();
    }

    // Generate the hex grid
    private void GenerateGrid() {
        for(int col = 0; col < gridWidth; col++) {
            for(int row = 0; row < gridHeight; row++) {
                CreateHexTile(new Vector2Int(row , col));
            }
        }
    }

    private void OnDestroy() {
        foreach(var tile in hexTiles) {
            Destroy(tile.Value.gameObject);
        }
    }


    /******************************************************************
        Creating and Removing Tiles
    ******************************************************************/
    private void CreateHexTile(Vector2Int offsetCoord) {
        Vector3Int cubicCoords = OffsetToCubic(offsetCoord);
        Vector3 pos = OffsetToWorld(offsetCoord);

        HexTile hexTile = CreateHexTile(pos , cubicCoords);
        hexTiles.Add(cubicCoords , hexTile);
    }

    private HexTile CreateHexTile(Vector3 position , Vector3Int cubicCoord) {
        GameObject hexTileObject = Instantiate(hexTilePrefab , position , Quaternion.identity , this.transform);
        HexTile hexTile = hexTileObject.GetComponent<HexTile>();

        hexTile.SetHexTile(cubicCoord);

        return hexTile;
    }

    public void RemoveHexTile(Vector2Int offsetCoord) {
        RemoveHexTile(OffsetToCubic(offsetCoord));
    }

    public void RemoveHexTile(Vector3Int cubicCoord) {
        GameObject hexObj = hexTiles[cubicCoord].gameObject;
        hexTiles.Remove(cubicCoord);
        Destroy(hexObj);
    }


    /******************************************************************
        Adding and Removing Rows/Cols
    ******************************************************************/
    public void AddColumn() {
        int col = gridWidth;
        for(int row = 0; row < gridHeight; row++) {
            CreateHexTile(new Vector2Int(row , col));
        }
        gridWidth++;
    }

    // Remove the last column from the grid
    public void RemoveColumn() {
        if(gridWidth <= 1)
            return;

        int col = gridWidth - 1;
        for(int row = 0; row < gridHeight; row++) {
            RemoveHexTile(new Vector2Int(row , col));
        }
        gridWidth--;
    }

    // Add a row to the grid
    public void AddRow() {
        int row = gridHeight;
        for(int col = 0; col < gridWidth; col++) {
            CreateHexTile(new Vector2Int(row , col));
        }
        gridHeight++;
    }

    // Remove the last row from the grid
    public void RemoveRow() {
        if(gridHeight <= 1)
            return;

        int row = gridHeight - 1;
        for(int col = 0; col < gridWidth; col++) {
            RemoveHexTile(new Vector2Int(row , col));
        }
        gridHeight--;
    }

    /******************************************************************
        Coord Conversions
    ******************************************************************/

    public Vector3 CubicToWorld(Vector3Int cubicCoord) {
        int row = cubicCoord.y;
        int col = cubicCoord.x + (row / 2);

        return OffsetToWorld(new Vector2Int(row , col));
    }

    // Convert hex coordinates to world coordinates
    public Vector3 OffsetToWorld(Vector2Int offsetCoord) {
        int row = offsetCoord.x;
        int col = offsetCoord.y;

        // Calculate x position
        float x = col * hexWidth;

        // Calculate y position and apply stagger for odd columns
        float y = row * hexHeight * 0.75f;
        if(row % 2 != 0) {
            x += hexWidth * 0.5f; // Offset by half the vertical distance between rows
        }
        //Debug.Log(new Vector3(x , y , 0));

        return new Vector3(x , y , 0);
    }

    public Vector3Int OffsetToCubic(Vector2Int offsetCoord) {
        int row = offsetCoord.x;
        int col = offsetCoord.y;

        int q = col - (row / 2);
        int r = row;
        int s = -q - r;

        return new Vector3Int(q , r , s);
    }

    public Vector3Int WorldToCubic(Vector2 worldPoint) {
        //Get Approximate Hexagon using Rounding
        float row = worldPoint.y / (hexHeight * 0.75f);
        float col = worldPoint.x / hexWidth;
        if(Mathf.RoundToInt(row) % 2 != 0) {
            //col = (worldPoint.x + (hexWidth * 0.5f)) / hexWidth;
        }

        float q = col - (row / 2);
        float r = row;
        float s = -q - r;

        Vector3Int targetCoord = RoundCoord(new Vector3(q , r , s)); //Cubic Coord

        //Check target and all neighboors to see who is closer
        List<Vector3Int> hex_directions = new List<Vector3Int> {
            new Vector3Int(-1, 1, 0), new Vector3Int(0, 1, -1), new Vector3Int(1, 0, -1),
            new Vector3Int(1, -1, 0), new Vector3Int(0, -1, 1), new Vector3Int(-1, 0, 1),
        };

        Vector3Int closestCoord = targetCoord;
        float closestDistance = Vector3.Distance(CubicToWorld(targetCoord) , worldPoint);

        foreach(Vector3Int possibleCoord in hex_directions) {
            Vector2 pos = CubicToWorld(possibleCoord);
            float dist = Vector3.Distance(pos , worldPoint);

            if(dist < closestDistance) {
                closestCoord = possibleCoord;
                closestDistance = dist;
            }
        }

        return closestCoord; // Cubic Coord
    }


    /******************************************************************
        Utility
    ******************************************************************/

    public Vector3Int RoundCoord(Vector3 worldCoord) {
        int q = Mathf.RoundToInt(worldCoord.x);
        int r = Mathf.RoundToInt(worldCoord.y);
        int s = Mathf.RoundToInt(worldCoord.z);

        float q_diff = Mathf.Abs(q - worldCoord.x);
        float r_diff = Mathf.Abs(r - worldCoord.y);
        float s_diff = Mathf.Abs(s - worldCoord.z);

        if(q_diff > r_diff && q_diff > s_diff)
            q = -r - s;
        else if(r_diff > s_diff)
            r = -q - s;
        else
            s = -q - r;

        return new Vector3Int(q , r , s);
    }

    // Get neighbors of a hex tile
    public List<HexTile> GetNeighbors(HexTile tile) {
        List<HexTile> neighbors = new List<HexTile>();
        List<Vector3Int> hex_directions = new List<Vector3Int> {
            new Vector3Int(-1, 1, 0), new Vector3Int(0, 1, -1), new Vector3Int(1, 0, -1),
            new Vector3Int(1, -1, 0), new Vector3Int(0, -1, 1), new Vector3Int(-1, 0, 1)
        };

        Vector3Int currentCoords = new Vector3Int(tile.q , tile.r , tile.s);
        foreach(var direction in hex_directions) {
            Vector3Int neighborCoords = currentCoords + direction;
            if(hexTiles.ContainsKey(neighborCoords)) {
                neighbors.Add(hexTiles[neighborCoords]);
            }
        }

        return neighbors;
    }

    public List<HexTile> GetAllTiles() {
        return new List<HexTile>(hexTiles.Values.ToArray());
    }

    /******************************************************************
        Painting Tiles
    ******************************************************************/

    void Update() {
        //if(Input.GetMouseButtonDown(0)) {
        //    Vector3 mousePoint = Input.mousePosition;
        //    mousePoint.z = -Camera.main.transform.position.z;
        //    Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mousePoint);
        //    Vector2 gridPoint = new Vector2(worldPoint.x , worldPoint.y);
        //    Vector3Int hexCoords = WorldToCubic(gridPoint);
        //    if(hexTiles.ContainsKey(hexCoords)) {
        //        RemoveHexTile(hexCoords);
        //    }

        //}

        if(Input.GetKeyDown(KeyCode.Equals)) {
            AddRow();
        }
        if(Input.GetKeyDown(KeyCode.Minus)) {
            RemoveRow();
        }
        if(Input.GetKeyDown(KeyCode.RightBracket)) {
            AddColumn();
        }
        if(Input.GetKeyDown(KeyCode.LeftBracket)) {
            RemoveColumn();
        }
    }


}
