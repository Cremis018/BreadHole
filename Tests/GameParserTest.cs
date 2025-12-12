using Godot;
using System;
using System.Linq;
using Godot.Collections;

public partial class GameParserTest : Node
{
    [Export] Array<Resource> _res;
    
    public override void _Ready()
    {
        GameParser.Load("res://Temporaries/Cremis/Levels/lvTest.txt");
    }
}
