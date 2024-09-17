using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test.Models;
using OfficeOpenXml;
using System.Globalization;

namespace Test.Controllers
{
    public class QuanLyThongSoController : Controller
    {
        // GET: QuanLyThongSo
        public ActionResult Index(int? ReLoad)
        {
            if(ReLoad.HasValue && ReLoad.Value==1)
            {
                ViewData["NoLayout"] = 1;
            }
            return View();
        }
        
        public ActionResult GetListModel()
        {

            var dataResult = new
            {
                data = new mTram().getList(),
                message = "ok"
            };
            return Json(dataResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(FormCollection form)
        {
            List<Tram> trams = new mTram().getList();

            for(int i=0;i<trams.Count;i++)
            {
                try
                {
                    trams[i].ProcTime = Convert.ToDouble(Convert.ToString(form["ProcTime_" + trams[i].Name]), CultureInfo.InvariantCulture);
                    if(trams[i].MaxProcTime > trams[i].ProcTime)
                    {
                        return Json(new { error = 1, msg = "Thông số của máy " + trams[i].NameVNM + " không hợp lệ. Vui lòng kiểm tra lại" });
                    }
                }
                catch(Exception e)
                {
                    return Json(new { error = 1, msg = "Thông số của máy " + trams[i].NameVNM + " không hợp lệ. Vui lòng kiểm tra lại" });
                }
                trams[i].NameVNM = Convert.ToString(form["NameVNM_" + trams[i].Name]);
            }
            string status = new mTram().Update(trams);
            if (status != "Success")
            {
                return Json(new { error = 1, msg = status });
            }
            new DataService().ExecuteNonQuery("update errorrate set numget=99", new List<SQLiteParameter>());
            return Json(new { error = 0 });

        }




        public ActionResult CheckLoi()
        {
            Tram KiemTra1 = new mTram().getItem("KiemTra1");
            Tram KiemTra2 = new mTram().getItem("KiemTra2");
            Tram KiemTra3 = new mTram().getItem("KiemTra3");

            DataRow dtRow = new DataService().GetData("select * from checkloi").Tables[0].Rows[0];
            int Failed = Convert.ToInt32(dtRow["Failed"]);
            int NumberProcessed = Convert.ToInt32(dtRow["NumberProcessed"]);
            int Failed1 = Convert.ToInt32(dtRow["Failed1"]);
            int NumberProcessed1 = Convert.ToInt32(dtRow["NumberProcessed1"]);
            int Failed2 = Convert.ToInt32(dtRow["Failed2"]);
            int NumberProcessed2 = Convert.ToInt32(dtRow["NumberProcessed2"]);
            int Failed3 = Convert.ToInt32(dtRow["Failed3"]);
            int NumberProcessed3 = Convert.ToInt32(dtRow["NumberProcessed3"]);

            //Fail check
            int check = 0;
            string msg = "";
            if (KiemTra1.NumberProcessed >= NumberProcessed + 30)
            {
                if ((1.0 * (KiemTra1.Failed - Failed1)) / (1.0 * (KiemTra1.NumberProcessed - NumberProcessed1)) > KiemTra1.MaxErrorRate)
                {
                    msg += "- Số sản phẩm lỗi của công đoạn 1 nhiều (" + Math.Round((1.0 * (KiemTra1.Failed - Failed1)) / (1.0 * (KiemTra1.NumberProcessed - NumberProcessed1)) * 100, 2) + "%)<br/>";


                    check += 1;
                    new DataService().ExecuteNonQuery("update checkloi set Failed1=@Failed1, NumberProcessed1=@NumberProcessed1", new List<SQLiteParameter>() {
                        new SQLiteParameter("Failed1", KiemTra1.Failed),
                        new SQLiteParameter("NumberProcessed1", KiemTra1.NumberProcessed)
                    });
                }
                if ((1.0 * (KiemTra2.Failed - Failed2)) / (1.0 * (KiemTra2.NumberProcessed - NumberProcessed2)) > KiemTra2.MaxErrorRate)
                {
                    msg += "- Số sản phẩm lỗi của công đoạn 2 nhiều (" + Math.Round((1.0 * (KiemTra2.Failed - Failed2)) / (1.0 * (KiemTra2.NumberProcessed - NumberProcessed2)) * 100, 2) + "%)<br/>";

                    check += 1;
                    new DataService().ExecuteNonQuery("update checkloi set Failed2=@Failed2, NumberProcessed2=@NumberProcessed2", new List<SQLiteParameter>() {
                        new SQLiteParameter("Failed2", KiemTra2.Failed),
                        new SQLiteParameter("NumberProcessed2", KiemTra2.NumberProcessed)
                    });
                }
                if ((1.0 * (KiemTra3.Failed - Failed3)) / (1.0 * (KiemTra3.NumberProcessed - NumberProcessed3)) > KiemTra3.MaxErrorRate)
                {
                    msg += "- Số sản phẩm lỗi của công đoạn 3 nhiều (" + Math.Round((1.0 * (KiemTra3.Failed - Failed3)) / (1.0 * (KiemTra3.NumberProcessed - NumberProcessed3)) * 100, 2) + "%)<br/>";

                    check += 1;
                    new DataService().ExecuteNonQuery("update checkloi set Failed3=@Failed3, NumberProcessed3=@NumberProcessed3", new List<SQLiteParameter>() {
                        new SQLiteParameter("Failed3", KiemTra3.Failed),
                        new SQLiteParameter("NumberProcessed3", KiemTra3.NumberProcessed)
                    });
                }
            }
            //Time check

            //List<WaittingTime> waittingTimes = new mWaittingTime().getList();
            //if (waittingTimes[0].SumProcessTime-waittingTimes[0].ProcessTime >= 1800)
            //{
            //    for (int i = 0; i < waittingTimes.Count; i++)
            //    {
            //        if ((waittingTimes[i].SumWaittingTime - waittingTimes[i].WaitTime) / (waittingTimes[i].SumProcessTime - waittingTimes[i].ProcessTime) > 0.05)
            //        {
            //            msg += "- Thời gian chờ của máy '" + waittingTimes[i].NameVNM + "' nhiều (" + Math.Round(100 * (waittingTimes[i].SumWaittingTime - waittingTimes[i].WaitTime) / (waittingTimes[i].SumProcessTime - waittingTimes[i].ProcessTime), 2) + "%)<br/>";
            //            new DataService().ExecuteNonQuery("update WaittingTime set WaitTime=@SumWaittingTime, ProcessTime=@SumProcessTime where name=@Name", new List<SQLiteParameter>()
            //            {
            //                new SQLiteParameter("SumWaittingTime", waittingTimes[i].SumWaittingTime),
            //                new SQLiteParameter("SumProcessTime", waittingTimes[i].SumProcessTime),
            //                new SQLiteParameter("Name", waittingTimes[i].Name)
            //            });
            //        }
            //    }
            //}

            //End check
            if (msg == "")
            {
                msg = "Ok";
            }
            return Json(new { msg = msg }, JsonRequestBehavior.AllowGet);

        }
        

        [HttpGet]
        public ActionResult GetExcel()
        {
            try
            {               
                List<Tram> trams = new mTram().getList();
                List<WaittingTime> waittingTimes = new mWaittingTime().getList();

                //Excel
                var stream = System.IO.File.OpenRead(Server.MapPath("~/DataBase/ThongKeDuLieu.xlsx"));
                using (var package = new ExcelPackage())
                {
                    package.Load(stream);

                    //Step 2 :
                    var ws = package.Workbook.Worksheets["ThongKeDuLieu"];
                    ws.Name = "ThongKeDuLieu";

                    ws.Cells["A1"].Value = "Thời gian xuất thống kê: " + Convert.ToString(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                    string ExcelColumnName = "EFGHIJKLM";
                    int TongLoi = 0;
                    double SumWaittingTime = 0;
                    double MaxWaittingTime = 0;
                    for (int i = 0; i < trams.Count; i++)
                    {
                        ws.Cells[ExcelColumnName[i].ToString() + "5"].Value = trams[i].NameVNM;
                        ws.Cells[ExcelColumnName[i].ToString() + "6"].Value = trams[i].ProcTime;
                        ws.Cells[ExcelColumnName[i].ToString() + "7"].Value = trams[i].NumberProcessed;
                        if (trams[i].IncludeFailed == 1)
                        {
                            ws.Cells[ExcelColumnName[i].ToString() + "8"].Value = trams[i].Failed;
                            TongLoi += trams[i].Failed;
                        }
                        ws.Cells[ExcelColumnName[i].ToString() + "9"].Value = waittingTimes[i].SumProcessTime;
                        ws.Cells[ExcelColumnName[i].ToString() + "10"].Value = Math.Round(waittingTimes[i].SumWaittingTime,2);
                        SumWaittingTime += waittingTimes[i].SumWaittingTime;
                        if (waittingTimes[i].SumWaittingTime > MaxWaittingTime) MaxWaittingTime = waittingTimes[i].SumWaittingTime;
                    }

                    double SumProcessTime = waittingTimes[0].SumWaittingTime + waittingTimes[0].SumProcessTime;
                    ws.Cells["N6"].Value = Math.Round( SumProcessTime/ trams[0].NumberProcessed, 2);
                    ws.Cells["N7"].Value = trams[0].NumberProcessed;
                    ws.Cells["N8"].Value = TongLoi;
                    ws.Cells["N9"].Value = Math.Round(SumProcessTime,2);
                    ws.Cells["N10"].Value = Math.Round(MaxWaittingTime, 2);

                    ws.Cells["E5:N10"].AutoFitColumns();
                    //Step 4 : Save

                    stream.Close();

                    var arr = package.GetAsByteArray();
                    var filename = $"attachment; filename=ThongKeDuLieu-{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";
                    Response.AddHeader("content-disposition", filename);
                    return File(arr, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                }
            }
            catch (Exception ex)
            {
                return Content("Try later. " + ex.Message);
            }
        }
    }
}