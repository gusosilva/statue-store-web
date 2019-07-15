using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StatueStoreWebApplic.Models;
using System.Web.Mvc;

namespace StatueStoreWebApplic.Controllers
{
    public class MinhaContaController : Controller
    {
        // GET: MinhaConta
        public ActionResult Index()
        {
            if (Session["idUsuario"] == null) return RedirectToAction("Index", "Entrar");

            Cliente cliente = new Cliente();

            cliente.GetClientById((int)Session["idUsuario"]);

            return View(cliente);
        }
    }
}