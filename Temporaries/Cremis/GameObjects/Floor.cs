using Godot;
using System;

public partial class Floor : Node2D
{
    #region entity
    private Entity e;

    private void InitEntity()
    {
        e ??= new(this);
        var floorComp = Component.Create<FloorComp>();
        var mapCompositionComp = Component.Create<MapCompositionComp>();
        floorComp.Texture = ResourceLoader.Load<Texture2D>("uid://ca34rxbcx0qjy");
        mapCompositionComp.Coordinate = Vector2I.One;
        e.AddComponent(floorComp);
        e.AddComponent(mapCompositionComp);
    }
    #endregion

    #region life
    public override void _Ready()
    {
        InitEntity();
    }
    #endregion
}
