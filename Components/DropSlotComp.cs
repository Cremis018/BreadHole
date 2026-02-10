using Godot;
using GodotSimpleTools;

[GlobalClass]
public partial class DropSlotComp : Component
{
    [Export,Notify] public Item DropItem { get=>GetDropItem(); set=>SetDropItem(value); }
}