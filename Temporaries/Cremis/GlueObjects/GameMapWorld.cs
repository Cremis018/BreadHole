using System.Collections.Generic;
using Godot;

public class GameMapWorld(Node2D root)
{
    #region props
    public Node2D MapRoot { get; set; } = root;
    public Dictionary<Vector2I, Floor> FloorSet { get; set; } = [];
    public Dictionary<Vector2I, Junction> JunctionSet { get; set; } = [];
    #endregion
}