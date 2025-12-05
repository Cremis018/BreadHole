using System;
using System.Collections.Generic;
using Godot;

public class ComponentPaths
{
    private static Dictionary<Type, string> Components = new()
    {
        {typeof(DetectorComp),"uid://by43ydcedqu5i" },
        {typeof(TriggerComp),"uid://b1dj6npabyqb5" },
        {typeof(MapCompositionComp),"uid://c4gfp6kja5rrn" },
        {typeof(LockComp),"uid://cl0640q7ccvry" },
        {typeof(JunctionComp),"uid://dtju680mmbbpg" },
        {typeof(ItemComp),"uid://d4hqh2jw1j8iy" },
        {typeof(InfoBoardComp),"uid://dfjbhx4sfo3v6" },
        {typeof(HoldingsComp),"uid://dcvkhv7t3emgf" },
        {typeof(FluoroscopyComp),"uid://dpt2epjoflptm" },
        {typeof(FloorComp),"uid://bqp2q2qm64wi2" },
        {typeof(DropsComp),"uid://cwp2h8p0sjo1q" },
        {typeof(MarkableComp),"uid://bquc7lp3lkyfc" },
        {typeof(TestComp),"uid://ja3ld4lfxmsi"}
    };
    
    public static string Get<T>()
    {
        return !Components.ContainsKey(typeof(T)) ?
            throw new Exception($"Component {typeof(T)} not found") 
            : Components[typeof(T)];
    }
}