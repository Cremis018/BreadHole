public static class ComponentPathProvider
{
    private const string _rootPath = "res://Temporaries/Cremis/Comps/";
    
    public static string Get<T>()
    {
        return $"{_rootPath}{typeof(T).Name}.cs";
    }
}