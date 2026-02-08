using Godot;
using System;
using Godot.Collections;

[GlobalClass]
public partial class Cell : Node2D, IEntity
{
    [Export] public Dictionary<string, Component> Components { get; private set; }
    public ComponentQuery E { get; private set; }
    
    public override void _EnterTree()
    {
        E ??= new(this);
    }
}
