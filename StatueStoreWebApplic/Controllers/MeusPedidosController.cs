using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StatueStoreWebApplic.Models;

namespace StatueStoreWebApplic.Controllers
{
    public class MeusPedidosController : Controller
    {
        // GET: MeusPedidos
        public ActionResult Index()
        {
            AcompanharPedidos acompanhar = new AcompanharPedidos();

            List<AcompanharPedidos> pedidos = acompanhar.PedidosById();

            return View(pedidos);
        }
    }
}