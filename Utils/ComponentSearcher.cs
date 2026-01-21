using System.Collections.Generic;
using Godot;

public class ComponentSearcher(IEntity entity)
{
    #region oper
    public bool AddComponent(Component comp, string name = null)
    {
        if (string.IsNullOrEmpty(name)) name = comp.GetType().ToString();
        if (!entity.Components.TryAdd(name, comp)) return false;
        comp._OnAdded(entity);
        return true;
    }
    
    public bool AddComponent<T>(string name = null) where T : Component, new()
    {
        if (string.IsNullOrEmpty(name)) name = typeof(T).ToString();
        var comp = new T();
        if (!entity.Components.TryAdd(name, comp)) return false;
        comp._OnAdded(entity);
        return true;
    }

    public bool AddComponent<T>(out T comp,string name = null) where T : Component, new()
    {
        if (string.IsNullOrEmpty(name)) name = typeof(T).ToString();
        comp = new T();
        if (!entity.Components.TryAdd(name, comp)) return false;
        comp._OnAdded(entity);
        return true;
    }

    public bool RemoveComponent(string name)
    {
        if (!entity.Components.TryGetValue(name, out var comp)) return false;
        if (comp.IsFixed) return false;
        comp._OnRemoved(entity);
        entity.Components.Remove(name);
        return true;
    }

    public bool RemoveComponent<T>() where T : Component
    {
        var name = typeof(T).ToString();
        return RemoveComponent(name);
    }
    
    public T GetComponent<T>(string name) where T : Component
        => entity.Components.GetValueOrDefault(name) as T;
    
    public T GetComponent<T>() where T : Component
    {
        var name = typeof(T).ToString();
        return GetComponent<T>(name);
    }
    #endregion
}