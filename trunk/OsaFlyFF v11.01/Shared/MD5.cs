namespace Shared
{
    public class MD5
    {
        public static System.String ComputeString(System.String data)
        {
            return str(System.Security.Cryptography.MD5.Create().ComputeHash(System.Text.Encoding.ASCII.GetBytes(data)));
        }
        private static System.String str(System.Byte[] d)
        {
            System.String str = "";
            for (int i = 0; i < d.Length; i++)
                str += d[i].ToString("X2");
            return str;
        }
    }
}
