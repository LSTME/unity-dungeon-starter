using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using UnityEngine;

namespace Scripts.Map
{
    class BlockBuilderFactory
    {
        private static Dictionary<char, AbstractBlockBuilder> mappedBuilders;

        public static AbstractBlockBuilder getBlockBuilder(char mapChar)
        {
            mapBuilders();

            if (mappedBuilders.ContainsKey(mapChar))
            {
                return mappedBuilders[mapChar];
            }

            return null;
        }

        private static void mapBuilders()
        {
            if (mappedBuilders != null) return;

            mappedBuilders = new Dictionary<char, AbstractBlockBuilder>();

            Type[] typelist = GetTypesInNamespace(Assembly.GetExecutingAssembly(), "Scripts.Map.Blocks");

            foreach (var type in typelist)
            {
                if (TypeHasParent(type, typeof(AbstractBlockBuilder)))
                {
                    AbstractBlockBuilder builder = (AbstractBlockBuilder)Activator.CreateInstance(type);
                    mappedBuilders.Add(builder.forMapChar(), builder);
                }
            }
        }

        private static Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return assembly.GetTypes().Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal)).ToArray();
        }

        private static bool TypeHasParent(Type type, Type parent)
        {
            var testType = type;
            while (testType != null)
            {
                if (testType.BaseType != null)
                {
                    if (testType.BaseType.Name.Equals(parent.Name))
                    {
                        return true;
                    }
                    testType = testType.BaseType;
                }
            }
            return false;
        }
    }

}
