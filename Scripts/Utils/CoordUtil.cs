using Godot;

internal class CoordUtil
{
    public static Vector2I CalcFacingCoord(Vector2I origin, Direction direction) => 
        origin + DirectionToVector2I(direction);

    public static Vector2I CalcBackingCoord(Vector2I origin, Direction direction) => 
        origin - DirectionToVector2I(direction);

    public static Vector2I DirectionToVector2I(Direction direction) =>
        direction switch
        {
            Direction.East => Vector2I.Right,
            Direction.South => Vector2I.Down,
            Direction.West => Vector2I.Left,
            Direction.North => Vector2I.Up,
            _ => Vector2I.Zero
        };
    
    public static Direction Vector2IToDirection(Vector2I vector2I)
    {
        if (vector2I == Vector2I.Right)
            return Direction.East;
        if (vector2I == Vector2I.Down)
            return Direction.South;
        if (vector2I == Vector2I.Left)
            return Direction.West;
        if (vector2I == Vector2I.Up)
            return Direction.North;
        return Direction.None;
    }

    public static float DirectionToRotation(Direction direction) =>
        (int)direction * float.Pi / 2;

    public static bool IsCellCoord(Vector2I coord) => coord.X % 2 == 1 && coord.Y % 2 == 1;

    public static bool IsJunctionCoord(Vector2I coord,out bool vertical)
    {
        if (coord.X % 2 == 1 && coord.Y % 2 == 0)
        {
            vertical = true;
            return true;
        }
        if (coord.X % 2 == 0 && coord.Y % 2 == 1)
        {
            vertical = false;
            return true;
        }
        vertical = false;
        return false;
    }
}