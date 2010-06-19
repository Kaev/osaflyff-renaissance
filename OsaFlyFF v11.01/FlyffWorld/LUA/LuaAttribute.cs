using System;
using System.Collections.Generic;

using System.Text;

namespace FlyffWorld
{
    public class LuaFunction : Attribute
    {
        public String FunctionName;
        /// <summary>
        /// Exposes this user-defined function to LUA file scripts. Will work with LUA only after registering the class inside LuaRegisterer.
        /// </summary>
        /// <param name="strFuncName">The name of the LUA function that will be used to call this user-defined function.</param>
        public LuaFunction(String strFuncName)
        {
            FunctionName = strFuncName;
        }
    }
}
