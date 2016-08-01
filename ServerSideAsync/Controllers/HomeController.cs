using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ServerSideAsync.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public async Task<JsonResult> GetDataAsync()
        {
            //await Task.Run(() => Thread.Sleep(5000));
            await Task.Delay(5000);
            return Json(new { message = "1", data = "hello" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetData()
        {
            Thread.Sleep(100);
            return Json(new { message = "1", data = "hello" }, JsonRequestBehavior.AllowGet);
        }
    }
}