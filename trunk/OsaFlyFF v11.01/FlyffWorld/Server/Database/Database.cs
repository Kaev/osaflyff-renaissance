using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Odbc;
using System.Windows.Forms;
namespace FlyffWorld
{
    partial class Database
    {
        private static OdbcConnection Link;
        public static void Connect(string username, string password, string database, string myodbcDriverVersion, string host)
        {
            string cstring = "Driver={" + myodbcDriverVersion + "};Server=" + host + ";Port=3306;Database=" + database + ";User=" + username + ";Password=" + password + ";Option=3;";
            Link = new OdbcConnection(cstring);
            try
            {
                if (Link.State == ConnectionState.Closed)
                {
                    Link.Open();
                    Log.Write(Log.MessageType.info, "Connection to MySQL server established.");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\r\nMySQL connection failed, shutting down", "MySQL error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Environment.Exit(2);
            }
        }
        public static string GetString(string query, int col)
        {
            try
            {
                OdbcDataReader o = new OdbcCommand(query, Link).ExecuteReader();
                o.Read();
                return o.GetString(col);
            }
            catch (Exception)
            {
                Log.Write(Log.MessageType.error, "Database::GetString(): failed for query {0} & column {1}!", query, col);
                return null;
            }
        }
        /// <summary>
        /// Executes a query with no results. Mostly used with INSERT INTO, UPDATE and DELETE queries.
        /// </summary>
        /// <param name="Query">The query string to execute.</param>
        public static void Execute(string Query,params object[] Arguments)
        {
            try
            {
                OdbcCommand exec = new OdbcCommand(string.Format(Query,Arguments), Link);
                exec.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Log.Write(Log.MessageType.error, "Could not execute nonquery!\r\nQuery: {0}\r\nError:{1}", Query, e.Message);
            }
        }
        public static string Escape(string Source)
        {
            return Source.Replace("'", "\\'");
        }
    }
}
