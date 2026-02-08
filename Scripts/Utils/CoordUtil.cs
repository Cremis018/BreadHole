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

    public static float DirectionToRotation(Direction direction) =>
        (int)direction * float.Pi / 2;
}