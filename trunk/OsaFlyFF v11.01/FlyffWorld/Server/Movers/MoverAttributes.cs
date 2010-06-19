using System;
using System.Collections;

using System.Text;

namespace FlyffWorld
{
    public class MoverAttributes
    {
        private int[] _attributes = new int[150];
        /// <summary>
        /// Gets or sets an attribute of the mover.
        /// </summary>
        /// <param name="FlyffAttributeID">Gets or sets an attribute of the mover.</param>
        /// <returns>An attribute.</returns>
        public int this[int FlyffAttributeID]
        {
            get
            {
                return _attributes[AttributeIndex(FlyffAttributeID)];
            }
            set
            {
                _attributes[AttributeIndex(FlyffAttributeID)] = value;
            }
        }
        private int AttributeIndex(int real_attribute_id)
        {
            real_attribute_id--;
            return real_attribute_id > 94 ? real_attribute_id - 9906 : real_attribute_id;
        }
        

    }
}
