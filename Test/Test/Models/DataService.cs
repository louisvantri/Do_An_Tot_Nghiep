using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace Test.Models
{
    public class DataService
    {
        private string connectionString = "Data Source="+ HttpContext.Current.Server.MapPath("~/DataBase/DataBase.db") + ";PRAGMA journal_mode=WAL;";
        private SQLiteConnection con;
        private SQLiteCommand cmd;
        private SQLiteDataAdapter sda;
        private DataSet dt;

        public DataSet GetList(string query)
        {
            con = new SQLiteConnection(connectionString);
            con.Open();
            try
            {
                cmd = new SQLiteCommand(query, con);
                sda = new SQLiteDataAdapter(cmd);
                dt = new DataSet();
                sda.Fill(dt);

            }
            catch (Exception e) { }

            con.Close();
            return dt;
        }

        public string ExecuteNonQuery(string query, List<SQLiteParameter> sqlParameter)
        {
            try
            {
                con = new SQLiteConnection(connectionString);
                con.Open();
                cmd = new SQLiteCommand(query, con);
                cmd.Parameters.AddRange(sqlParameter.ToArray());

                cmd.ExecuteNonQuery();

                con.Close();

                return "Success";
            }
            catch(Exception e)
            {
                con.Close();
                return e.Message;
            }
        }
        public DataSet GetData(string query)
        {
            con = new SQLiteConnection(connectionString);
            dt = new DataSet();
            con.Open();
            try
            {
                cmd = new SQLiteCommand(query, con);
                sda = new SQLiteDataAdapter(cmd);
                sda.Fill(dt);

                con.Close();

                return dt;
            }
            catch (Exception e)
            {
                con.Close();
                return dt;
            }
        }
    }
}