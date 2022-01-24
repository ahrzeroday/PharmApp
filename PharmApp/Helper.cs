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
        public static List<int> alllistCode = new List<int>();
        public static List<int> alllistsCode = new List<int>();
        public static List<int> alllistCustomer = new List<int>();
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
        public static DataTable Search(string TableName, string column, string where)
        {
            return commandDbs($"select  {column} from '{TableName}' where {where}".Replace("'", "\""));
        }
        public static DataTable Multiply(string TableName1 ,string TableName2 ,string column ,string where)
        {
            string test = $"select {column} from '{TableName1}'as a,'{TableName2}'as b where {where}".Replace("'", "\"");
            return commandDbs($"select {column} from '{TableName1}'as a,'{TableName2}'as b where {where}".Replace("'", "\""));
        }
        public static DataTable Multiply_dis(string TableName1 ,string TableName2 ,string column ,string where)
        {
            return commandDbs($"select distinct {column} from '{TableName1}'as a,'{TableName2}'as b where {where}".Replace("'", "\""));
        }

        public static DataTable Triply(string TableName1, string TableName2, string tableName3,string column , string where)
        {
            return commandDbs($"select {column}from '{TableName1}'as a ,'{TableName2}'as b , '{tableName3}' as c where {where}".Replace("'","\""));
        }
        public static int Update(string TableName,string set , string where)
        {
            string test = $"update '{TableName}' set {set} where {where}".Replace("'", "\"");
            SQLiteCommand cmd = new SQLiteCommand($"update '{TableName}' set {set} where {where}".Replace("'", "\""), dbs);
            return cmd.ExecuteNonQuery();;
        }

        public static DataTable DeleteT (string TableName ,  string where)
        {
            return commandDbs($"delete from '{TableName}' where {where}".Replace("'","\""));
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
        public static int GetNewlistNum()
        {
            int tempnum = 1;
            while (true)
            {
                if (!alllistCode.Contains(tempnum))
                {
                    alllistCode.Add(tempnum);
                    return tempnum;
                }
                else
                {
                    tempnum++;
                }
            }
        }
        public static int GetNewsCode()
        {
            int tempnum = 1;
            while (true)
            {
                if (!alllistsCode.Contains(tempnum))
                {
                    alllistsCode.Add(tempnum);
                    return tempnum;
                }
                else
                {
                    tempnum++;
                }
            }
        }
        public static int GetNewsCustomer()
        {
            int tempnum = 1;
            while (true)
            {
                if (!alllistCustomer.Contains(tempnum))
                {
                    alllistCustomer.Add(tempnum);
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
