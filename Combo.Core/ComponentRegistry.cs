using System;
using System.Collections.Generic;

namespace Combo
{
    public sealed class ComponentRegistry
    {
        private readonly EntryDict _dict = new EntryDict();

        public void AddEntry(Entry entry)
        {
            EntryCollection coll;

            if (!_dict.TryGetValue(entry.Type, out coll)) 
            {
                coll = new EntryCollection();
                _dict[entry.Type] = coll;
            }

            coll.Add(entry);
        }

        public EntryCollection GetEntries(Type type)
        {
            EntryCollection coll;
            return _dict.TryGetValue(type, out coll) ? coll : null;
        }

        public sealed class Entry
        {
            public Type Type { get; internal set; }
            public object Instance { get; internal set; }
            public Lifetime Lifetime { get; internal set; }
            public Func<object> Factory { get; internal set; }

            public object GetInstance()
            {
                if (Instance != null) return Instance;
                if (Factory != null && Lifetime == Lifetime.Transient) return Factory.Invoke();
                if (Factory != null && Lifetime == Lifetime.Singleton) return Instance = Factory.Invoke();
                return null;
            }
        }

        public sealed class EntryCollection : List<Entry> { }
        public sealed class EntryDict : Dictionary<Type, EntryCollection> { }
    }
}