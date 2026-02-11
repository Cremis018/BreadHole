using Godot;

public class UnitsBuilder(Node root)
{
    public void Build(CoordUnitStorage storage)
    {
        var cells = storage.GetUnits<ICell>();
        var junctions = storage.GetUnits<IJunction>();
        foreach (var cell in cells)
        {
            root.AddChild(cell as Node2D);
        }
        foreach (var junction in junctions)
        {
            root.AddChild(junction as Node2D);
        }
    }
}