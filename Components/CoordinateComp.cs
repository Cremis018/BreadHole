using Godot;
using GodotSimpleTools;

[GlobalClass]
public partial class CoordinateComp : Component
{
    [Export,Notify] public Vector2I Coordinate { get => GetCoordinate(); set => SetCoordinate(value); }
}