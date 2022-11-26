using System;

namespace Forge.Extensions;

public static class TypeExtensions
{
    public static bool Inherits<T>(this Type type)
        where T : class =>
        type.Inherits(typeof(T));

    public static bool Inherits(this Type type, Type inheritType) =>
        type.InheritsGeneric(inheritType) || type.InheritsNonGeneric(inheritType);

    public static bool InheritsGeneric(this Type type, Type genericTypeDefinition)
    {
        var currentType = type;

        while(currentType != null)
        {
            if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == genericTypeDefinition)
                return true;

            currentType = currentType.BaseType;
        }

        return false;
    }

    public static bool InheritsNonGeneric(this Type type, Type inheritType)
    {
        var currentType = type;
        while (currentType != null)
        {
            if (currentType == inheritType)
                return true;

            currentType = currentType.BaseType;
        }

        return false;
    }
}
