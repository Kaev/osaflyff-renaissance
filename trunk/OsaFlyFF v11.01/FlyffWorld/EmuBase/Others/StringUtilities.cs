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
        public const string censor = "I have a potty mouth.";
        public static string FilterChat(string str)
        {
            string[] insults = new string[]
            {
                "asshole","asswipe","bastard","betlog","biatch","bitch","blowjob","bondage","boner","bufu","bullshit","burat",
                "burnik","buttface","buttfuck","butthead","butthole","buttpicker","chink","clit","cock","cooter","cornhole","cum",
                "cunnilingus","cunt","defecation","dick","dildo","dilldo","dipshit","diyoga","douche","dumbass","dyke","fag",
                "fareskin","fatass","fuck","fellatio",
                "fuk","furburger","gago","gazongers","goddam","gonads","gook","horny","hussy","jackass","jizz","kantot","kantut",
                "keps","kike","kraut","kepyas","kupal","lesbian","lesbo","masterbate","motherfucker","nigger","penis","pekpek","pissed","poontang","potah"
            }; // tired of adding anymore from filter_eng :\
            System.Collections.Specialized.NameValueCollection forbidden = new System.Collections.Specialized.NameValueCollection(3); // change 3 to somethin else
            forbidden["\r"] = "\\r";
            forbidden["\n"] = "\\n";
            forbidden["#"] = "##";
            for (int i = 0; i < insults.Length; i++)
                if (str.ToLower().IndexOf(insults[i]) != -1)
                    return censor;
            for (int i = 0; i < forbidden.Count; i++)
                str = str.Replace(forbidden.GetKey(i), forbidden[i]);
            return str;
        }
        [Obsolete("Please avoid using this method as much as possible.. thanks.")]
        public static byte[] GetByteFromHex(string hex)
        {
            hex = hex.Replace(" ", "");
            if (hex.Length % 2 != 0)
                hex = hex.Substring(0, hex.Length - 1);
            byte[] ret = new byte[hex.Length/2];
            for (int i = 0; i < hex.Length; i = i + 2)
                ret[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return ret;
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
