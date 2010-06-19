using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FlyffWorld
{
    class StringUtilities
    {
        public static string FillString(string[] fill)
        {
            return FillString(fill, 0);
        }
        public static string FillString(string[] fill, int index)
        {
            return FillString(fill, index, fill.Length);
        }
        public static string FillString(string[] fill, int index, int endIndex)
        {
            string ret = "";
            for (int i = index; i < endIndex; i++)
            {
                ret += fill[i];
                if (i != endIndex - 1)
                    ret += " ";
            }
            return ret;
        }
        public static string StringToken(string source, char delimiter, out String remaining)
        {
            int pos = source.IndexOf(delimiter);
            if (pos == -1)
            {
                remaining = source;
                return source;
            }
            else
            {
                remaining = source.Substring(0, pos);
                return source.Substring(++pos, source.Length - pos);
            }
        }
        public static string[] QuoteIgnoringSplit(string source, char delimiter, bool removeQuotes)
        {
            List<string> strlist = new List<string>();
            string current = "";
            bool quote = false;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] == '"')
                {
                    quote = !quote;
                    if (removeQuotes)
                        continue;
                }
                if (source[i] == delimiter)
                {
                    if (!quote)
                    {
                        strlist.Add(current);
                        current = "";
                        continue;
                    }
                }
                current += source[i];
            }
            strlist.Add(current);
            return strlist.ToArray();
        }
        public static string[] QuoteIgnoringSplit(string source, char delimiter)
        {
            return QuoteIgnoringSplit(source, delimiter, false);
        }
    }
}
