using System;
using System.Collections.Generic;

namespace Combo
{
    public sealed class Container
    {
        private readonly static Lazy<Container> _default = 
            new Lazy<Container>(() => new Container(), true);

        public static Container Default => _default.Value;

        private readonly IDictionary<Type, Registry> _registry = 
            new Dictionary<Type, Registry>();

        public Container Register(Type type, object instance)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if (!instance.IsType(type))
            {
                throw new ArgumentException($"must be instance of type: {type}");
            }

            _registry[type] = new Registry
            {
                Instance = instance,
                Lifetime = Lifetime.Singleton
            };
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
            {
                throw new ArgumentNullException(nameof(factory));
            }

            var type = typeof(T);
            _registry[type] = new Registry
            {
                Lifetime = lifetime,
                Factory = () => factory.Invoke()
            };
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
            {
                throw new ArgumentNullException(nameof(factory));
            }

            var type = typeof(I);
            _registry[type] = new Registry
            {
                Lifetime = lifetime,
                Factory = () => factory.Invoke()
            };
            return this;
        }

        public T Resolve<T>()
        {
            var type = typeof(T);
            var instance = Resolve(type);
            return (instance != null) ? (T)instance : default(T);
        }

        public object Resolve(Type type)
        {
            Registry ci;

            if (!_registry.TryGetValue(type, out ci)) return null;

            if (ci.Instance != null) return ci.Instance;

            if (ci.Lifetime == Lifetime.Transient) return ci.Factory.Invoke();

            if (ci.Lifetime == Lifetime.Singleton)
            {
                ci.Instance = ci.Factory.Invoke();
                return ci.Instance;
            }

            return null;
        }

        sealed class Registry
        {
            public object Instance { get; set; }
            public Lifetime Lifetime { get; set; }
            public Func<object> Factory { get; set; }
        }
    }
}
