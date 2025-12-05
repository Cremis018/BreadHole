using Godot;
using System;
using GodotSimpleTools;

public partial class MarkableComp : Component
{
    #region props
    [Notify,Export] public bool WasMarked { get => GetWasMarked(); set => SetWasMarked(value); }
    #endregion
}
