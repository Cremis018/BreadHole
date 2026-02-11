using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Cell : Node2D, ICell
{
    [Export] public Array<Component> Components { get; private set; } = [];
    public ComponentQuery E { get; private set; }
    
    private CoordModi _coordModi;
    private TextureModi _textureModi;
    
    [Export] private Sprite2D _sprite;
    
    public override void _EnterTree()
    {
        E ??= new(this);
        _coordModi = new(this);
        _textureModi = new(this,_sprite);
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
