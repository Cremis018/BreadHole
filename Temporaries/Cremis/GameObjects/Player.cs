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
        E.GetComponent<DetectorComp>().DetectedFeaturesChanged += Detect;
    }
    
    public override void _ExitTree()
    {
        E.GetComponent<DetectorComp>().DetectedFeaturesChanged -= Detect;
    }
    #endregion

    #region respond
    private void Detect(string[] feats)
    {
        GD.Print($"探测到特性为[{feats.Join("|")}]的衔接处");
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
