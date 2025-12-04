using Godot;
using System;

public partial class Pru : Sprite2D
{
    private double time;
    private Color color;

    public override void _Process(double delta)
    {
        time += delta;
        if (time < 1) return;
        time = 0;
        var rnd = new Random();
        color = new Color(rnd.NextSingle(), rnd.NextSingle(), rnd.NextSingle());
        Modulate = color;
    }
}
