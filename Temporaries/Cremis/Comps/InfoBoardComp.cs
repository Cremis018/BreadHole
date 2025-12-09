using Godot;
using System;
using GodotSimpleTools;

public partial class InfoBoardComp : Component
{
    [Notify,Export] public string[] InfoText { get => GetInfoText(); set => SetInfoText(value); }

    public void ShowText()
    {
        var node = GetParent();
        if (node is not Node2D node2D) return;
        Game.Instance.Dialogue.PopupText(InfoText,node2D);
    }
}
