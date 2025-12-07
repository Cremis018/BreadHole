using Godot;
using System;
using GodotSimpleTools;

public partial class PeepholeComp : Component
{
    [Notify,Export] public PeepholePassDirection PassDirection { get => GetPassDirection(); set => SetPassDirection(value); }
}
