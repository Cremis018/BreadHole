using System;
using System.Collections.Generic;
using Godot;

public class Entity(Node entityNode)
{
    private Node EntityNode { get; } = entityNode;
    
    private List<string> _components = [];
    
    public void AddComponent(Component component,string name = null)
    {
        component.Name = string.IsNullOrWhiteSpace(name) 
            ? component.GetType().ToString() 
            : name;
        EntityNode.AddChild(component);
        _components.Add(component.Name);
    }

    public void BatchAddComponent(params Component[] components)
    {
        foreach (var component in components) AddComponent(component);
    }
    
    public void RemoveComponent(string name)
    {
        var component = EntityNode.GetNodeOrNull(name);
        if (component is null) return;
        EntityNode.RemoveChild(component);
        component.QueueFree();
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
            : EntityNode.GetNode<T>(name);
    }
    
    public bool HasComponent(string name)
    {
        return EntityNode.GetNodeOrNull(name) is not null;
    }

    public void ClearComponent()
    {
        foreach (var component in _components)
        {
            RemoveComponent(component);
        }
    }
}