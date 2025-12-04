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
        _components.Add(component.Name);
        EntityNode.AddChild(component);
    }
    
    public void RemoveComponent(string name)
    {
        var component = EntityNode.GetNode(name);
        EntityNode.RemoveChild(component);
        component.QueueFree();
        _components.Remove(name);
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