using Godot;
using System;
using GodotSimpleTools;

public partial class InfoBoardComp : Node,IComponent
{
    [Notify,Export] public string InfoText { get => GetInfoText(); set => SetInfoText(value); }

    public void ShowText()
    {
        //TODO:将文本内容显示在信息面板上
    }
}
