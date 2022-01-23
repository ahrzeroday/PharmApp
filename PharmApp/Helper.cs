using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmApp
{
    public class Helper
    {

        public static SQLiteConnection dbs;

        public static List<int> allCode = new List<int>();

        public static DataTable commandDbs(string command)
        {
            SQLiteCommand cmd = new SQLiteCommand(command, dbs);
            SQLiteDataReader output = cmd.ExecuteReader();
            var dataTable = new DataTable();
            dataTable.Load(output);
            return dataTable;
        }

        public static int CreateTable(string TableName,string SchemaTable)
        {
            SQLiteCommand cmd = new SQLiteCommand($"create table if not exists '{TableName}' ( {SchemaTable} )", dbs);
            return cmd.ExecuteNonQuery();
        }
        public static int Insert(string TableName, string SchemaTable)
        {
            SQLiteCommand cmd = new SQLiteCommand($"insert or ignore into '{TableName}' values ( {SchemaTable} )", dbs);
            return cmd.ExecuteNonQuery();
        }
        public static DataTable GetTable(string TableName,string column)
        {
            return commandDbs($"select {column} from \"{TableName}\"".Replace("'","\""));
        }
        public static int GetNewCode()
        {
            int tempnum = 1;
            while (true)
            {
                if (!allCode.Contains(tempnum))
                {
                    allCode.Add(tempnum);
                    return tempnum;
                }
                else
                {
                    tempnum++;
                }
            }
        }

    }
}
