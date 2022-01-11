using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RigMaterialMovementWeb.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ErrorValidate()
        {
            return View();
        }
        public ActionResult ErrorAccess()
        {
            return View();
        }
    }
}