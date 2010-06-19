using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    /// <summary>
    /// Basic bitwise operations.
    /// </summary>
    public class Bitwise
    {
        /// <summary>
        /// Checks if a flag exists in the specific flags holder.
        /// </summary>
        /// <param name="container">The flags holder that will be used to check.</param>
        /// <param name="flag">The flag that will be checked.</param>
        public static bool Exists(int container, int flag)
        {
            return (container & flag) == flag;
        }
        /// <summary>
        /// Adds a flag to a flags holder.
        /// </summary>
        /// <param name="container">The flags holder which the flag will be added to.</param>
        /// <param name="flag">The flag which will be added to the flags holder.</param>
        public static void Add(ref int container, int flag)
        {
            container |= flag;
        }
        /// <summary>
        /// Removes a flag from the flags holder.
        /// </summary>
        /// <param name="container">The flags holder which the flag will be removed from.</param>
        /// <param name="flag">The flag which will be removed from the flags holder.</param>
        public static void Remove(ref int container, int flag)
        {
            container ^= flag;
        }
    }
}
