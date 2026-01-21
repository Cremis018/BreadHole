using Godot;
using System;
using Godot.Collections;

public partial class TestEntity : Node,IEntity
{
    #region impl
    [Export] public Dictionary<string, Component> Components { get; private set; } = [];
    public ComponentSearcher E { get; private set; }
    #endregion

    #region ctor
    public override void _Ready()
    {
        E ??= new(this);
        var comp = new TestComp();
        comp.TestProp = "hello world";
        E.AddComponent(comp);
    }
    #endregion

    #region oper
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_left"))
        {
            var b = E.RemoveComponent<TestComp>();
        }
    }
    #endregion
}
