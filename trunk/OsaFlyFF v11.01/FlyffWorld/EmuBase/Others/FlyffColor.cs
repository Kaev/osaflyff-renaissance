using System;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    public class FlyffColor
    {
        private byte m_dwRed = 0;
        private byte m_dwGreen = 0;
        private byte m_dwBlue = 0;
        private byte m_dwEffect = 255;
        private bool m_bLittleEndian = true;
        public const string // By Nicco
            COLOR_CHAT_RED = "ff0000",
            COLOR_CHAT_GREEN = "00ff00",
            COLOR_CHAT_BLUE = "0000ff";
        public FlyffColor()
        {

        }
        public FlyffColor(byte dwRed, byte dwGreen, byte dwBlue)
        {
            this.m_dwRed = dwRed;
            this.m_dwGreen = dwGreen;
            this.m_dwBlue = dwBlue;
        }
        public FlyffColor(byte dwRed, byte dwGreen, byte dwBlue, byte dwEffect)
        {
            this.m_dwRed = dwRed;
            this.m_dwGreen = dwGreen;
            this.m_dwBlue = dwBlue;
            this.m_dwEffect = dwEffect;
        }
        public FlyffColor(string hexstr)
        {
            try
            {
                string sb = hexstr.Substring(0, 2),
                       sg = hexstr.Substring(2, 2),
                       sr = hexstr.Substring(4, 2),
                       sa = hexstr.Substring(6, 2);
                m_dwRed = Convert.ToByte(sr, 16);
                m_dwGreen = Convert.ToByte(sg, 16);
                m_dwBlue = Convert.ToByte(sb, 16);
                m_dwEffect = Convert.ToByte(sa, 16);
            }
            catch
            {
                Log.Write(Log.MessageType.warning, "new FlyffColor(string): input was not in the correct format.");
            }
        }
        public int dword
        {
            get
            {
                byte[] array;
                if (m_bLittleEndian)
                    array = new byte[]
                    {
                        m_dwBlue,
                        m_dwGreen,
                        m_dwRed,
                        m_dwEffect
                    };
                else
                    array = new byte[]
                    {
                        m_dwEffect,
                        m_dwRed,
                        m_dwGreen,
                        m_dwBlue
                    };
                return BitConverter.ToInt32(array, 0);
            }
        }
        public string hexstr
        {
            get
            {
                if (m_bLittleEndian)
                    return m_dwBlue.ToString("X2") + m_dwGreen.ToString("X2") + m_dwRed.ToString("X2") + m_dwEffect.ToString("X2");
                else
                    return m_dwEffect.ToString("X2") + m_dwRed.ToString("X2") + m_dwGreen.ToString("X2") + m_dwBlue.ToString("X2");
            }
        }
        public byte dwGreen
        {
            get
            {
                return m_dwGreen;
            }
            set
            {
                m_dwGreen = value;
            }
        }
        public byte dwBlue
        {
            get
            {
                return m_dwBlue;
            }
            set
            {
                m_dwBlue = value;
            }
        }
        public byte dwRed
        {
            get
            {
                return m_dwRed;
            }
            set
            {
                m_dwRed = value;
            }
        }
        public bool bLittleEndian
        {
            get
            {
                return m_bLittleEndian;
            }
            set
            {
                m_bLittleEndian = value;
            }
        }
    }
}
