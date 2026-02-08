using System;
using System.Collections.Generic;
using System.Linq;

public class ComponentScanner
{
    private readonly List<Type> _scanWhiteList = [];

    public void Add<T>() where T : Component => _scanWhiteList.Add(typeof(T));

    public bool Remove<T>() where T : Component => _scanWhiteList.Remove(typeof(T));
    
    public bool Scan(IEntity entity)
    {
        var components = entity.E.GetAllComponents();
        return components.Any(component => _scanWhiteList.Contains(component.GetType()));
    }
}