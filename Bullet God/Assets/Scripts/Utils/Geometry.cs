using System;
using UnityEngine;

public static class Geometry
{
    public static Vector2 FindIntersection(Vector2 line1Start, Vector2 line1End, Vector2 line2Start, Vector2 line2End)
    {
        float A1 = line1End.y - line1Start.y;
        float B1 = line1Start.x - line1End.x;
        float C1 = A1 * line1Start.x + B1 * line1Start.y;

        float A2 = line2End.y - line2Start.y;
        float B2 = line2Start.x - line2End.x;
        float C2 = A2 * line2Start.x + B2 * line2Start.y;

        float delta = A1 * B2 - A2 * B1;
        if (delta == 0)
        {
            // Lines are parallel, thus there is no point of intersection
            throw new ArgumentException("Lines are parallel");
        }

        float x = (B2 * C1 - B1 * C2) / delta;
        float y = (A1 * C2 - A2 * C1) / delta;

        return new Vector2(x, y);
    }
}

public class Line
{
    public Vector2 start; 
    public Vector2 end;

    public Line(Vector2 start, Vector2 end)
    {
        this.start = start;
        this.end = end;
    }
}