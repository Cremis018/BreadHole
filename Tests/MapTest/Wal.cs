using Godot;
using System;

public partial class Wal : Node2D
{
    private double time;

    public override void _Process(double delta)
    {
        time += delta;
        if (time < 1) return;
        time = 0;
        var rnd = new Random();
        var dir = rnd.Next(0, 3);
        RotationDegrees = 90 * dir;
    }
}
