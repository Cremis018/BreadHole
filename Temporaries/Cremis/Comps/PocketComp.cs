using Godot;
using GodotSimpleTools;

public partial class PocketComp : Component
{
    [Notify,Export] public int[] Items { get => GetItems(); set => SetItems(value); }
}