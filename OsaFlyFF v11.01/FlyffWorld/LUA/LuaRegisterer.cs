using System;
using System.Collections;
using System.Reflection;
using System.Text;
using LuaInterface;

namespace FlyffWorld
{
    public class LuaRegisterer
    {
        public static Lua registerLuaFunctions(Object pTarget, Lua pLuaVM)
        {
            if (pLuaVM == null)
                return null;
            
            Type pTrgType =  pTarget.GetType();
            foreach (MethodInfo mInfo in pTrgType.GetMethods())
            {
                foreach (Attribute attr in Attribute.GetCustomAttributes(mInfo))
                {
                    if (attr.GetType() == typeof(LuaFunction))
                    {
                        LuaFunction pAttr = (LuaFunction)attr;
                        pLuaVM.RegisterFunction(pAttr.FunctionName, pTarget, mInfo);
                    }
                }
            }
            return pLuaVM;
        }
        public static Lua registerFields(Object pTarget, Lua pLuaVM)
        {
            foreach (FieldInfo field in pTarget.GetType().GetFields())
            {
                foreach (Attribute a in field.GetCustomAttributes(true))
                    if (a is LuaMember)
                        pLuaVM[field.Name] = field.GetValue(pTarget).ToString();
            }
            return pLuaVM;
        }
    }
}