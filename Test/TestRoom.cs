using Godot;
using System;
using System.Diagnostics;

public partial class TestRoom : Node2D
{
    [Export(PropertyHint.ResourceType)] public Resource a;
    
    public override void _Ready()
    {
        object[] objects = new object[1000000];
        var random = new Random();
        
        // 填充测试数据
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i] = random.Next(2) == 0 ? new string('a', 10) : new object();
        }
        
        var sw = Stopwatch.StartNew();
        
        // 方法1: 传统is + cast
        int count1 = 0;
        foreach (var obj in objects)
        {
            if (obj is string)
            {
                var str = (string)obj;
                count1 += str.Length;
            }
        }

        GD.Print($"传统方式: {sw.ElapsedMilliseconds}ms, Count: {count1}");
        
        sw.Restart();
        
        // 方法2: 模式匹配 (推荐)
        int count2 = 0;
        foreach (var obj in objects)
        {
            if (obj is string str)
            {
                count2 += str.Length;
            }
        }
        GD.Print($"模式匹配: {sw.ElapsedMilliseconds}ms, Count: {count2}");
        
        sw.Restart();
        
        // 方法3: as操作符
        int count3 = 0;
        foreach (var obj in objects)
        {
            var str = obj as string;
            if (str != null)
            {
                count3 += str.Length;
            }
        }
        GD.Print($"as操作符: {sw.ElapsedMilliseconds}ms, Count: {count3}");
    }
}
