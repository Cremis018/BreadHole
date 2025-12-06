using Godot;
using System;
using GodotSimpleTools;

public partial class FloorComp : Component
{
    #region props
    [Notify,Export] public Texture2D Texture { get => GetTexture(); set => SetTexture(value); }
    [Notify(-1),Export] public int DropsItemId { get => GetDropsItemId(); set => SetDropsItemId(value); }
    #endregion

    #region nodes
    [ExportGroup("Nodes")]
    [Export] public Sprite2D N_Texture { get; private set; }
    [Export] public MarkableComp C_Markable { get; private set; }
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
        C_Markable ??= GetParent().GetNode<MarkableComp>("MarkableComp");
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
    }
    
    [Receiver(nameof(TextureChanged))]
    public void RenderTexture(Texture2D texture)
    {
        if (N_Texture is null) return;
        N_Texture.Texture = texture;
    }

    [Receiver(nameof(C_Markable.WasMarkedChanged))]
    public void RenderMark(bool wasMarked)
    {
        if (N_Texture is null) return;
        N_Texture.SelfModulate = wasMarked ? Colors.Red : Colors.White;
    }
    #endregion
}
