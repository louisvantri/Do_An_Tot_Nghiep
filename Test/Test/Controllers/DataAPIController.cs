using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test.Models;

namespace Test.Controllers
{
    public class DataAPIController : Controller
    {
        // GET: DataAPI
        public ActionResult getData(string query)
        {
            var dt = new DataService().GetData(query).Tables[0].Rows[0][0];
            return Json( dt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getRate(string Name)
        {
            try
            {
                int NumGet = Convert.ToInt32(new DataService().GetData("select numget from errorrate where name='" + Name + "'").Tables[0].Rows[0][0]);
                new DataService().ExecuteNonQuery("update errorrate set numget=numget+1 where name='" + Name + "'", new List<SQLiteParameter>());
                if (NumGet == 0 || NumGet % 100 == 0)
                {
                    if (Name == "KiemTra1")
                    {
                        var dt = new DataService().GetData("select MaxProcTime/ProcTime from ModelList where name='MayNung' ").Tables[0].Rows[0][0];
                        double rateMayNung = Convert.ToDouble(Convert.ToString(dt));
                        if (rateMayNung <= 0.5) rateMayNung = 1;
                        else
                        {
                            rateMayNung = 1 + (rateMayNung - 0.5) * 10;
                        }

                        dt = new DataService().GetData("select MaxProcTime/ProcTime from ModelList where name='EpKhuonNhua' ").Tables[0].Rows[0][0];
                        double rateEpKhuonNhua = Convert.ToDouble(Convert.ToString(dt));
                        if (rateEpKhuonNhua <= 0.5) rateEpKhuonNhua = 1;
                        else
                        {
                            rateEpKhuonNhua = 1 + (rateEpKhuonNhua - 0.5) * 10;
                        }

                        dt = new DataService().GetData("select MaxProcTime/ProcTime from ModelList where name='LamNguoi' ").Tables[0].Rows[0][0];
                        double rateLamNguoi = Convert.ToDouble(Convert.ToString(dt));
                        if (rateLamNguoi <= 0.5) rateLamNguoi = 1;
                        else
                        {
                            rateLamNguoi = 1 + (rateLamNguoi - 0.5) * 10;
                        }

                        return Json(rateMayNung+ rateEpKhuonNhua+rateLamNguoi, JsonRequestBehavior.AllowGet);
                    }
                    else if (Name == "KiemTra2")
                    {
                        var dt = new DataService().GetData("select MaxProcTime/ProcTime from ModelList where name='XuLyBeMat' ").Tables[0].Rows[0][0];
                        double rate = Convert.ToDouble(Convert.ToString(dt));
                        if (rate <= 0.5) rate = 1;
                        else
                        {
                            rate = 3 + (rate - 0.5) * 10;
                        }
                        return Json(rate, JsonRequestBehavior.AllowGet);
                    }
                    else if (Name == "KiemTra3")
                    {
                        var dt = new DataService().GetData("select MaxProcTime/ProcTime from ModelList where name='MaySon' ").Tables[0].Rows[0][0] ;
                        double rate = Convert.ToDouble(Convert.ToString(dt));
                        if (rate <= 0.5) rate = 1;
                        else
                        {
                            rate = 3 + (rate - 0.5) * 10;
                        }
                        return Json(rate, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult updateData(string query)
        {
            return Json(new DataService().ExecuteNonQuery(query, new List<SQLiteParameter>()), JsonRequestBehavior.AllowGet);
        }
    }
}