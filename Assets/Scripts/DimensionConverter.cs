using UnityEngine;

public static class DimensionConverter
{
    public static Vector3 XZtoXYZ(Vector2 input, float y = 0.0f)
    {
        return new Vector3(input.x, y, input.y);
    }

    public static Vector3 XYZtoXZ(Vector3 input)
    {
        return new Vector2(input.x, input.z);
    }
}
