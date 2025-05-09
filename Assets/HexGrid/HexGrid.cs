using UnityEngine;
using static HexUtil.HexMath;
using System.Collections.Generic;
using System.Linq;

public class HexGrid : MonoBehaviour {
    public GameObject hexTilePrefab = null;
    public Sprite exampleSpriteSize = null;

    [SerializeField] Sprite DefualtBackSprite;

    public int gridWidth = 1; // Number of columns
    public int gridHeight = 1; // Number of rows

    private float hexWidth, hexHeight;
    public Vector2 hexDimensions => new Vector2(hexWidth, hexHeight);


    public Dictionary<Vector3Int , HexController> hexTiles = new Dictionary<Vector3Int , HexController>();

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
            Vector3 pos = CubicToWorld(cubicCoords, hexDimensions);

            HexController hexTile = new HexTileBuilder(cubicCoords, DefualtBackSprite)
                .WithWorldLocation(pos)
                .WithPassability(dataTile.isPassable)
                .WithInteractability(dataTile.isInteractable)
                .WithSprites(dataTile.GetSprites())
                .WithObject(null)
                .WithUnit(null)
                .Build();

            hexTiles.Add(cubicCoords , hexTile);
        }

        instantiated = true;
    }

    void Start() {
        if(instantiated) { EventBus<UpdateHexGridEvent>.Raise(new UpdateHexGridEvent { grid = this }); return; }

        hexWidth = exampleSpriteSize.bounds.size.x;
        hexHeight = exampleSpriteSize.bounds.size.y;

        GenerateGrid();
        EventBus<UpdateHexGridEvent>.Raise(new UpdateHexGridEvent { grid = this });
    }

    // Generate the hex grid
    private void GenerateGrid() {
        for(int col = 0; col < gridWidth; col++) {
            for(int row = 0; row < gridHeight; row++) {
                Vector3Int cubicCoords = OffsetToCubic(new Vector2Int(row, col));
                Vector3 pos = CubicToWorld(cubicCoords, hexDimensions);

                HexController hexTile = new HexTileBuilder(cubicCoords, DefualtBackSprite)
                    .WithWorldLocation(pos)
                    .Build();

                hexTiles.Add(cubicCoords, hexTile);
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
    public void RemoveHexController(Vector2Int offsetCoord) {
        RemoveHexController(OffsetToCubic(offsetCoord));
    }

    public void RemoveHexController(Vector3Int cubicCoord) {
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
            Vector3Int cubicCoords = OffsetToCubic(new Vector2Int(row, col));
            Vector3 pos = CubicToWorld(cubicCoords, hexDimensions);

            HexController hexTile = new HexTileBuilder(cubicCoords, DefualtBackSprite)
                .WithWorldLocation(pos)
                .Build();

            hexTiles.Add(cubicCoords, hexTile);
        }
        gridWidth++;

        EventBus<UpdateHexGridEvent>.Raise(new UpdateHexGridEvent { grid = this });
    }

    // Remove the last column from the grid
    public void RemoveColumn() {
        if(gridWidth <= 1)
            return;

        int col = gridWidth - 1;
        for(int row = 0; row < gridHeight; row++) {
            RemoveHexController(new Vector2Int(row , col));
        }
        gridWidth--;

        EventBus<UpdateHexGridEvent>.Raise(new UpdateHexGridEvent { grid = this });
    }

    // Add a row to the grid
    public void AddRow() {
        int row = gridHeight;
        for(int col = 0; col < gridWidth; col++) {
            Vector3Int cubicCoords = OffsetToCubic(new Vector2Int(row, col));
            Vector3 pos = CubicToWorld(cubicCoords, hexDimensions);

            HexController hexTile = new HexTileBuilder(cubicCoords, DefualtBackSprite)
                .WithWorldLocation(pos)
                .Build();

            hexTiles.Add(cubicCoords, hexTile);
        }
        gridHeight++;

        EventBus<UpdateHexGridEvent>.Raise(new UpdateHexGridEvent { grid = this });
    }

    // Remove the last row from the grid
    public void RemoveRow() {
        if(gridHeight <= 1)
            return;

        int row = gridHeight - 1;
        for(int col = 0; col < gridWidth; col++) {
            RemoveHexController(new Vector2Int(row , col));
        }
        gridHeight--;

        EventBus<UpdateHexGridEvent>.Raise(new UpdateHexGridEvent { grid = this });
    }

    /******************************************************************
        Utility
    ******************************************************************/



    // Get neighbors of a hex tile
    public List<HexController> GetNeighbors(HexController tile) {
        List<HexController> neighbors = new List<HexController>();
        List<Vector3Int> hex_directions = new List<Vector3Int> {
            new Vector3Int(-1, 1, 0), new Vector3Int(0, 1, -1), new Vector3Int(1, 0, -1),
            new Vector3Int(1, -1, 0), new Vector3Int(0, -1, 1), new Vector3Int(-1, 0, 1)
        };

        Vector3Int currentCoords = tile.Coord;
        foreach(var direction in hex_directions) {
            Vector3Int neighborCoords = currentCoords + direction;
            if(hexTiles.ContainsKey(neighborCoords)) {
                neighbors.Add(hexTiles[neighborCoords]);
            }
        }

        return neighbors;
    }

    public List<HexController> GetAllTiles() {
        return new List<HexController>(hexTiles.Values.ToArray());
    }

    public HexController GetTile(Vector3Int targetCoord) {
        foreach(HexController tile in hexTiles.Values) {
            if(tile.Coord == targetCoord) {
                return tile;
            }
        }

        return null;
    }
}
