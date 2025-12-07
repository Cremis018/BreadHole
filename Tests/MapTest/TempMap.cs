using Godot;
using System;

public partial class TempMap : Node2D
{
    [Export] private Player _player;

    public override void _Ready()
    {
        // var mapWorld = new MapWorld(this);
        // var absPath = ProjectSettings.GlobalizePath("res://Temporaries/Cremis/Levels/lvTest.txt");
        // mapWorld.Load(absPath);
        //
        // mapWorld.Save("user://map/lvTest.txt");
    }
}
