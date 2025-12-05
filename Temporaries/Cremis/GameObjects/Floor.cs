using Godot;
using System;

public partial class Floor : Node2D
{
    #region entity
    public Entity E { get; protected set; }

    private void InitEntity()
    {
        E ??= new(this);
        var mapCompositionComp = Component.Create<MapCompositionComp>();
        var markableComp = Component.Create<MarkableComp>();
        var floorComp = Component.Create<FloorComp>();
        mapCompositionComp.Coordinate = Vector2I.One;
        floorComp.Texture = ResourceLoader.Load<Texture2D>("uid://ca34rxbcx0qjy");
        E.BatchAddComponent(mapCompositionComp,markableComp,floorComp);
    }
    #endregion

    #region life
    public override void _Ready()
    {
        InitEntity();
    }
    #endregion
}
