using Godot;
using System;

public partial class Floor : Node2D,IEntity
{
    #region entity
    public static readonly PackedScene Scene = ResourceLoader
        .Load<PackedScene>("res://Temporaries/Cremis/GameObjects/floor.tscn");
    
    public Entity E { get; protected set; }

    public void InitEntity()
    {
        E ??= new(this);
        var mapCompositionComp = Component.Create<MapCompositionComp>();
        var markableComp = Component.Create<MarkableComp>();
        var floorComp = Component.Create<FloorComp>();
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
