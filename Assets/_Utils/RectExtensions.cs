using UnityEngine;

public static class RectExtensions
{
    // Scales the size of the Rect by the provided scale factor while keeping the position anchored to the provided pivot point.
    public static Rect ScaleSizeBy(this Rect rect, float scale, Vector2 pivotPoint)
    {
        Rect result = rect;
        result.x -= pivotPoint.x; // Move pivot to origin
        result.y -= pivotPoint.y;
        result.xMin *= scale; // Scale the rect
        result.xMax *= scale;
        result.yMin *= scale;
        result.yMax *= scale;
        result.x += pivotPoint.x; // Move pivot back to original position
        result.y += pivotPoint.y;
        return result;
    }

    // Returns the top-left corner of the Rect, which is just its position.
    public static Vector2 TopLeft(this Rect rect)
    {
        return new Vector2(rect.xMin, rect.yMin);
    }
}