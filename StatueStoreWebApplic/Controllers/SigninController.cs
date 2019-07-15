using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StatueStoreWebApplic.Models;

namespace StatueStoreWebApplic.Controllers
{
    public class SigninController : Controller
    {
        // GET: Signin
        public ActionResult Index()
        {
            if (Session["idUsuario"] != null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Index(Cliente cliente)
        {
            cliente.BdSetClient();
            return RedirectToAction("Index", "Entrar");
        }
    }
}