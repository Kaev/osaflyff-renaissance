/*
 * Example class to extract .res files
 * Created by xadet (xadetthree@gmail.com)
 * Please do not remove this header if you redistribute this example
 */

/*
 * Resource files reader
 * Code & struct by xadet (as mentioned above)
 * http://forum.ragezone.com/f457/res-file-format-568781/
 */
using System;
using System.IO;
using System.Text;

namespace FlyffWorld.Databases
{
    public struct FlyffResourceSubFile
    {
        public string strFileName;
        public int dwSize;
        public int dwUnknown;
        public int dwOffset;
        public string strContents;
        public void SaveFile(string path)
        {
            StreamWriter s = new StreamWriter(path);
            s.Write(strContents);
            s.Close();
        }
    }

    public class FlyffResourceFile
    {
        public string strResourceName;
        private FileStream m_stream;
        public FlyffResourceSubFile[] files;
        public bool bObfuscated = false;

        public FlyffResourceFile(string strResourceName)
        {
            Constructor(strResourceName, true);
        }
        public FlyffResourceFile(string strResourceName, bool bLoadFiles)
        {
            Constructor(strResourceName, bLoadFiles);
        }
        private void Constructor(string strResourceName, bool bLoadFiles)
        {
            this.strResourceName = strResourceName;
            this.m_stream = File.OpenRead(strResourceName);
            if (bLoadFiles)
                LoadResources();
        }
        /// <summary>
        /// Loads resource files into memory. :|
        /// </summary>
        public void LoadResources()
        {
            BinaryReader br = new BinaryReader(this.m_stream);
            byte key = br.ReadByte();
            bObfuscated = br.ReadByte() != 0;
            int dwSize = br.ReadInt32();
            byte[] header = br.ReadBytes(dwSize);
            // decrypt header
            for (int i = 0; i < dwSize; i++)
                header[i] = (byte)((16 * (key ^ (byte)~header[i])) | ((key ^ (byte)~header[i]) >> 4));
            BinaryReader head = new BinaryReader(new MemoryStream(header));
            head.ReadBytes(7); // "V0.01"
            short count = head.ReadInt16();
            files = new FlyffResourceSubFile[count];
            for (int i = 0; i < count; i++)
            {
                short len = head.ReadInt16();
                files[i] = new FlyffResourceSubFile()
                {
                    strFileName = Encoding.ASCII.GetString(head.ReadBytes(len)),
                    dwSize = head.ReadInt32(),
                    dwUnknown = head.ReadInt32(),
                    dwOffset = head.ReadInt32()
                };
                br.BaseStream.Seek(files[i].dwOffset, SeekOrigin.Begin);
                byte[] buffer = br.ReadBytes(files[i].dwSize);
                if (bObfuscated)
                    for (int l = 0; l < files[i].dwSize; l++)
                        buffer[l] = (byte)((16 * (key ^ (byte)~buffer[l])) | ((key ^ (byte)~buffer[l]) >> 4));
                files[i].strContents = Encoding.ASCII.GetString(buffer);
            }
        }
        public FlyffResourceSubFile GetFileByName(string strFileName)
        {
            for (int i = 0; i < files.Length; i++)
                if (files[i].strFileName.ToLower() == strFileName.ToLower())
                    return files[i];
            return new FlyffResourceSubFile();
        }
    }
}