using Xunit;

namespace Combo.Tests
{
    public class DependencyTests
    {
        public DependencyTests()
        {
            Container.Default
                .Register<IFoo, Foo>(Lifetime.Singleton)
                .Register<IBar, Bar>(Lifetime.Singleton)
                .Register<InjectFieldTestCase>(Lifetime.Transient)
                .Register<InjectPropertyTestCase>(Lifetime.Transient)
                .Register<IZoo, InjectPropertyTestCase>(Lifetime.Transient);
        }

        [Fact]
        [Trait("Category", "DependencyInjection")]
        [Trait("Target", Contracts.Target)]
        public void TestInjectFields()
        {
            var testCase = Container.Default.Resolve<InjectFieldTestCase>();
            Assert.Equal("Foo", testCase.DoFoo());
            Assert.Equal("Bar", testCase.DoBar());
        }

        [Fact]
        [Trait("Category", "DependencyInjection")]
        [Trait("Target", Contracts.Target)]
        public void TestInjectProperties()
        {
            var testCase = Container.Default.Resolve<InjectPropertyTestCase>();
            Assert.Equal("Foo", testCase.Foo?.DoFoo());
            Assert.Equal("Bar", testCase.Bar?.DoBar());

            var zoo = Container.Default.Resolve<IZoo>();
            Assert.Equal("Foo", zoo.Foo.DoFoo());
            Assert.Equal("Bar", zoo.Bar.DoBar());
        }

        [Dependent]
        class InjectFieldTestCase
        {
            [Dependency]
            IFoo _foo = null;

            [Dependency]
            IBar _bar = null;

            public string DoFoo() => _foo.DoFoo();

            public string DoBar() => _bar.DoBar();
        }

        [Dependent]
        class InjectPropertyTestCase : IZoo
        {
            [Dependency]
            public IFoo Foo { get; private set; }

            [Dependency]
            public IBar Bar { get; private set; }
        }
    }
}
