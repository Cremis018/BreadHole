using Godot;
using System;

public partial class SubstringWithBracketTest : Node
{
    public override void _Ready()
    {
        // this.AddNodeIfNone<Sprite2D>("Hello");
        // var s = new Sprite2D();
        // AddChild(s);
        // GD.Print(s.GetPath());
        this.AddNodeIfNone<Sprite2D>("Hello");
    }

    
}