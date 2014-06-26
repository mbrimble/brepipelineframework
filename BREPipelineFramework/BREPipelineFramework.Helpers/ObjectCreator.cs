using System;
using System.Reflection.Emit;
using System.Reflection;

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

        public ObjectCreator(ConstructorInfo target)
        {
            CreateObject(target);
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

        public object CreateInstance()
        {
            return methodHandler();
        }
     }
}
