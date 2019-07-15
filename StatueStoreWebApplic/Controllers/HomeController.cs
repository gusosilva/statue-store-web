using StatueStoreWebApplic.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StatueStoreWebApplic.Controllers {
    public class HomeController : Controller {
        // GET: Home
        public ActionResult Index() {
            //if(Session["idUsuario"] != null) {
            //    Response.Write("<script language=javascript> alert('logado como id: " + Session["idUsuario"].ToString() + "'); </script>");
            //}

            ProdutosEAnuncios coisas = new ProdutosEAnuncios(4, 5);

            return View(coisas);
        }
    }
}