using System;
using System.Collections.Generic;
using Godot;

// TODO：当所有事件的处理方法写完后，别忘记把调试的GD.Print()方法删除
public static class EventApplier
{
    #region fields
    private static readonly Dictionary<string,Action<TriggerComp,string[]>> _handlers = new()
    {
        {"print",HandlePrintEvent},
        {"tp",HandleTpEvent},
        {"goto",HandleGotoEvent},
        {"dialog",HandleDialogEvent},
        {"win",HandleWinEvent},
        {"lose",HandleLoseEvent},
        {"give",HandleGiveEvent},
        {"mark",HandleMarkingEvent},
        {"unmark",HandleUnmarkingEvent},
        {"feature",HandleFeatureEvent},
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
        var args = rawEvent.SubstringWithBracket("{", "}").Split('|');
        var eventName = rawEvent[..found];
        return (eventName,args);
    }
    #endregion

    #region handle
    private static void HandlePrintEvent(TriggerComp _, string[] args)
    {
        if (args.Length < 1) return;
        GD.Print(args.Join("|"));
    }

    private static void HandleTpEvent(TriggerComp _, string[] args)
    {
        if (args.Length < 2) return;
        var (x,y) = (int.Parse(args[0]),int.Parse(args[1]));
        if (Game.Instance.Map is null) return;
        Game.Instance.Map.TpPlayer(new Vector2I(x,y));
        GD.Print($"玩家已传送到地图[{x},{y}]");
    }
    
    private static void HandleGotoEvent(TriggerComp _, string[] args)
    {
        if (args.Length < 1) return;
        var room = args[0];
        if (Game.Instance.Map is null) return;
        Game.Instance.Map.GotoRoom(room);
        GD.Print($"已进入地图[{room}]");
    }

    private static void HandleDialogEvent(TriggerComp _, string[] args)
    {
        if (args.Length < 1) return;
        if (Game.Instance.Dialog is null) return;
        Game.Instance.Dialog.Dialog(args);
        GD.Print("已开始对话");
    }

    private static void HandleWinEvent(TriggerComp _, string[] args)
    {
        /*
         TODO: 获取玩家所在的房间的坐标，然后在玩家面向的前方的衔接处
         变为可通关的门（就是定制化的feature事件）
         */
        GD.Print("游戏胜利");
    }
    
    private static void HandleLoseEvent(TriggerComp _, string[] args)
    {
        // TODO：播放失败动画并随后跳转到选关房间（就是定制化的goto事件）
        GD.Print("游戏失败");
    }

    private static void HandleGiveEvent(TriggerComp _, string[] args)
    {
        // TODO：在槽位为M的位置给予玩家ID为N的物品
        if (args.Length < 2) return;
        var itemId = int.Parse(args[0]);
        var slot = int.Parse(args[1]);
        GD.Print($"为玩家的槽位[{slot}]给予物品[{itemId}]");
    }

    private static void HandleMarkingEvent(TriggerComp _, string[] args)
    {
        // TODO：将指定坐标的地板或衔接处设为已标记了的
        if (args.Length < 2) return;
        var x = int.Parse(args[0]);
        var y = int.Parse(args[1]);
        GD.Print($"将地图[{x},{y}]设为已标记");
    }
    
    private static void HandleUnmarkingEvent(TriggerComp _, string[] args)
    {
        // TODO：将指定坐标的地板或衔接处设为未标记了的
        if (args.Length < 2) return;
        var x = int.Parse(args[0]);
        var y = int.Parse(args[1]);
        GD.Print($"将地图[{x},{y}]设为未标记");
    }

    private static void HandleFeatureEvent(TriggerComp _, string[] args)
    {
        // TODO：将指定坐标的地板或衔接处设为指定特征
        if (args.Length < 3) return;
        var x = int.Parse(args[0]);
        var y = int.Parse(args[1]);
        var features = args[2..];
        GD.Print($"将地图[{x},{y}]设为特征[{features.Join("|")}]");
    }
    #endregion
}