using Godot;
using System;

public partial class FeaturesTest : Node
{
    // [Export] private Junction _junction;
    [Export] private Floor _floor;
    
    public override void _Ready()
    {
        var featureComp = _floor.E.GetComponent<FeatureComp>();
        featureComp.Features = ["carpet{carpet_test}"];
        // FeatureApplier.(_junction);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton { Pressed: true })
        {
            GD.Print("pressed");
            var featureComp = _floor.E.GetComponent<FeatureComp>();
            featureComp.Features = ["text{hhhhh}","heal{1}"];
        }
    }
}
