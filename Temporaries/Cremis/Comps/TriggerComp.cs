using Godot;
using System;

public partial class TriggerComp : Node,IComponent
{
    public Action Event { get; set; }
    
    public void Execute()
    {
        Event();
    }
}
