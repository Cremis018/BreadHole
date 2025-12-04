using Godot;
using System;
using GodotSimpleTools;

public partial class ItemComp : Node,IComponent
{
    [Notify,Export] public int ItemId { get => GetItemId(); set => SetItemId(value); }
    [Notify,Export] public string ItemName { get => GetItemName(); set => SetItemName(value); }
    [Notify,Export] public string ItemDesc { get => GetItemDesc(); set => SetItemDesc(value); }
    [Notify,Export] public ItemType ItemType { get => GetItemType(); set => SetItemType(value); }
    [Notify,Export] public Texture2D HoldingIcon { get => GetHoldingIcon(); set => SetHoldingIcon(value); }
    [Notify,Export] public Texture2D DropsIcon { get => GetDropsIcon(); set => SetDropsIcon(value); }
    public Action UsingLogic { get; set; }
    public Action DroppingLogic { get; set; }
}
