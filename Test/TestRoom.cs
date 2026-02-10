using Godot;
using System;
using System.Diagnostics;

public partial class TestRoom : Node2D
{
    public override void _Ready()
    {
        var b = new B();
        b.A.Aaa = "777";
        b.A.Bbb.Add(2);
        GD.Print(b.A.Aaa);
        GD.Print(b.A.Bbb.Count);
        b.C();
        GD.Print(b.A.Aaa);
        GD.Print(b.A.Bbb.Count);
    }
}
