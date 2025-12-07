using System;
using System.Collections.Generic;
using Godot;

public static class EventApplier
{
    #region fields
    private static readonly Dictionary<string,Action<TriggerComp,string[]>> _handlers = new()
    {
        {"print",HandlePrintEvent}
    };
    #endregion

    #region op
    public static void Apply(TriggerComp triggerComp)
    {
        var events = triggerComp.RawEvents;
        foreach (var @event in events) ApplySingle(triggerComp, @event);
    }
    
    private static void ApplySingle(TriggerComp triggerComp, string @event)
    {
        var (eventName,args) = ParseEvent(@event);
        _handlers.TryGetValue(eventName, out var handler);
        handler?.Invoke(triggerComp, args);
    }

    private static (string, string[]) ParseEvent(string rawEvent)
    {
        var found = rawEvent.IndexOf('{');
        if (found < 0) return (rawEvent,[]);
        var args = rawEvent.SubstringWithBracket("{", "}").Split(",");
        var eventName = rawEvent[..found];
        return (eventName,args);
    }
    #endregion

    #region handle
    private static void HandlePrintEvent(TriggerComp _, string[] args)
    {
        if (args.Length < 1) return;
        GD.Print(args.Join(","));
    }
    #endregion
}