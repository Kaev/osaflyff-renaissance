using System;
using System.IO;

namespace FlyffCluster
{
    public abstract class PacketBase
    {
        /// <summary>
        /// Gets the bytes in the current memory stream.
        /// </summary>
        public virtual byte[] buffer
        {
            get
            {
                return m_memBuffer.GetBuffer();
            }
            set { }
        }
        protected MemoryStream m_memBuffer;
        /// <summary>
        /// Gets the size of the current memory stream.
        /// </summary>
        public int dwSize
        {
            get
            {
                return (int)m_memBuffer.Length;
            }
        }
        /// <summary>
        /// Gets or sets the pointer of the current memory stream.
        /// </summary>
        public int dwPointer
        {
            get
            {
                return (int)m_memBuffer.Position;
            }
            set
            {
                m_memBuffer.Position = value;
            }
        }
    }
}