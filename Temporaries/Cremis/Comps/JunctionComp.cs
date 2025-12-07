using Godot;
using System;
using GodotSimpleTools;

public partial class JunctionComp : Component
{
    #region props
    [Notify,Export] public bool IsHorizontal { get => GetIsHorizontal(); set => SetIsHorizontal(value); }
    [Notify,Export] public Texture2D Texture { get => GetTexture(); set => SetTexture(value); }
    [Notify(true),Export] public bool CanPass { get => GetCanPass(); set => SetCanPass(value); }
    //HACK:这个属性会单独成为一个组件，当地图生成完全重构后，再删除这个属性
    [Notify,Export] public int[] Features { get => GetFeatures(); set => SetFeatures(value); }
    #endregion

    #region nodes
    [ExportGroup("Nodes")]
    [Export] public Node2D N_Junction { get; private set; }
    [Export] public MapCompositionComp N_MapComposition { get; private set; }
    [Export] public Sprite2D N_Texture { get; private set; }
    #endregion

    #region life
    public override void _Ready()
    {
        InitNodes();
        InitNotifies();
    }

    private void InitNodes()
    {
        if (GetParent() is Node2D node2D)
            N_Junction = node2D;
        N_Texture ??= GetParent().GetNode<Sprite2D>("Texture");
        N_MapComposition ??= GetParent().GetNode<MapCompositionComp>("MapCompositionComp");
    }
    
    public override void _ExitTree()
    {
        DestroyNotifies();   
    }
    #endregion

    #region render
    public void RenderUpdate()
    {
        OnCoordinateChanged(N_MapComposition.Coordinate);
        RenderTexture(Texture);
    }
    
    [Receiver(nameof(IsHorizontalChanged))]
    private void RenderRotation(bool isHorizontal)
    {
        N_Junction.Rotation = isHorizontal ? Mathf.Pi / 2 : 0;
    }

    [Receiver(nameof(N_MapComposition.CoordinateChanged))]
    private void OnCoordinateChanged(Vector2I coordinate)
    {
        if (N_MapComposition is null) return;
        var isH = coordinate.X % 2 != 0 && coordinate.Y % 2 == 0;
        var isV = coordinate.X % 2 == 0 && coordinate.Y % 2 != 0;
        if (isH) IsHorizontal = true;
        else if (isV) IsHorizontal = false;
    }

    [Receiver(nameof(TextureChanged))]
    private void RenderTexture(Texture2D texture)
    {
        N_Texture.Texture = texture;
    }
    #endregion
}
