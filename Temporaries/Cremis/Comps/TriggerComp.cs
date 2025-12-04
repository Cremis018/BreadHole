using Godot;
using System;

public partial class TriggerComp : Component
{
    public Action Event { get; set; }
    
    public void Execute()
    {
        Event();
    }
}
