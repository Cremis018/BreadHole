using Godot;
using GodotSimpleTools;

public partial class DetectorComp : Component
{
    #region prop
    [Notify] public IEntity EnterEntity { get => GetEnterEntity(); set => SetEnterEntity(value); }
    [Notify] public IEntity ExitEntity { get => GetExitEntity(); set => SetExitEntity(value); }
    #endregion

    #region nodes
    [ExportGroup("Nodes")]
    [Export] public Area2D N_Area { get; private set; }
    #endregion

    #region life
    public override void _Ready()
    {
        InitNodes();
        InitNotifies();
    }

    private void InitNodes()
    {
        N_Area ??= GetParent().GetNode<Area2D>("Area2D");
    }
    
    public override void _ExitTree()
    {
        DestroyNotifies();
    }
    #endregion

    #region handle
    [Receiver(nameof(N_Area.AreaEntered))]
    private void Enter(Area2D area)
    {
        var node = area.GetParent();
        if (node is not IEntity entity) return;
        EnterEntity = entity;
    }

    [Receiver(nameof(N_Area.AreaExited))]
    private void Exit(Area2D area)
    {
        var node = area.GetParent();
        if (node is not IEntity entity) return;
        ExitEntity = entity;
    }
    #endregion
}
