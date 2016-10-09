using System;
using System.Collections.Generic;
using System.Linq;

namespace Combo
{
    public sealed class Container
    {
        private readonly static Lazy<Container> _default = 
            new Lazy<Container>(() => new Container(), true);

        public static Container Default => _default.Value;

        private readonly ComponentRegistry _registry = new ComponentRegistry();

        public Container Register(Type type, object instance)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            if (!instance.IsType(type))
                throw new ArgumentException($"Object must be instance of type: {type}");

            _registry.AddEntry(new ComponentRegistry.Entry()
            {
                Type = type,
                Instance = instance,
                Lifetime = Lifetime.Singleton       
            });

            return this;
        }

        public Container Register<T>(T instance)
        {
            return Register(typeof(T), instance);
        }

        public Container Register<T>(Lifetime lifetime) where T : class, new()
        {
            return Register<T>(() =>
            {
                var x = new T();

                if (typeof(T).HasAttribute<DependentAttribute>())
                {
                    x.InitializeDependencies();
                }

                return x;

            }, lifetime);
        }

        public Container Register<T>(Func<T> factory, Lifetime lifetime)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            _registry.AddEntry(new ComponentRegistry.Entry()
            {
                Type = typeof(T),
                Lifetime = lifetime,
                Factory = () => factory.Invoke()
            });

            return this;
        }

        public Container Register<I, T>(T instance) where T : I
        {
            return Register(typeof(I), instance);
        }

        public Container Register<I, T>(Lifetime lifetime) where T : class, I, new()
        {
            return Register<I, T>(() =>
            {
                var x = new T();

                if (typeof(T).HasAttribute<DependentAttribute>())
                {
                    x.InitializeDependencies();
                }

                return x;

            }, lifetime);
        }

        public Container Register<I, T>(Func<T> factory, Lifetime lifetime) where T : I
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            _registry.AddEntry(new ComponentRegistry.Entry()
            {
                Type = typeof(I),
                Lifetime = lifetime,
                Factory = () => factory.Invoke()
            });

            return this;
        }

        public T Resolve<T>()
        {
            var instance = Resolve(typeof(T));
            return (instance != null) ? (T)instance : default(T);
        }

        public IEnumerable<T> ResolveMany<T>()
        {
            return ResolveMany(typeof(T)).Cast<T>();
        }

        public object Resolve(Type type)
        {
            var entries = _registry.GetEntries(type);
            if (entries == null || entries.Count == 0) return null;
            return entries.FirstOrDefault()?.GetInstance();
        }

        public IEnumerable<object> ResolveMany(Type type)
        {
            var entries = _registry.GetEntries(type);
            if (entries == null || entries.Count == 0) return new object[] { };
            return entries.Select(x => x.GetInstance());
        }
    }
}
