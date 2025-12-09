using Godot;
using System;
using GodotSimpleTools;

public partial class PeepholeComp : Component
{
    [Notify,Export] public PeepholePassDirection PassDirection { get => GetPassDirection(); set => SetPassDirection(value); }

    public void ShowPassRoom(Vector2I floorPos)
    {
        // TODO: 加载可穿房间的内容
    }
}
