# Combo

A simple implementation of IoC container and dependency injection.

## Usage

```C#

using Combo;

// interfaces
//
public interface IFoo 
{ 
    void DoFoo();  
}

public interface IBar
{
    void DoBar();
}

// implementation
//
public class Foo : IFoo
{
    public void DoFoo()
    {
        Console.WriteLine("Foo");
    }
}

public class Bar : IBar
{
    public void DoBar()
    {
        Console.WriteLine("Bar");
    }
}

// comsumer
//
[Dependent]
public class Stuff
{
    [Dependency]
    public IFoo Foo { get; private set; }

    [Dependency]
    public IBar Bar { get; private set; }
}

// program
//
class Program {

    static void Main(string[] args) 
    {
        Container.Default
            .Register<IFoo, Foo>(Lifetime.Singleton);
            .Register<IBar, Bar>(Lifetime.Singleton);
            .Register<Stuff>(Lifetime.Transient);

        var stuff = Container.Default.Resolve<Stuff>();
        stuff.Foo.DoFoo();   // => "Foo"
        stuff.Bar.DoBar();   // => "Bar"
    }
}

```

## License

MIT License