using Godot;
using GodotSimpleTools;

public partial class HealComp : Component
{
    [Notify,Export] public int HealAmount { get => GetHealAmount(); set => SetHealAmount(value); }
}