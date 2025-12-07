using Godot;
using GodotSimpleTools;

public partial class FeatureComp : Component
{
    #region props
    [Notify, Export] public string[] Features { get => GetFeatures(); set => SetFeatures(value); }
    #endregion

    #region life
    public override void _Ready()
    {
        InitNotifies();
    }
    
    public override void _ExitTree()
    {
        DestroyNotifies();
    }
    #endregion
    
    #region handle
    [Receiver(nameof(FeaturesChanged))]
    private void ApplyFeatures(string[] features)
    {
        var node = GetParent();
        if (node is not IEntity entity) return;
        FeatureApplier.Apply(entity);
    }
    #endregion
}