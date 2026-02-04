using System;
using System.Collections.Generic;

public static class ItemNameStorage
{
    private static readonly object _lock = new();
    private static Dictionary<Type, string> _typeNameMap;

    public static void UpdateData(Dictionary<Type, string> data)
    {
        lock (_lock)
        {
            _typeNameMap = data;
        }
    }
    
    
}