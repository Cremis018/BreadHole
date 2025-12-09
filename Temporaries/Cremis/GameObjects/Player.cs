using Godot;

public partial class Player : Node2D,IEntity
{
    #region entity
    public Entity E { get; protected set; }

    public void InitEntity()
    {
        E ??= new(this);
        var detectorComp = Component.Create<DetectorComp>();
        var mapCompositionComp = Component.Create<MapCompositionComp>();
        var pocketComp = Component.Create<PocketComp>();
        E.BatchAddComponent(detectorComp,mapCompositionComp,pocketComp);
    }
    #endregion

    #region life
    public override void _Ready()
    {
        InitEntity();
        E.GetComponent<DetectorComp>().EnterEntityChanged += DetectEnter;
        E.GetComponent<DetectorComp>().ExitEntityChanged += DetectExit;
    }
    
    public override void _ExitTree()
    {
        E.GetComponent<DetectorComp>().EnterEntityChanged -= DetectEnter;
        E.GetComponent<DetectorComp>().ExitEntityChanged -= DetectExit;
    }
    #endregion

    #region respond
    private void DetectEnter(IEntity entity)
    {
        if (entity is not Node node) return;
        GD.Print($"探测到实体{node.Name}位于区域");
        Game.Instance.TriggerHandler.TriggerEntity(entity);
    }
    
    private void DetectExit(IEntity entity)
    {
        if (entity is not Node node) return;
        GD.Print($"探测到实体{node.Name}离开区域");
        Game.Instance.TriggerHandler.TriggerEntity(entity);
    }
    #endregion

    public override void _Process(double delta)
    {
        if (Input.IsActionPressed("ui_right"))
            Position += Vector2.Right * 100 * (float)delta;
        if (Input.IsActionPressed("ui_left"))
            Position += Vector2.Left * 100 * (float)delta;
        if (Input.IsActionPressed("ui_up"))
            Position += Vector2.Up * 100 * (float)delta;
        if (Input.IsActionPressed("ui_down"))
            Position += Vector2.Down * 100 * (float)delta;
    }
}
