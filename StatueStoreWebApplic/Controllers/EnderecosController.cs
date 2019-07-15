using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StatueStoreWebApplic.Models;

namespace StatueStoreWebApplic.Controllers
{
    public class EnderecosController : Controller
    {
        // GET: Enderecos
        public ActionResult Index()
        {
            try { 
            if (Session["idUsuario"] == null || Session["TipoDePagamento"] == null)
                return RedirectToAction("Index", "Entrar");

            Endereco end = new Endereco();

            if (!end.TemEndereco())
                return RedirectToAction("Index", "CadastroEndereco");

            return View(end.CepDosClientes());
            }
            catch
            {
                return RedirectToAction("Index", "Ops");
            }
        }
    }
}