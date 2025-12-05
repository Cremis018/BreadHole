using Godot;
using System;
using System.Collections.Generic;

public partial class Player : Node2D
{
    #region entity
    public Entity E { get; protected set; }

    private void InitEntity()
    {
        E ??= new(this);
        var detectorComp = Component.Create<DetectorComp>();
        var mapCompositionComp = Component.Create<MapCompositionComp>();
        E.BatchAddComponent(detectorComp,mapCompositionComp);
    }
    #endregion

    #region life
    public override void _Ready()
    {
        InitEntity();
        E.GetComponent<DetectorComp>().DetectedJunctionChanged += Detect;
    }
    #endregion

    #region respond
    private void Detect(JunctionType junction)
    {
        GD.Print(junction);
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
