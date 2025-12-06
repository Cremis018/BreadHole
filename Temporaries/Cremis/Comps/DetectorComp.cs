using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using GodotSimpleTools;

public partial class DetectorComp : Component
{
    #region prop
    [Notify] public int[] DetectedFeatures { get => GetDetectedFeatures(); set => SetDetectedFeatures(value); }
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
        var node = area.GetParent();
        if (node is not Junction junction) return;
        DetectedFeatures = junction.E.GetComponent<JunctionComp>()
            .Features;
    }
    #endregion
}
