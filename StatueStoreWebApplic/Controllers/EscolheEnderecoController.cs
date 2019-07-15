using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StatueStoreWebApplic.Controllers
{
    public class EscolheEnderecoController : Controller
    {
        // GET: EscolheEndereco
        public ActionResult Index(int idec)
        {
            if (Session["idUsuario"] == null || Session["TipoDePagamento"] == null) return RedirectToAction("Index", "Home");

            Session["idEndereco"] = idec;
            return RedirectToAction("Index", "VisaoGeralCompra");
        }

        public ActionResult Excluir(int idec)
        {
            if (Session["idUsuario"] == null || Session["TipoDePagamento"] == null) return RedirectToAction("Index", "Home");

            Models.Endereco endereco = new Models.Endereco();

            endereco.DeletaEndereco(idec); 

            return RedirectToAction("Index", "Enderecos");
        }
    }
}