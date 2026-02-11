using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Player : Node2D, IEntity
{
    [Export] public Array<Component> Components { get; private set; }
    public ComponentQuery E { get; private set; }

    public Vector2I FacingCoord => GetFacingCoord();
    public Vector2I BackingCoord => GetBackingCoord();
    
    private DirectionModi _directionModi;

    public override void _EnterTree()
    {
        E ??= new(this);
        _directionModi = new(this);
    }

    private Vector2I GetFacingCoord()
    {
        var coord = E.GetComponent<CoordinateComp>().Coordinate;
        var dir = E.GetComponent<DirectionComp>().Direction;
        return CoordUtil.CalcFacingCoord(coord, dir);
    }

    private Vector2I GetBackingCoord()
    {
        var coord = E.GetComponent<CoordinateComp>().Coordinate;
        var dir = E.GetComponent<DirectionComp>().Direction;
        return CoordUtil.CalcBackingCoord(coord, dir);
    }
}
