using System;
using Xunit;

namespace Combo.Tests
{
    public class ContainerTests
    {
        [Fact]
        [Trait("Category", "Container")]
        [Trait("Target", Contracts.Target)]
        public void TestRegisterTypedInstance()
        {
            var container = new Container();
            var foo = new Foo();

            container.Register(typeof(IFoo), foo);
            Assert.Same(foo, container.Resolve<IFoo>());
            Assert.Null(container.Resolve<Foo>());

            Assert.Throws<ArgumentNullException>(() =>
            {
                container.Register(null, null);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                container.Register(typeof(IFoo), null);
            });

            Assert.Throws<ArgumentNullException>(() =>
            {
                container.Register(null, foo);
            });

            Assert.Throws<ArgumentException>(() =>
            {
                container.Register(typeof(IFoo), "Foo");
            });
        }

        [Fact]
        [Trait("Category", "Container")]
        [Trait("Target", Contracts.Target)]
        public void TestRegisterInstance()
        {
            var container = new Container();
            var foo = new Foo();

            container.Register(foo);
            Assert.Same(foo, container.Resolve<Foo>());

            Assert.Throws<ArgumentNullException>(() =>
            {
                container.Register<Foo>(null);
            });
        }

        [Fact]
        [Trait("Category", "Container")]
        [Trait("Target", Contracts.Target)]
        public void TestRegisterSingleton()
        {
            var container = new Container();
            container.Register<Foo>(Lifetime.Singleton);
            Assert.IsType(typeof(Foo), container.Resolve<Foo>());

            var fooA = container.Resolve<Foo>();
            var fooB = container.Resolve<Foo>();
            Assert.Same(fooA, fooB);
        }

        [Fact]
        [Trait("Category", "Container")]
        [Trait("Target", Contracts.Target)]
        public void TestRegisterTransient()
        {
            var container = new Container();
            container.Register<Foo>(Lifetime.Transient);
            Assert.IsType(typeof(Foo), container.Resolve<Foo>());

            var fooA = container.Resolve<Foo>();
            var fooB = container.Resolve<Foo>();
            Assert.NotSame(fooA, fooB);
        }

        [Fact]
        [Trait("Category", "Container")]
        [Trait("Target", Contracts.Target)]
        public void TestRegisterSingletonFactory()
        {
            var container = new Container();
            container.Register(() => new Foo(), Lifetime.Singleton);
            Assert.IsType(typeof(Foo), container.Resolve<Foo>());

            var fooA = container.Resolve<Foo>();
            var fooB = container.Resolve<Foo>();
            Assert.Same(fooA, fooB);

            Assert.Throws<ArgumentNullException>(() =>
            {
                container.Register<Foo>(null, Lifetime.Singleton);
            });
        }

        [Fact]
        [Trait("Category", "Container")]
        [Trait("Target", Contracts.Target)]
        public void TestRegisterTransientFactory()
        {
            var container = new Container();
            container.Register(() => new Foo(), Lifetime.Transient);
            Assert.IsType(typeof(Foo), container.Resolve<Foo>());

            var fooA = container.Resolve<Foo>();
            var fooB = container.Resolve<Foo>();
            Assert.NotSame(fooA, fooB);

            Assert.Throws<ArgumentNullException>(() =>
            {
                container.Register(null, Lifetime.Transient);
            });
        }

        [Fact]
        [Trait("Category", "Container")]
        [Trait("Target", Contracts.Target)]
        public void TestRegisterInterface()
        {
            var container = new Container();
            var foo = new Foo();
            container.Register<IFoo>(foo);
            Assert.Same(foo, container.Resolve<IFoo>());
            Assert.Null(container.Resolve<Foo>());

            Assert.Throws<ArgumentNullException>(() =>
            {
                container.Register<IFoo>(null);
            });
        }

        [Fact]
        [Trait("Category", "Container")]
        [Trait("Target", Contracts.Target)]
        public void TestRegisterTypedInterface()
        {
            var container = new Container();
            var foo = new Foo();

            container.Register<IFoo, Foo>(foo);
            Assert.Same(foo, container.Resolve<IFoo>());
            Assert.Null(container.Resolve<Foo>());
        }

        [Fact]
        [Trait("Category", "Container")]
        [Trait("Target", Contracts.Target)]
        public void TestRegisterSingletonInterface()
        {
            var container = new Container();
            container.Register<IFoo, Foo>(Lifetime.Singleton);
            Assert.IsAssignableFrom<IFoo>(container.Resolve<IFoo>());
            Assert.Null(container.Resolve<Foo>());

            var fooA = container.Resolve<IFoo>();
            var fooB = container.Resolve<IFoo>();
            Assert.Same(fooA, fooB);
        }

        [Fact]
        [Trait("Category", "Container")]
        [Trait("Target", Contracts.Target)]
        public void TestRegisterTransientInterface()
        {
            var container = new Container();
            container.Register<IFoo, Foo>(Lifetime.Transient);
            Assert.IsAssignableFrom<IFoo>(container.Resolve<IFoo>());
            Assert.Null(container.Resolve<Foo>());

            var fooA = container.Resolve<IFoo>();
            var fooB = container.Resolve<IFoo>();
            Assert.NotSame(fooA, fooB);
        }

        [Fact]
        [Trait("Category", "Container")]
        [Trait("Target", Contracts.Target)]
        public void TestRegisterSingletonInterfaceFactory()
        {
            var container = new Container();
            container.Register<IFoo, Foo>(() => new Foo(), Lifetime.Singleton);
            Assert.IsAssignableFrom(typeof(IFoo), container.Resolve<IFoo>());
            Assert.Null(container.Resolve<Foo>());

            var fooA = container.Resolve<IFoo>();
            var fooB = container.Resolve<IFoo>();
            Assert.Same(fooA, fooB);

            Assert.Throws<ArgumentNullException>(() =>
            {
                container.Register<IFoo, Foo>(null, Lifetime.Singleton);
            });
        }

        [Fact]
        [Trait("Category", "Container")]
        [Trait("Target", Contracts.Target)]
        public void TestRegisterTransientInterfaceFactory()
        {
            var container = new Container();
            container.Register<IFoo, Foo>(Lifetime.Transient);
            Assert.IsAssignableFrom(typeof(IFoo), container.Resolve<IFoo>());
            Assert.Null(container.Resolve<Foo>());

            var fooA = container.Resolve<IFoo>();
            var fooB = container.Resolve<IFoo>();
            Assert.NotSame(fooA, fooB);

            Assert.Throws<ArgumentNullException>(() =>
            {
                container.Register<IFoo, Foo>(null, Lifetime.Transient);
            });
        }
    }
}
