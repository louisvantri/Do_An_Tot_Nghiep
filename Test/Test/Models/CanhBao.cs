using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace Test.Models
{
    public class TramCanhBao
    {
        public string Name { get; set; } = "";
        public string NameVNM { get; set; }

        public double MaxErrorRate { get; set; }
    }

    public class CanhBaoModel
    {
        public List<TramCanhBao> getTramCanhBaos()
        {

            string queryGetTramCanhBaos = "select * from ModelList where IncludeFailed=1";
            DataSet dt = new DataService().GetList(queryGetTramCanhBaos);

            List<TramCanhBao> trams = new List<TramCanhBao>();
            try
            {
                foreach (DataRow dtRow in dt.Tables[0].Rows)
                {
                    trams.Add(new TramCanhBao
                    {
                        Name = Convert.ToString(dtRow["Name"]),
                        NameVNM = Convert.ToString(dtRow["NameVNM"]),
                        MaxErrorRate = Convert.ToDouble(dtRow["MaxErrorRate"])*100
                    });
                }
            }
            catch (Exception e) { }
            return trams;
        }

        public string Update(List<TramCanhBao> tramCanhBaos)
        {
            string queryUpdate = "";
            List<SQLiteParameter> sQLiteParameters = new List<SQLiteParameter>();
            for(int i=0;i<tramCanhBaos.Count;i++)
            {
                queryUpdate += "update modellist set MaxErrorRate=@MaxErrorRate"+i+ " where Name=@Name" + i + "; ";
                sQLiteParameters.Add(new SQLiteParameter("MaxErrorRate" + i , tramCanhBaos[i].MaxErrorRate));
                sQLiteParameters.Add(new SQLiteParameter("Name" + i, tramCanhBaos[i].Name));
            }
            string status = new DataService().ExecuteNonQuery(queryUpdate, sQLiteParameters);
            return status;
        }
    }
}