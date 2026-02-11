using Godot;

public class JunctionVHModi
{
    private Node2D _node;
    
    public JunctionVHModi(IEntity entity)
    {
        _node = entity as Node2D;
        var comp = entity.E.GetComponent<CoordinateComp>();
        comp.CoordinateChanged += OnCoordinateChanged;
        OnCoordinateChanged(comp.Coordinate);
    }

    private void OnCoordinateChanged(Vector2I coord)
    {
        _node.Rotation = (coord.X % 2) switch
        {
            1 when coord.Y % 2 == 0 => Mathf.Pi / 2,
            0 when coord.Y % 2 == 1 => 0,
            _ => _node.Rotation
        };
    }
}