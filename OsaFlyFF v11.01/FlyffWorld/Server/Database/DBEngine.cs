using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Odbc;

namespace FlyffWorld
{
    partial class Database 
    {
        public static object QReadObject(OdbcDataReader QResource, string cname)
        {
            try { return QResource.GetValue(QResource.GetOrdinal(cname)); }
            catch (Exception e) { Log.Write(Log.MessageType.error, "Unable to get field {0}: {1}", cname, e.Message); return 0; }
        }
        public static OdbcDataReader QResource(string Query, params object[] Arguments)
        {
            try { return new OdbcCommand(string.Format(Query, Arguments), Link).ExecuteReader(); }
            catch (Exception e) { Log.Write(Log.MessageType.error, "Could not execute query!\r\nQuery: {0}\r\nError:{1}", Query, e.Message); return null; }
        }
        public static DateTime QReadDT(OdbcDataReader QResource, string cname)
        {
            try { return QResource.GetDateTime(QResource.GetOrdinal(cname)); }
            catch (Exception e)
            {
                try { return (DateTime)QResource.GetValue(QResource.GetOrdinal(cname)); }
                catch (Exception ee) { Log.Write(Log.MessageType.error, "Field {0} is not DT-based field. Database.QReadDT failed.\r\nError 1: {1}\r\nError 2: {2}", cname, e.Message, ee.Message); return Convert.ToDateTime(0); }
            }
        }
        public static long QReadlong(OdbcDataReader QResource, string ColumnName)
        {
            try { return QResource.GetInt64(QResource.GetOrdinal(ColumnName)); }
            catch (Exception e)
            {
                try { return Convert.ToInt64(QResource.GetValue(QResource.GetOrdinal(ColumnName))); }
                catch (Exception ee) { Log.Write(Log.MessageType.error, "Field {0} is not long-based field. Database.QReadlong failed.\r\nError 1: {1}\r\nError 2: {2}", ColumnName, e.Message, ee.Message); return 0; }
            }
        }
        public static int QReadint(OdbcDataReader QResource, string ColumnName)
        {
            try { return QResource.GetInt32(QResource.GetOrdinal(ColumnName)); }
            catch (Exception e)
            {
                try { return int.Parse(QResource.GetString(QResource.GetOrdinal(ColumnName))); }
                catch (Exception ee) { Log.Write(Log.MessageType.error, "Field {0} is not integer-based field. Database.QReadint failed.\r\nError 1: {1}\r\nError 2: {2}", ColumnName, e.Message, ee.Message); return 0; }
            }
        }
        public static uint QReadint(OdbcDataReader QResource, string ColumnName, bool unsigned)
        {
            try { return Convert.ToUInt32(QResource.GetValue(QResource.GetOrdinal(ColumnName))); }
            catch (Exception e) { Log.Write(Log.MessageType.error, "Field {0} is not uinteger-based field. Database.QReadint failed.\r\n{1}", ColumnName, e.Message); return 0; }
        }
        public static string QReadstring(OdbcDataReader QResource, string ColumnName)
        {
            try { return QResource.GetString(QResource.GetOrdinal(ColumnName)); }
            catch (Exception e) { Log.Write(Log.MessageType.error, "Field {0} is not string-based field. Database.QReadstring failed.\r\n{1}", ColumnName, e.Message); return null; }
        }
        public static float QReadfloat(OdbcDataReader QResource, string ColumnName)
        {
            try { return QResource.GetFloat(QResource.GetOrdinal(ColumnName)); }
            catch (Exception e)
            {
                try { return Convert.ToSingle(QResource.GetValue(QResource.GetOrdinal(ColumnName))); }
                catch (Exception ee) { Log.Write(Log.MessageType.error, "Field {0} is not float-based field. Database.QReadfloat failed.\r\nError 1: {1}\r\nError 2: {2}", ColumnName, e.Message, ee.Message); return 0; }
            }
        }
    }
}
