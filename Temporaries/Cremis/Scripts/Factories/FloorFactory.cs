public static class FloorFactory
{
    public static Floor GetFloor()
    {
        var floor = Floor.Scene.Instantiate().Duplicate() as Floor;
        floor.InitEntity();
        return floor;
    }
}