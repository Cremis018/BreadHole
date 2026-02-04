using Godot;
using GodotSimpleTools;

[GlobalClass]
public partial class DirectionComp : Component
{
    [Export,Notify] public Direction Direction { get => GetDirection(); set => SetDirection(value); }
}