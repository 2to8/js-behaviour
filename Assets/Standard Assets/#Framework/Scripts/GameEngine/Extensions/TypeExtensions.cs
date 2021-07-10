using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

#if ECS
using Unity.Entities;
#endif

namespace GameEngine.Extensions {

static class TypeExtensions {

    public static bool IsDerivedFromOpenGenericType(this Type type, Type openGenericType)
    {
        Contract.Requires(type != null);
        Contract.Requires(openGenericType != null);
        Contract.Requires(openGenericType.IsGenericTypeDefinition);

        return type.GetTypeHierarchy()
            .Where(t => t.IsGenericType)
            .Select(t => t.GetGenericTypeDefinition())
            .Any(t => openGenericType.Equals(t));
    }

#if ECS
    public static ComponentType[] GetComponentType(this IEnumerable<Type> types)
    {
        var componentTypes = new List<ComponentType>();
        types?.ForEach(t => componentTypes.Add(t));

        return componentTypes.ToArray();
    }
#endif

    public static IEnumerable<Type> GetTypeHierarchy(this Type type)
    {
        Contract.Requires(type != null);
        var currentType = type;

        while (currentType != null) {
            yield return currentType;

            currentType = currentType.BaseType;
        }
    }

}

}