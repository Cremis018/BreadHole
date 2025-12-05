using Godot;
using System;
using GodotSimpleTools;

public partial class FloorComp : Component
{
    #region props
    [Notify,Export] public Texture2D Texture { get => GetTexture(); set => SetTexture(value); }
    [Notify,Export] public Texture2D Carpet { get => GetCarpet(); set => SetCarpet(value); }
    [Notify(-1),Export] public int DropsItemId { get => GetDropsItemId(); set => SetDropsItemId(value); }
    #endregion

    #region nodes
    [ExportGroup("Nodes")]
    [Export] public Sprite2D N_Texture { get; private set; }
    [Export] public Sprite2D N_Carpet { get; private set; }
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
        N_Texture ??= GetParent().GetNode<Sprite2D>("Sprite2D");
        N_Carpet ??= GetParent().GetNode<Sprite2D>("Sprite2D2");
    }
    
    public override void _ExitTree()
    {
        DestroyNotifies();
    }
    #endregion
    
    #region render
    public void RenderUpdate()
    {
        RenderTexture(Texture);
        RenderCarpet(Carpet);
    }
    
    [Receiver(nameof(TextureChanged))]
    public void RenderTexture(Texture2D texture)
    {
        if (N_Texture is null) return;
        N_Texture.Texture = texture;
    }
    
    [Receiver(nameof(CarpetChanged))]
    public void RenderCarpet(Texture2D carpet)
    {
        if (N_Carpet is null) return;
        N_Carpet.Texture = carpet;
    }
    #endregion
}
