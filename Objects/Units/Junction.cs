using Godot;
using System;
using Godot.Collections;

public partial class Junction : Node2D, IEntity
{
    [Export] public Dictionary<string, Component> Components { get; private set; } = [];
    public ComponentQuery E { get; private set; }

    public override void _EnterTree()
    {
        E ??= new(this);
    }
}
