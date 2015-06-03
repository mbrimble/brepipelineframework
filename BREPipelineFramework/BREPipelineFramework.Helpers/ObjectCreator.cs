using System;
using System.Reflection.Emit;
using System.Reflection;
using System.Collections.Generic;

namespace BREPipelineFramework.Helpers
{
    /// <summary>
    /// Class which can aid in quickly instantiating an object with a parameter-less constructor
    /// Idea for this class comes from http://www.ozcandegirmenci.com/post/2008/02/Create-object-instances-Faster-than-Reflection.aspx
    /// </summary>
    public class ObjectCreator
    {
        delegate object MethodInvoker();
        MethodInvoker methodHandler = null;
        private static Dictionary<string, Type> typeDictionary = new Dictionary<string, Type>();
        
        public ObjectCreator(ConstructorInfo target)
        {
            CreateObject(target);
        }

        public object CreateInstance()
        {
            return methodHandler();
        }

        public static Type ResolveType(string fullyQualifiedClass)
        {
            Type resolvedType;

            //Check to see if the type already exists in the static typeDictionary, if so then use that type, 
            //if not then get the type and add it to the typeDictionary
            if (typeDictionary.TryGetValue(fullyQualifiedClass, out resolvedType))
            {
                resolvedType = typeDictionary[fullyQualifiedClass];
            }
            else
            {
                resolvedType = Type.GetType(fullyQualifiedClass);

                if (resolvedType != null)
                {
                    typeDictionary.Add(fullyQualifiedClass, resolvedType);
                }
                else
                {
                    throw new Exception("Unable to instantiate object of type - " + fullyQualifiedClass);
                }
            }

            return resolvedType;
        }

        public static object CreateConstructorlessInstance(Type type)
        {
            ConstructorInfo info = type.GetConstructor(Type.EmptyTypes);
            ObjectCreator inv = new ObjectCreator(info);
            object o = null;
            o = inv.CreateInstance();
            return o;
        }

        void CreateObject(ConstructorInfo target)
        {
            DynamicMethod dynamic = new DynamicMethod(string.Empty, typeof(object), new Type[0], target.DeclaringType);
            ILGenerator il = dynamic.GetILGenerator();
            il.DeclareLocal(target.DeclaringType);
            il.Emit(OpCodes.Newobj, target);
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ret);

            methodHandler = (MethodInvoker)dynamic.CreateDelegate(typeof(MethodInvoker));
        }
     }
}
