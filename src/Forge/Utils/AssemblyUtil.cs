using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Forge.Utils
{
    public static class AssemblyUtil
    {

        public static Type FindNonGenericDerivedTypeFromCurrentDomain(Type interfaceType)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            return FindNonGenericDerivedType(assemblies, interfaceType);
        }

        public static IEnumerable<Type> FindGenericDerivedTypesFromCurrentDomain(Type genericInterfaceType)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            return FindGenericDerivedTypes(assemblies, genericInterfaceType);
        }

        public static Type FindNonGenericDerivedType(Assembly[] assemblies, Type interfaceType)
        {
            if (assemblies == null || interfaceType == null)
            {
                return null;
            }
            Type foundType = assemblies.SelectMany(assembly => assembly.GetTypes())
                                      .FirstOrDefault(type => type.GetInterfaces().Any(iType => iType == interfaceType) && !type.IsAbstract && !type.IsInterface);
            return foundType;
        }

        public static IEnumerable<Type> FindGenericDerivedTypes(Assembly[] assemblies, Type genericInterfaceType)
        {
            if (assemblies == null || genericInterfaceType == null)
            {
                return null;
            }

            return assemblies.SelectMany(assembly => assembly.GetTypes())
                             .Where(t => t.GetInterfaces().Where(i => i.IsGenericType).Any(i => i.GetGenericTypeDefinition() == genericInterfaceType) && !t.IsAbstract && !t.IsInterface);
        }
    }
}
