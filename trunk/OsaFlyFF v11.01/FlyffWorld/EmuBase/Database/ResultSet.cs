using System;
using System.Collections;
using System.Text;
using System.Data.Odbc;
using System.Data;

namespace FlyffWorld
{
    class ResultSet
    {
        /// <summary>
        /// Gets an object from the resultset.
        /// </summary>
        /// <param name="Column">The column name.</param>
        /// <returns></returns>
        public object this[string Column]
        {
            get
            {
                return Database.QReadObject(QResource, Column);
            }
        }
        private OdbcDataReader QResource;
        private bool Active = true;
        public ResultSet(string Query, params object[] Arguments){
            QResource = Database.QResource(Query, Arguments);
            if (QResource == null)
                Active = false;
        }
        public bool Readbool(string col)
        {
            if (!Active)
                return false;
            return Readint(col) != 0;
        }
        public int Readint(string col)
        {
            if (!Active)
                return 0;
            return Database.QReadint(QResource, col);
        }
        public uint Readint(string col, bool unsigned)
        {
            if (!Active)
                return 0;
            return Database.QReadint(QResource, col, unsigned);
        }
        public string Readstring(string col)
        {
            if (!Active)
                return "";
            return Database.QReadstring(QResource, col);
        }
        public DateTime Readtime(string col)
        {
            if (!Active)
                return Convert.ToDateTime(0);
            return Database.QReadDT(QResource, col);
        }
        public long Readlong(string col)
        {
            if (!Active)
                return 0;
            return Database.QReadlong(QResource, col);
        }
        public ulong Readulong(string col)
        {
            if (!Active)
                return 0;
            return (ulong)Database.QReadlong(QResource, col);
        }
        public float Readfloat(string col)
        {
            if (!Active)
                return 0;
            return Database.QReadfloat(QResource, col);
        }
        public bool Advance()
        {
            if (!Active)
                return false;
            return QResource.Read();
        }
        public void Free()
        {
            if (Active)
            {
                QResource.Close();
                QResource = null;
            }
            Active = false;
        }
    }
}
