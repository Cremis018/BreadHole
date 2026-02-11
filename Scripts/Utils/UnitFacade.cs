using Godot;

public static class UnitFacade
{
    public static IUnit CreateUnit<T>(bool toggle = false) where T : IUnit
    {
        if (typeof(T) == typeof(Cell))
            return CreateCell(toggle);
        if (typeof(T) == typeof(Void))
            return CreateVoid();
        if (typeof(T) == typeof(Edge))
            return CreateEdge();
        if (typeof(T) == typeof(Junction))
            return CreateJunction();
        if (typeof(T) == typeof(Placeholder))
            return CreatePlaceholder();
        return null;
    }
    
    private static Cell CreateCell(bool marked)
    {
        var cell = NodeUtil.Create<Cell>();
        cell.E.GetComponent<MarkableComp>().IsMarked = marked;
        cell.E.GetComponent<MarkableComp>().ActivateItems = [new Crumbs()];
        return cell;
    }
    
    private static Void CreateVoid()
    {
        return NodeUtil.Create<Void>();
    }
    
    private static Edge CreateEdge()
    {
        return NodeUtil.Create<Edge>();
    }
    
    private static Junction CreateJunction()
    {
        return NodeUtil.Create<Junction>();
    }
    
    private static Placeholder CreatePlaceholder()
    {
        return NodeUtil.Create<Placeholder>();
    }
}