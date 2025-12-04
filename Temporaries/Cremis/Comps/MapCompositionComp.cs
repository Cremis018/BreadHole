using Godot;
using System;
using GodotSimpleTools;

public partial class MapCompositionComp : Component
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
        InitNodes();
        InitNotifies();
        RenderUpdate();
    }

    private void InitNodes()
    {
        if (GetParent() is Node2D node2D)
            N_Coordinate = node2D;
    }
    
    public override void _ExitTree()
    {
        DestroyNotifies();   
    }
    #endregion
    
    #region render
    public void RenderUpdate()
    {
        RenderPosition(Coordinate);
    }
    
    [Receiver(nameof(CoordinateChanged))]
    private void RenderPosition(Vector2I coordinate)
    {
        N_Coordinate.Position = (Vector2)coordinate * Consts.TILE_SIZE / 2;
    }
    #endregion
}
