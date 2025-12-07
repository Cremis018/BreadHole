using Godot;
using System;

public partial class Junction : Node2D,IEntity
{
    #region entity
    public Entity E { get; protected set; }

    public void InitEntity()
    {
        E ??= new(this);
        var mapCompositionComp = Component.Create<MapCompositionComp>();
        var junctionComp = Component.Create<JunctionComp>();
        var markableComp = Component.Create<MarkableComp>();
        E.BatchAddComponent(mapCompositionComp,junctionComp,markableComp);
    }
    #endregion

    #region life
    public override void _Ready()
    {
        InitEntity();
    }
    #endregion
}
