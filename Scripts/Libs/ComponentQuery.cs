using System.Collections.Generic;
using System.Linq;

public class ComponentQuery
{
    private readonly IEntity _entity;

    #region ctor
    public ComponentQuery(IEntity entity)
    { 
        _entity = entity; 
        foreach (var (_, component) in entity.Components) component._OnLoaded(entity);
    }
    #endregion
    
    #region op
    //add
    public bool AddComponent(Component component, string name = null)
    {
        if (string.IsNullOrEmpty(name)) name = component.GetType().ToString();
        if (!_entity.Components.TryAdd(name, component))
            return false;
        component._OnAdded(_entity);
        return true;
    }

    public bool AddComponent<T>(string name = null) where T : Component, new()
    {
        var component = new T();
        return AddComponent(component, name);
    }

    public bool AddComponent<T>(out T component, string name = null) where T : Component, new()
    {
        component = new T();
        return AddComponent(component, name);
    }
    
    //remove
    public bool RemoveComponent(string name)
    {
        if (!_entity.Components.TryGetValue(name, out var component)) return false;
        if (component.IsFixed) return false;
        component._OnRemoved(_entity);
        _entity.Components.Remove(name);
        return true;
    }

    public bool RemoveComponent<T>() where T : Component
    {
        var name = typeof(T).ToString();
        return RemoveComponent(name);
    }
    
    //get
    public T GetComponent<T>(string name) where T : Component
    {
        return _entity.Components.GetValueOrDefault(name) as T;
    }

    public T GetComponent<T>() where T : Component
    {
        var name = typeof(T).ToString();
        return GetComponent<T>(name);
    }
    
    public Component[] GetAllComponents() => _entity.Components.Values.ToArray();

    //has
    public bool HasComponent(string name)
    {
        return _entity.Components.ContainsKey(name);
    }

    public bool HasComponent<T>(string name, out T component) where T : Component
    {
        component = null;
        if (!_entity.Components.TryGetValue(name, out var value)) return false;
        component = value as T;
        return true;
    }

    public bool HasComponent<T>() where T : Component
    {
        var name = typeof(T).ToString();
        return HasComponent(name);
    }

    public bool HasComponent<T>(out T component) where T : Component
    {
        var name = typeof(T).ToString();
        return HasComponent(name, out component);
    } 
    #endregion
}