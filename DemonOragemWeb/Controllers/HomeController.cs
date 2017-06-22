using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemonOragemWeb.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult MyAction()
        {
            List<List<int>> retList = new List<List<int>>();
            List<int> ret = new List<int>();
            ret.Add(3);
            ret.Add(6);
            ret.Add(3);
            ret.Add(2);
            ret.Add(4);
            ret.Add(3);
            ret.Add(1);
            retList.Add(ret);
            ret = new List<int>();
            ret.Add(1);
            ret.Add(0);
            ret.Add(3);
            ret.Add(4);
            ret.Add(0);
            ret.Add(1);
            ret.Add(2);
            retList.Add(ret);
            ret = new List<int>();
            ret.Add(4);
            ret.Add(3);
            ret.Add(3);
            ret.Add(2);
            ret.Add(1);
            ret.Add(1);
            ret.Add(1);
            retList.Add(ret);

            return Json(retList);
        }

    }
}
