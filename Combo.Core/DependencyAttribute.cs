using System;

namespace Combo
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class DependencyAttribute : Attribute
    {
    }
}
