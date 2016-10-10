namespace Combo.Tests
{
    public interface IFoo
    {
        string DoFoo();
    }

    public interface IBar
    {
        string DoBar();
    }

    public interface ICommon
    {
        string Name { get; }
    }

    public interface IZoo
    {
        IFoo Foo { get; }
        IBar Bar { get; }
    }

    public class Foo : IFoo, ICommon
    {
        public string Name => "Foo";
        public string DoFoo() => "Foo";
    }

    public class Bar : IBar, ICommon
    {
        public string Name => "Bar";
        public string DoBar() => "Bar";
    }
}
