using System.Collections.Generic;
using Godot;
using GodotSimpleTools;

[GlobalClass]
public partial class MarkableComp : Component
{
    [Export,Notify] public bool IsMarked { get => GetIsMarked(); set=>SetIsMarked(value); }
    [Export,Notify] public Item[] ActivateItems { get => GetActivateItems(); set=>SetActivateItems(value); }
}