using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace Test.Models
{
    public class WaittingTime
    {
        public string Name { get; set; }
        public string NameVNM { get; set; }
        public double SumWaittingTime { get; set; }
        public double ExitTime { get; set; }
        public double WaitTime { get; set; }
        public double ProcessTime { get; set; }
        public double SumProcessTime { get; set; }
    }

    public class mWaittingTime
    {
        public string query = "select *, (select d.NameVNM from ModelList d where d.Name=WaittingTime.Name) NameVNM from WaittingTime";

        public WaittingTime getItem(string Name)
        {
            DataSet dt = new DataService().GetList(query + " where Name='" + Name + "'");

            WaittingTime waittingTime = new WaittingTime();
            try
            {
                foreach (DataRow dtRow in dt.Tables[0].Rows)
                {
                    waittingTime = new WaittingTime
                    {
                        Name = Convert.ToString(dtRow["Name"]),
                        NameVNM = Convert.ToString(dtRow["NameVNM"]),
                        SumWaittingTime= Convert.ToDouble(dtRow["SumWaittingTime"]),
                        ExitTime= Convert.ToDouble(dtRow["ExitTime"]),
                        WaitTime= Convert.ToDouble(dtRow["WaitTime"]),
                        ProcessTime= Convert.ToDouble(dtRow["ProcessTime"]),
                        SumProcessTime = Convert.ToDouble(dtRow["SumProcessTime"])
                    };
                }
            }
            catch (Exception e) { }

            return waittingTime;
        }
        public List<WaittingTime> getList()
        {
            DataSet dt = new DataService().GetList(query);

            List<WaittingTime> waittingTimes = new List<WaittingTime>();
            try
            {
                foreach (DataRow dtRow in dt.Tables[0].Rows)
                {
                    waittingTimes.Add(new WaittingTime
                    {
                        Name = Convert.ToString(dtRow["Name"]),
                        NameVNM = Convert.ToString(dtRow["NameVNM"]),
                        SumWaittingTime = Convert.ToDouble(dtRow["SumWaittingTime"]),
                        ExitTime = Convert.ToDouble(dtRow["ExitTime"]),
                        WaitTime = Convert.ToDouble(dtRow["WaitTime"]),
                        ProcessTime = Convert.ToDouble(dtRow["ProcessTime"]),
                        SumProcessTime = Convert.ToDouble(dtRow["SumProcessTime"])
                    });
                }
            }
            catch (Exception e) { }

            return waittingTimes;

        }
    }
}