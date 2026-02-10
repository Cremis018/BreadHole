using System.Collections.Generic;
using System.Linq;

public class ComponentQuery
{
    private readonly IEntity _entity;

    #region ctor
    public ComponentQuery(IEntity entity)
    { 
        _entity = entity; 
        foreach (var component in entity.Components) component._OnLoaded(entity);
    }
    #endregion
    
    #region op
    //add
    public bool AddComponent(Component component, string name = null)
    {
        if (_entity.Components.Any(c => c.GetType() == component.GetType() && c.UniqueName == name)) return false;
        component.UniqueName = name;
        _entity.Components.Add(component);
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
    public bool RemoveComponent(Component component) => _entity.Components.Remove(component);

    public bool RemoveComponent(string name) => _entity.Components.Remove(_entity.Components.FirstOrDefault(c => c.UniqueName == name));
    
    public bool RemoveComponent<T>() where T : Component => _entity.Components.Remove(_entity.Components.FirstOrDefault(c => c.GetType() == typeof(T)));

    public int RemoveComponents<T>() where T : Component
    {
        var components = _entity.Components.Where(c => c.GetType() == typeof(T)).ToList();
        var count = components.Count;
        foreach (var _ in components.Where(c => _entity.Components.Remove(c))) count--;

        return count;
    }
    //get
    public T GetComponent<T>(string name) where T : Component => _entity.Components.FirstOrDefault(c => c.UniqueName == name) as T;

    public T GetComponent<T>() where T : Component => _entity.Components.FirstOrDefault(c => c.GetType() == typeof(T)) as T;

    public Component[] GetComponents<T>() where T : Component => _entity.Components.Where(c => c.GetType() == typeof(T)).ToArray();

    public Component[] GetAllComponents() => _entity.Components.ToArray();

    //has
    public bool HasComponent(string name) => _entity.Components.Any(c =>c.UniqueName == name);

    public bool HasComponent<T>(string name, out T component) where T : Component
    {
        component = _entity.Components.FirstOrDefault(c => c.UniqueName == name) as T;
        return component is not null;
    }

    public bool HasComponent<T>() where T : Component => _entity.Components.Any(c => c.GetType() == typeof(T));

    public bool HasComponent<T>(out T component) where T : Component
    {
        component = _entity.Components.FirstOrDefault(c => c.GetType() == typeof(T)) as T;
        return component is not null;
    } 
    #endregion
}