using System;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace DotNetVerifier
{
    public class Verifier
    {
        public const string OSAFLYFF = "11.01";
        public const string DOT_NET_MINIMAL_RUNTIME_VERSION = "2.0.50727.1433";
        public static void Verify(string minimalVersion, bool closeApp, string recommendedVersion)
        {
            Version req = new Version(minimalVersion);
            if (((req.Major > Environment.Version.Major) || (req.Major == Environment.Version.Major && req.Minor > Environment.Version.Minor) || (req.Major == Environment.Version.Major && req.Minor == Environment.Version.Minor && req.Build > Environment.Version.Build) || (req.Major == Environment.Version.Major && req.Minor == Environment.Version.Minor && req.Build == Environment.Version.Build && req.Revision > Environment.Version.Revision)))
            {
                Output("Detected bad .NET Framework version information.\r\nConsider upgrading to .NET Framework " + recommendedVersion + ".", closeApp);
                if (closeApp)
                    Environment.Exit(1234);
            }
        }
        private static void Output(string s, bool msgbox)
        {
            if (msgbox)
                MessageBox.Show(s + "\r\nProgram cannot continue.", ".NET Framework - bad version error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.Red;
                foreach (string str in s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    Console.Write(str.PadRight(Console.BufferWidth));
                }
                Console.ResetColor();
            }
        }
    }
}
