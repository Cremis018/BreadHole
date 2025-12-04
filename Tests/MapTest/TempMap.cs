using Godot;
using System;

public partial class TempMap : Node2D
{
    [Export] private Player _player;
    
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_right"))
            _player.QueueFree();
    }
}
