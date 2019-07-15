using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StatueStoreWebApplic.Models;

namespace StatueStoreWebApplic.Controllers
{
    public class CartaoCreditoController : Controller
    {
        // GET: CartaoCredito
        public ActionResult Index()
        {
            try
            {
                if (Session["idUsuario"] == null) return RedirectToAction("Index", "Home");
                if (Session["TipoDePagamento"] == null) return RedirectToAction("index", "Home");

                CartaoDeCredito cartao = new CartaoDeCredito();
                if (!cartao.Verifica())
                    return RedirectToAction("Index", "CadastroCartao");

                ViewBag.Digitos = cartao.UltimosDigitos();
                return View();
            }
            catch
            {
                return RedirectToAction("Index", "Ops");
            }
        }

        public ActionResult NovoCadastro()
        {
            return RedirectToAction("Index", "CadastroCartao");
        }


        public ActionResult Prosseguir()
        {
            CartaoDeCredito cartao = new CartaoDeCredito();
            Session["idCartaoCredito"] = cartao.RetornaId();

            return RedirectToAction("Index", "VisaoGeralCompra");
        }
    }
}