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

    public interface IZoo
    {
        IFoo Foo { get; }
        IBar Bar { get; }
    }

    public class Foo : IFoo
    {
        public string DoFoo() => "Foo";
    }

    public class Bar : IBar
    {
        public string DoBar() => "Bar";
    }
}
