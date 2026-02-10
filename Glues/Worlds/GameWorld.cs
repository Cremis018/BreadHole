using Godot;

public class GameWorld
{
    public CoordUnitStorage MapContent { get; private set; }

    public IUnit GetUnit(Vector2I coord) => MapContent.GetUnit(coord);
    
    public T GetUnit<T>(Vector2I coord) where T : Node,IUnit => MapContent.GetUnit(coord) as T;
}