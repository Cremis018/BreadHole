using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using GodotSimpleTools;

public partial class DetectorComp : Component
{
    #region prop
    [Notify] public List<StringName> DetectedGroups { get => GetDetectedGroups(); set => SetDetectedGroups(value); }
    #endregion

    #region nodes
    [ExportGroup("Nodes")]
    [Export] public Area2D N_Area { get; private set; }
    #endregion

    #region life
    public override void _Ready()
    {
        InitNodes();
        N_Area.AreaEntered += Detect;
    }

    private void InitNodes()
    {
        N_Area ??= GetParent().GetNode<Area2D>("Area2D");
    }
    #endregion

    #region op
    private void Detect(Area2D area)
    {
        var node = area.Owner;
        DetectedGroups = node.GetGroups().ToList();
    }
    #endregion
}
