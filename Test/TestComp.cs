using Godot;

[GlobalClass]
public partial class TestComp : Component
{
    [Export] public string TestProp { get; set; }

    public override void _OnAdded(IEntity entity)
    {
        GD.Print($"TestComp added");
        if (entity is Node node)
            GD.Print(node.Name);
    }

    public override void _OnRemoved(IEntity entity)
    {
        GD.Print($"TestComp removed");
        if (entity is Node node)
            GD.Print(node.Name);
    }
}