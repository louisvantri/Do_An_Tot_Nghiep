using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Test.Models
{
    public class Tram
    {
        public string Name { get; set; } = "";
        public string NameVNM { get; set; }
        public double ProcTime { get; set; } = -1;
        public int IncludeProcTime { get; set; }
        public double MaxProcTime { get; set; } = -1;
        public int IncludeNumberProcessed { get; set; }
        public int NumberProcessed { get; set; }
        public int IncludeFailed { get; set; }
        public int Failed { get; set; }
        public double MaxErrorRate { get; set; }
    }

    public class mTram
    {
        public string query = "select * from ModelList";

        public Tram getItem(string Name)
        {
            DataSet dt = new DataService().GetList(query +" where Name='"+Name+"'");

            Tram tram = new Tram();
            try
            {
                foreach (DataRow dtRow in dt.Tables[0].Rows)
                {
                    tram = new Tram
                    {
                        Name = Convert.ToString(dtRow["Name"]),
                        NameVNM = Convert.ToString(dtRow["NameVNM"]),
                        ProcTime = Convert.ToDouble(dtRow["ProcTime"]),
                        MaxProcTime = Convert.ToDouble(dtRow["MaxProcTime"]),
                        IncludeProcTime = Convert.ToInt32(dtRow["IncludeProcTime"]),
                        NumberProcessed = Convert.ToInt32(dtRow["NumberProcessed"]),
                        IncludeNumberProcessed = Convert.ToInt32(dtRow["IncludeNumberProcessed"]),
                        Failed = Convert.ToInt32(dtRow["Failed"]),
                        IncludeFailed = Convert.ToInt32(dtRow["IncludeFailed"]),
                        MaxErrorRate = Convert.ToDouble(dtRow["MaxErrorRate"]),
                    };
                }
            }
            catch (Exception e) { }

            return tram;
        }
        public List<Tram> getList()
        {
            DataSet dt = new DataService().GetList(query);

            List<Tram> trams = new List<Tram>();
            try
            {
                foreach (DataRow dtRow in dt.Tables[0].Rows)
                {
                    trams.Add(new Tram
                    {
                        Name = Convert.ToString(dtRow["Name"]),
                        NameVNM = Convert.ToString(dtRow["NameVNM"]),
                        ProcTime = Convert.ToDouble(dtRow["ProcTime"]),
                        MaxProcTime = Convert.ToDouble(dtRow["MaxProcTime"]),
                        IncludeProcTime = Convert.ToInt32(dtRow["IncludeProcTime"]),
                        NumberProcessed = Convert.ToInt32(dtRow["NumberProcessed"]),
                        IncludeNumberProcessed = Convert.ToInt32(dtRow["IncludeNumberProcessed"]),
                        Failed = Convert.ToInt32(dtRow["Failed"]),
                        IncludeFailed = Convert.ToInt32(dtRow["IncludeFailed"]),
                    });
                }
            }
            catch (Exception e) { }
            return trams;
        }

        public string Update(List<Tram> trams)
        {
            string queryUpdate = "";

            List<SQLiteParameter> sQLiteParameters = new List<SQLiteParameter>();
            for (int i = 0; i < trams.Count; i++)
            {
                queryUpdate += "update modellist set NameVNM=@NameVNM" + i + ", ProcTime=@ProcTime" + i + " where Name=@Name" + i+";";
                sQLiteParameters.Add(new SQLiteParameter("Name"+ i , trams[i].Name));
                sQLiteParameters.Add(new SQLiteParameter("ProcTime"+i, trams[i].ProcTime));
                sQLiteParameters.Add(new SQLiteParameter("NameVNM"+i, trams[i].NameVNM));
            }
            return new DataService().ExecuteNonQuery(queryUpdate, sQLiteParameters);
        }
    }
}