using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public class Entity(Node node)
{
    private Node Node { get; } = node;
    
    private List<string> _components = [];
    
    public void AddComponent(Component component,string name = null)
    {
        name = GetCompName(component,name);
        component.Name = name;
        Node.AddChild(component);
        _components.Add(name);
    }
    
    public void AddComponentIfNone(string oldName, Component component, string name = null)
    {
        if (!HasComponent(oldName))
            AddComponent(component,name);
    }
    
    public T AddComponentIfNone<T>(T component = null,string name = null) where T : Component
    {
        if (HasComponent<T>()) return GetComponent<T>();
        component ??= Component.Create<T>();
        AddComponent(component,name);
        return GetComponent<T>();
    }

    public void BatchAddComponent(params Component[] components)
    {
        foreach (var component in components) AddComponent(component);
    }

    public void ReplaceComponent(Component component, string name = null)
    {
        name = GetCompName(component,name);
        component.Name = name;
        if (!HasComponent(name))
        {
            AddComponent(component);
            return;
        }
        RemoveComponent(name);
        AddComponent(component);
    }
    
    public void ReplaceComponent<T>(string name = null) where T : Component
    {
        name = GetCompName<T>(name);
        var component = Component.Create<T>();
        component.Name = name;
        if (!HasComponent<T>())
        {
            AddComponent(component);
            return;
        }
        RemoveComponent<T>();
        AddComponent(component);
    }
    
    public void RemoveComponent(string name, bool free = true)
    {
        var component = Node.GetNodeOrNull(name);
        if (component is null) return;
        Node.RemoveChild(component);
        if (free) component.QueueFree();
        _components.Remove(name);
    }
    
    public void RemoveComponent<T>(string name = null) where T : Component
    {
        if (string.IsNullOrWhiteSpace(name)) name = typeof(T).ToString();
        RemoveComponent(name);
    }

    public void BatchRemoveComponent(params string[] names)
    {
        foreach (var name in names) RemoveComponent(name);
    }
    
    public T GetComponent<T>(string name = null) where T : Component
    {
        if (string.IsNullOrWhiteSpace(name)) name = typeof(T).ToString();
        return !HasComponent(name) 
            ? throw new Exception($"Component {name} not found") 
            : Node.GetNode<T>(name);
    }
    
    public bool HasComponent(string name)
    {
        return Node.GetNodeOrNull(name) is not null;
    }
    
    public bool HasComponent<T>(string name = null) where T : Component
    {
        return HasComponent(typeof(T).ToString());
    }

    public void ClearComponent(bool free = true)
    {
        var length = _components.Count;
        for (int i = 0; i < length; i++)
        {
            var component = _components.LastOrDefault();
            RemoveComponent(component,free);
        }
    }

    public void ClearComponentWithBlacklist(string[] blackList, bool free = true)
    {
        var length = _components.Count;
        for (int i = 0; i < length; i++)
        {
            var compName = _components.LastOrDefault();
            if (blackList.Contains(compName)) continue;
            RemoveComponent(compName,free);
        }
    }

    public void ClearComponentWithBlacklist(Component[] blackList, bool free = true)
    {
        var length = _components.Count;
        for (int i = 0; i < length; i++)
        {
            var compName = _components.LastOrDefault();
            if (blackList.Any(x => x.Name == compName)) continue;
            RemoveComponent(compName,free);
        }
    }

    private string GetCompName(Component component, string name = null)
    {
        return string.IsNullOrWhiteSpace(name) 
            ? component.GetType().ToString() 
            : name;
    }
    
    private string GetCompName<T>(string name = null) where T : Component =>
        string.IsNullOrWhiteSpace(name)
            ? typeof(T).ToString()
            : name;
}

public interface IEntity
{
    Entity E { get; }
    void InitEntity();
}