using System.Collections.Generic;
using Godot;

public class GameMapWorld
{
    #region life
    public GameMapWorld()
    {
        
    }

    public GameMapWorld(Node2D mapRoot)
    {
        MapRoot = mapRoot;
    }
    #endregion
    
    #region props
    public Node2D MapRoot { get; set; }
    public Dictionary<Vector2I, Floor> FloorSet { get; set; } = [];
    public Dictionary<Vector2I, Junction> JunctionSet { get; set; } = [];
    #endregion

    #region op
    public void GenerateSets()
    {
        MapRoot.ClearChildren();
        foreach (var floor in FloorSet.Values) MapRoot.AddChild(floor);
        foreach (var junction in JunctionSet.Values) MapRoot.AddChild(junction);
    }
    #endregion
}