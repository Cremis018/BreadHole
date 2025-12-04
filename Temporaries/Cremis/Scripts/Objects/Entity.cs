using System;
using System.Collections.Generic;

//还不清楚要不要用
public class Entity
{
    private static ulong _nextId;

    public ulong Id { get; }
    private Dictionary<string, IComponent> _components { get; }
    
    public Entity()
    {
        Id = _nextId++;
    }
    
    public void AddComponent(string name,IComponent component)
    {
        _components.Add(name,component);
    }
    
    public void RemoveComponent(string name)
    {
        _components.Remove(name);
    }
    
    public T GetComponent<T>(string name) where T : class, IComponent
    {
        return _components[name] as T;
    }
    
    public bool HasComponent(string name)
    {
        return _components.ContainsKey(name);
    }
}