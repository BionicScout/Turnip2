using System.Collections.Generic;
using UnityEngine;

namespace HexUtil {
    public static class HexMath {
        public static List<Vector3Int> hex_directions = new List<Vector3Int> {
            new Vector3Int(-1, 1, 0), new Vector3Int(0, 1, -1), new Vector3Int(1, 0, -1),
            new Vector3Int(1, -1, 0), new Vector3Int(0, -1, 1), new Vector3Int(-1, 0, 1),
        };

        /******************************************************************
            Coord Conversions
        ******************************************************************/

        public static Vector3 CubicToWorld(Vector3Int cubicCoord, Vector2 hexDimensions) {
            int row = cubicCoord.y;
            int col = cubicCoord.x + (row / 2);

            return OffsetToWorld(new Vector2Int(row, col), hexDimensions);
        }

        // Convert hex coordinates to world coordinates
        public static Vector3 OffsetToWorld(Vector2Int offsetCoord, Vector2 hexDimensions) {
            int row = offsetCoord.x;
            int col = offsetCoord.y;

            // Calculate x position
            float x = col * hexDimensions.x; //hexWidth

            // Calculate y position and apply stagger for odd columns
            float y = row * hexDimensions.y * 0.75f; //hexHeight
            if(row % 2 != 0) {
                x += hexDimensions.x * 0.5f; // Offset by half the vertical distance between rows
            }

            return new Vector3(x, y, 0);
        }

        public static Vector3Int OffsetToCubic(Vector2Int offsetCoord) {
            int row = offsetCoord.x;
            int col = offsetCoord.y;

            int q = col - (row / 2);
            int r = row;
            int s = -q - r;

            return new Vector3Int(q, r, s);
        }

        public static Vector3Int WorldToCubic(Vector2 worldPoint, Vector2 hexDimensions) {
            //Get Approximate Hexagon using Rounding
            float row = worldPoint.y / (hexDimensions.y * 0.75f);
            float col = worldPoint.x / hexDimensions.x;
            if(Mathf.RoundToInt(row) % 2 != 0) {
                //col = (worldPoint.x + (hexWidth * 0.5f)) / hexWidth;
            }

            float q = col - (row / 2);
            float r = row;
            float s = -q - r;

            Vector3Int targetCoord = RoundCoord(new Vector3(q, r, s)); //Cubic Coord

            //Check target and all neighboors to see who is closer
            Vector3Int closestCoord = targetCoord;
            float closestDistance = Vector3.Distance(CubicToWorld(targetCoord, hexDimensions), worldPoint);

            foreach(Vector3Int possibleCoord in hex_directions) {
                Vector2 pos = CubicToWorld(possibleCoord, hexDimensions);
                float dist = Vector3.Distance(pos, worldPoint);

                if(dist < closestDistance) {
                    closestCoord = possibleCoord;
                    closestDistance = dist;
                }
            }

            return closestCoord; // Cubic Coord
        }

        public static Vector3Int RoundCoord(Vector3 worldCoord) {
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

            return new Vector3Int(q, r, s);
        }
    }
}
