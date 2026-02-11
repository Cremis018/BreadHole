using Godot;
using System;
using Godot.Collections;

public partial class Placeholder : Node2D, IUnit
{
    [Export] public Array<Component> Components { get; private set; } = [];
    public ComponentQuery E { get; private set; }
    
    private CoordModi _coordModi;
    
    public override void _EnterTree()
    {
        E ??= new(this);
        _coordModi = new(this);
    }
}
