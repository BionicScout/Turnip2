using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Pathfinding {
    private const int UNVISITED_DISTANCE = -1;
    private readonly Vector3Int INVALID_COORD = Vector3Int.one;
    private const int MAX_ITERATIONS = int.MaxValue;


    /*********************************
       Classes
    *********************************/

    struct SearchHex {
        public Vector3Int coord;
        public bool visited;
        public bool isObstacle;
        public int dist;
        public Vector3Int previous;

        // Constructor
        public SearchHex(HexTile tile, bool ignoreUnits) {
            Debug.Assert(tile != null, "Pathfinding.SearchHex[SearchHex]: tile is null");

            coord = tile.GetCoords();
            visited = false;
            isObstacle = !tile.isPassable /*|| (ignoreUnits && (GlobalVars.players.ContainsKey(hexCoord) || GlobalVars.enemies.ContainsKey(hexCoord)))*/;
            dist = UNVISITED_DISTANCE;
            previous = Vector3Int.one; //Impossible to Get with Hexagon Coords
        }

        public SearchHex(HexTile tile, bool ignoreUnits, Vector3Int startLoc) {
            Debug.Assert(tile != null, "Pathfinding.SearchHex[SearchHex]: tile is null");

            coord = tile.GetCoords();
            visited = false;
            isObstacle = !tile.isPassable /* || (ignoreUnits && startLoc != hexCoord && (GlobalVars.players.ContainsKey(hexCoord) || GlobalVars.enemies.ContainsKey(hexCoord)))*/;
            dist = UNVISITED_DISTANCE;
            previous = Vector3Int.one; //Impossible to Get with Hexagon Coords
        }
    }

    //class PriorityQueue<Vector3Int> {
    //    private List<Tuple<Vector3Int, float>> elements = new List<Tuple<Vector3Int, float>>();

    //    public int Count {
    //        get { return elements.Count; }
    //    }

    //    public void Enqueue(Vector3Int item, float priority) {
    //        elements.Add(Tuple.Create(item, priority));
    //    }

    //    public Vector3Int Dequeue() {
    //        int bestIndex = 0;
    //        for(int i = 0; i < elements.Count; i++) {
    //            if(elements[i].Item2 < elements[bestIndex].Item2) {
    //                bestIndex = i;
    //            }
    //        }

    //        Vector3Int bestItem = elements[bestIndex].Item1;
    //        elements.RemoveAt(bestIndex);
    //        return bestItem;
    //    }

    //    public bool Contains(Vector3Int item) {
    //        foreach(var element in elements) {
    //            if(EqualityComparer<Vector3Int>.Default.Equals(element.Item1, item)) {
    //                return true;
    //            }
    //        }
    //        return false;
    //    }
    //}



    /*********************************
       Pathfinding
    *********************************/
    public static List<Tuple<HexTile, int>> AllTilesInRangeWithDistance(List<HexTile> allTiles, HexTile startTile, int range, bool ignoreUnits) {
        Debug.Assert(startTile != null, "Pathfinding[AllTilesInRangeWithDistance]: startTile is null");
        Debug.Assert(range > 0, "Pathfinding[AllTilesInRangeWithDistance]: range less than 1");
        Debug.Assert(allTiles != null, "Pathfinding[AllTilesInRangeWithDistance]: allTiles is null");

        // Define variables for loop
        HexTile currentTile;
        HexTile nextTile;
        SearchHex nextSearchHex;

        Dictionary<Vector3Int, HexTile> tileLookUp = allTiles.ToDictionary(tile => tile.GetCoords());

        // Create dictionary with start hex
        Dictionary<HexTile, SearchHex> searchList = new Dictionary<HexTile, SearchHex> { { startTile, new SearchHex(startTile, ignoreUnits) } };

        // Define queue with tiles to search. Starts with the starting location. Then Search list.
        Queue<HexTile> queue = new Queue<HexTile>();
        queue.Enqueue(startTile);

        Visit(searchList, startTile);

        for(int iterations = 0; queue.Count > 0; iterations++) {// Check for potential infinite loop
            if(iterations > MAX_ITERATIONS) {
                throw new Exception("Pathfinding Error: Maximum iterations reached. Potential infinite loop detected");
            }

            currentTile = queue.Dequeue();

            foreach(Vector3Int dir in HexTile.directions) {
                // If position is off board, skip.
                tileLookUp.TryGetValue(currentTile.GetCoords() + dir, out nextTile);
                if(nextTile == null) { continue; }

                // If position is on board, add Search hex if needed
                if(!searchList.TryGetValue(nextTile, out nextSearchHex)) {
                    nextSearchHex = new SearchHex(nextTile, ignoreUnits);
                    searchList.Add(nextTile, nextSearchHex);
                }


                // Avoid Obstacle
                if(nextSearchHex.isObstacle) {
                    Visit(searchList, nextTile);
                    continue;
                }

                // Already Visited
                if(nextSearchHex.visited) {
                    continue;
                }

                // Visit
                queue.Enqueue(nextTile);
                Visit(searchList, nextTile, searchList[currentTile].dist);
            }
        }

        // Return List of Possibilities
        List<Tuple<HexTile, int>> possibleTiles = new List<Tuple<HexTile, int>>();

        foreach(KeyValuePair<HexTile, SearchHex> data in searchList) {
            if(data.Value.dist < range && !data.Value.isObstacle) {
                possibleTiles.Add(new Tuple<HexTile, int>(data.Key, (int)data.Value.dist)); //Only int for dist in this algorithm
            }
        }


        return possibleTiles;
    }

    public static List<HexTile> AllTilesInRange(List<HexTile> allTiles, HexTile startTile, int range, bool ignoreUnits) {
        var hexList = AllTilesInRangeWithDistance(allTiles, startTile, range, ignoreUnits);
        List<HexTile> tiles = new List<HexTile>();

        foreach(var data in hexList) {
            tiles.Add(data.Item1);
        }

        return tiles;
    }

    public static List<Vector3Int> PathBetweenPoints(List<HexTile> allTiles, HexTile startTile, HexTile endTile, bool ignoreUnits) {
        // A* (star) Pathfinding

        Debug.Assert(startTile != null, "Pathfinding[PathBetweenPoints]: startTile is null");
        Debug.Assert(endTile != null, "Pathfinding[PathBetweenPoints]: endTile is null");
        Debug.Assert(allTiles != null, "Pathfinding[PathBetweenPoints]: allTiles is null");

        //Loop Variables 
        HexTile nextTile;
        SearchHex nextSearchHex;
        Dictionary<Vector3Int, HexTile> tileLookUp = allTiles.ToDictionary(tile => tile.GetCoords());

        //Initilzize frontier with Priority Queue
        MinPriorityQueue<Vector3Int> frontier = new MinPriorityQueue<Vector3Int>();
        frontier.Enqueue(startTile.GetCoords(), 0);

        //Define dictionary of tile Activley in search
        Dictionary<Vector3Int, SearchHex> searchList = new Dictionary<Vector3Int, SearchHex>();
        SearchHex temp = new SearchHex(startTile, ignoreUnits, startTile.GetCoords());
        temp.dist = 0;
        searchList.Add(startTile.GetCoords(), temp);

        //
        int iterations = 0;
        while(frontier.Count() != 0) {
            Vector3Int currentTilePos = frontier.Dequeue();

            //If target found exit
            if(currentTilePos == endTile.GetCoords()) {
                //Debug.Log(searchList[endPoint].previous);
                break;
            }

            //
            foreach(Vector3Int dir in HexTile.directions) {
                // If position is off board, skip.
                tileLookUp.TryGetValue(currentTilePos + dir, out nextTile);
                if(nextTile == null) { continue; }

                // If position is on board, add Search hex if needed
                if(!searchList.TryGetValue(nextTile.GetCoords(), out nextSearchHex)) {
                    nextSearchHex = new SearchHex(nextTile, ignoreUnits);
                    searchList.Add(nextTile.GetCoords(), nextSearchHex);
                }

                // Avoid Obstacle
                if(nextSearchHex.isObstacle) {
                    continue;
                }

                //
                SearchHex currentSearchHex = searchList[currentTilePos];
                int newDist = (currentSearchHex.dist + 1);

                if(nextSearchHex.dist == UNVISITED_DISTANCE || nextSearchHex.dist > newDist) {
                    nextSearchHex.dist = newDist;
                    float priority = newDist + ManhattanDistance(currentTilePos, endTile.GetCoords());
                    frontier.Enqueue(nextTile.GetCoords(), priority);
                    nextSearchHex.previous = currentTilePos;

                    searchList[nextTile.GetCoords()] = nextSearchHex;
                }
            }

            //
            iterations++;

            if(iterations >= MAX_ITERATIONS) {
                Debug.LogError("ERROR - Maximum iterations reached. Potential infinite loop detected.");
                return null;
            }
        }

        //Doesn't Reach Target
        if(!searchList.ContainsKey(endTile.GetCoords())) {
            //Debug.Log("Pathfinding - end point not reached");
            return null;
        }



        //Trace Path back
        List<Vector3Int> path = new List<Vector3Int>();
        Vector3Int currentPath = endTile.GetCoords();

        // Maximum iteration limit for path reconstruction
        int pathIterations = 0;
        //Debug.Log("---------------------------------------------------------------------------------------------------");
        //Debug.Log(currentPath + "\t" + searchList.ContainsKey(currentPath));

        while(searchList[currentPath].previous != Vector3.one) {
            // Increment path reconstruction iteration count
            pathIterations++;

            // Check maximum iteration limit
            if(pathIterations >= MAX_ITERATIONS) {
                Debug.LogError("ERROR - Maximum path reconstruction iterations reached. Potential infinite loop detected.");
                return null;
            }

            // Add current path to the list and Move to the previous tile
            path.Add(currentPath);
            currentPath = searchList[currentPath].previous;
            //Debug.Log(currentPath);
        }

        // Add the last path
        // Add the last path
        path.Add(currentPath);
        path.Reverse();

        return path;

    }

    /*********************************
        Smaller methods
    *********************************/

    private static void Visit(Dictionary<HexTile, SearchHex> searchList, HexTile key) {
        SearchHex temp = searchList[key];

        temp.visited = true;

        searchList[key] = temp;
    }

    private static void Visit(Dictionary<HexTile, SearchHex> searchList, HexTile key, int currentRange) {
        SearchHex temp = searchList[key];

        temp.visited = true;
        temp.dist = currentRange + 1;

        searchList[key] = temp;
    }

    // Heuristic function using Manhattan distance
    private static float ManhattanDistance(Vector3Int from, Vector3Int to) {
        int dx = Mathf.Abs(from.x - to.x);
        int dy = Mathf.Abs(from.y - to.y);
        int dz = Mathf.Abs(from.z - to.z);
        return dx + dy + dz;
    }
}
