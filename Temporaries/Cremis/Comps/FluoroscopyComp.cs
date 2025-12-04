using Godot;
using System;
using BreadHole.Temporaries.Cremis.Enums;
using GodotSimpleTools;

public partial class FluoroscopyComp : Component
{
    [Notify,Export] public bool IsOpen { get => GetIsOpen(); set => SetIsOpen(value); }
    [Notify,Export] public FluoroscopyPassDirection PassDirection { get => GetPassDirection(); set => SetPassDirection(value); }
}
