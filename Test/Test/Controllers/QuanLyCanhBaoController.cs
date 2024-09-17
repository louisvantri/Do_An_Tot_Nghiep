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
    public class QuanLyCanhBaoController : Controller
    {
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
                data = new CanhBaoModel().getTramCanhBaos(),
                message = "ok"
            };
            return Json(dataResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(FormCollection form)
        {
            List<TramCanhBao> data = new CanhBaoModel().getTramCanhBaos();
            for(int i=0;i<data.Count; i++)
            {
                try
                {
                    data[i].MaxErrorRate = Convert.ToDouble(form["MaxErrorRate_" + data[i].Name], CultureInfo.InvariantCulture) /100;
                    if(data[i].MaxErrorRate<0) return Json(new { error = 1, msg = "Tỉ lệ lỗi không hợp lệ. Vui lòng kiểm tra lại" });
                }
                catch(Exception e)
                {
                    return Json(new { error = 1, msg = "Tỉ lệ lỗi không hợp lệ. Vui lòng kiểm tra lại" });
                }
            }
            string status = new CanhBaoModel().Update(data);
            if (status != "Success")
            {
                return Json(new { error = 1, msg = status });
            }  
            return Json(new { error = 0 });
        }

        
    }
}