using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
namespace FlyffWorld
{
    public class Config
    {
        public static int GetInt(string file, string key, int idefault)
        {
            IniFile _if = new IniFile(file);
            string val = _if.ReadValue(key, idefault.ToString());
            _if.destroy();
            try
            {
                return int.Parse(val);
            }
            catch (Exception e)
            {
                Log.Write(Log.MessageType.warning, "Error parsing {0} into an integer: {1} - returning {2}.", val, e.Message, idefault);
                return idefault;
            }
        }

        public static string GetString(string file, string key, string idefault)
        {
            IniFile _if = new IniFile(file);
            string str = _if.ReadValue(key, idefault);
            _if.destroy();
            return str;
        }
    }
    /// <summary>
    /// Parse settings from ini files.
    /// </summary>
    public class IniFile
    {
        public int ReadInt(string key, int idefault)
        {
            try
            {
                return int.Parse(ReadValue(key, idefault.ToString()));
            }
            catch (Exception)
            {
                Log.Write(Log.MessageType.error, "Couldn't read int value {0} from inifile", key);
                return idefault;
            }

        }
        private bool init = false;
        private string _iniFileName;
        ~IniFile()
        {
            s.Close();
        }
        public void destroy()
        {
            if (init)
                s.Close();
        }
        public IniFile(string iniFileName)
        {
            _iniFileName = iniFileName;
            try
            {
                s = new StreamReader(_iniFileName);
                init = true;
            }
            catch (Exception e)
            {
                Log.Write(Log.MessageType.info, "Error opening {0}: {1}", _iniFileName, e.Message);
                init = false;
            }
        }
        private StreamReader s;
        public string ReadValue(string key, string defVal)
        {
            if (!init)
                return defVal;
            s.DiscardBufferedData();
            s.BaseStream.Seek(0, SeekOrigin.Begin);
            string retVal = defVal; // return value is default value for now.
            try
            {
                while (true)
                {
                    string line = s.ReadLine();
                    if (line == null)
                        break;
                    try
                    {
                        if (line.Trim().Substring(0, 2).Equals("##"))
                            continue;
                    }
                    catch (Exception) { }
                    string[] split = line.Split(new char[] { '=' }, 2);
                    if (split.Length < 2)
                        continue;
                    string fkey = split[0].Trim(), fval = split[1].Trim();
                    if (fkey.ToLower().Equals(key.ToLower()))
                    {
                        //  get value and ignore comments
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < fval.Length; i++)
                        {
                            string current = fval.Substring(i, 1);
                            try
                            {
                                current += fval.Substring(i + 1, 1);
                            }
                            catch (Exception) { }
                            if (!current.Equals("##"))
                                sb.Append(fval.Substring(i, 1));
                            else
                                break;
                        }
                        retVal = sb.ToString().Trim();
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Write(Log.MessageType.error, "Failed to read value from file {0}: {1}", _iniFileName, e.Message);
                return defVal;
            }
            return retVal;
        }
        public string IniFileName { get { return _iniFileName; } }
    }
}