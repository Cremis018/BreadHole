using Godot;

public class CoordModi
{
    private Node2D _node;
    
    public CoordModi(IEntity entity)
    {
        _node = entity as Node2D;
        var comp = entity.E.GetComponent<CoordinateComp>();
        comp.CoordinateChanged += OnCoordinateChanged;
        OnCoordinateChanged(comp.Coordinate);
    }

    private void OnCoordinateChanged(Vector2I coord)
    {
        _node.Position = coord * Constants.BASE_LENGTH;
    }
}