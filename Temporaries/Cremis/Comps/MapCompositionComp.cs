using Godot;
using System;
using GodotSimpleTools;

public partial class MapCompositionComp : Node,IComponent
{
    #region props
    [Notify, Export] public Vector2I Coordinate { get => GetCoordinate(); set => SetCoordinate(value); }
    #endregion

    #region nodes
    [ExportGroup("Nodes")]
    [Export] public Node2D N_Coordinate { get; private set; }
    #endregion
    
    #region life
    public override void _Ready()
    {
        InitNotifies();
    }
    
    public override void _ExitTree()
    {
        DestroyNotifies();   
    }
    #endregion
    
    #region render
    [Receiver(nameof(CoordinateChanged))]
    private void RenderPosition(Vector2I coordinate)
    {
        N_Coordinate.Position = (Vector2)coordinate * Consts.TILE_SIZE;
    }
    #endregion
}
