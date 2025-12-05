using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// 全局事件系统，用于组件间通信。
/// </summary>
public static class EventSystem
{
    private static Dictionary<string, List<Action<GameEvent>>> _eventHandlers = new Dictionary<string, List<Action<GameEvent>>>();

    /// <summary>
    /// 订阅事件。
    /// </summary>
    public static void Subscribe<T>(Action<T> handler) where T : GameEvent
    {
        string eventName = typeof(T).Name;
        if (!_eventHandlers.ContainsKey(eventName))
        {
            _eventHandlers[eventName] = new List<Action<GameEvent>>();
        }

        _eventHandlers[eventName].Add((e) => handler((T)e));
    }

    /// <summary>
    /// 取消订阅事件。
    /// </summary>
    public static void Unsubscribe<T>(Action<T> handler) where T : GameEvent
    {
        string eventName = typeof(T).Name;
        if (_eventHandlers.ContainsKey(eventName))
        {
            _eventHandlers[eventName].RemoveAll(h => h == (Action<GameEvent>)((e) => handler((T)e)));
        }
    }

    /// <summary>
    /// 发布事件。
    /// </summary>
    public static void Emit(GameEvent gameEvent)
    {
        string eventName = gameEvent.GetType().Name;
        if (_eventHandlers.ContainsKey(eventName))
        {
            foreach (var handler in _eventHandlers[eventName])
            {
                try
                {
                    handler(gameEvent);
                }
                catch (Exception e)
                {
                    GD.PrintErr($"事件处理错误: {eventName}, 错误: {e.Message}");
                }
            }
        }
    }

    /// <summary>
    /// 清除所有事件订阅。
    /// </summary>
    public static void Clear()
    {
        _eventHandlers.Clear();
    }
}

