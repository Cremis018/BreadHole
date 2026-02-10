using System.Collections.Generic;

public class A
{
    public string Aaa { get; set; }
    public List<int> Bbb { get; set; } = [];
}

public class B
{
    public A A = new();
    
    public void C()
    {
        Foo(A);
    }
    
    private void Foo(A a)
    {
        a.Aaa = "123";
        a.Bbb.Add(1);
    }
}