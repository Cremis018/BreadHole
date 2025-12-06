using Godot;
using GodotSimpleTools;

public partial class FeatureComp : Component
{
    [Notify, Export] public string[] Features { get => GetFeatures(); set => SetFeatures(value); }
}