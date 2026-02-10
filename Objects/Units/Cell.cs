using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Cell : Node2D, IEntity, IUnit
{
    [Export] public Array<Component> Components { get; private set; } = [];
    public ComponentQuery E { get; private set; }
    
    public override void _EnterTree()
    {
        E ??= new(this);
    }

    public IUnit GetEastUnit()
    {
        var coord = E.GetComponent<CoordinateComp>().Coordinate;
        return GameManager.World.GetUnit(coord+Vector2I.Right);
    }
    
    public IUnit GetWestUnit()
    {
        var coord = E.GetComponent<CoordinateComp>().Coordinate;
        return GameManager.World.GetUnit(coord+Vector2I.Left);
    }
    
    public IUnit GetNorthUnit()
    {
        var coord = E.GetComponent<CoordinateComp>().Coordinate;
        return GameManager.World.GetUnit(coord+Vector2I.Up);
    }
    
    public IUnit GetSouthUnit()
    {
        var coord = E.GetComponent<CoordinateComp>().Coordinate;
        return GameManager.World.GetUnit(coord+Vector2I.Down);
    }
}
