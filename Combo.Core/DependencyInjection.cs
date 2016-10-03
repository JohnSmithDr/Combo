using System.Reflection;

namespace Combo
{
    public static class DependencyInjection
    {
        public static void InitializeDependencies<T>(this T host) where T : class
            => InitializeDependencies(host, Container.Default);

        public static void InitializeDependencies<T>(this T host, Container container) where T : class
        {
            // initialize fields
            var fields = host.GetType().GetRuntimeFields();

            foreach (var field in fields)
            {
                var da = field.GetCustomAttribute<DependencyAttribute>();
                if (da != null)
                {
                    var fieldType = field.FieldType;
                    var typedInstance = container.Resolve(fieldType);
                    if (typedInstance != null)
                    {
                        field.SetValue(host, typedInstance);
                    }
                }
            }

            // initialize properties
            var properties = host.GetType().GetRuntimeProperties();

            foreach (var prop in properties)
            {
                var da = prop.GetCustomAttribute<DependencyAttribute>();
                if (da != null)
                {
                    var propType = prop.PropertyType;
                    var typedInstance = container.Resolve(propType);
                    if (typedInstance != null)
                    {
                        prop.SetValue(host, typedInstance);
                    }
                }
            }
        }
    }
}
