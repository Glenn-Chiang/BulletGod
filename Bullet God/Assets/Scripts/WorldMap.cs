using UnityEngine;

public static class WorldMap
{
    public static float minX = -200f; // Leftmost edge
    public static float maxX = 200f; // Rightmost edge
    public static float minY = -200f; // Bottommost edge
    public static float maxY = 200f; // Topmost edge

    public static Vector2 topleft = new(minX, maxY);
    public static Vector2 topRight = new(maxX, maxY);
    public static Vector2 bottomLeft = new(minX, minY);
    public static Vector2 bottomRight = new(maxX, minY);

    public static Line topBoundary = new(topleft, topRight);
    public static Line bottomBoundary = new(bottomLeft, bottomRight);
    public static Line leftBoundary = new(topleft, bottomLeft);
    public static Line rightBoundary = new(topRight, bottomRight);

    public static Line[] boundaries = { topBoundary, bottomBoundary, leftBoundary, rightBoundary };

    // Check if a point is within the map
    public static bool WithinMap(Vector2 point)
    {
        int x = (int)point.x;
        int y = (int)point.y;
        return x >= minX && x <= maxX && y >= minY && y <= maxY;
    }
}