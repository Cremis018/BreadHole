using Godot;
using System;
using Godot.Collections;

public partial class Edge : Node2D, IEntity, IUnit
{
    [Export] public Array<Component> Components { get; private set; }
    public ComponentQuery E { get; private set; }
}
