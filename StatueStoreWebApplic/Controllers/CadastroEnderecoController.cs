using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StatueStoreWebApplic.Models;

namespace StatueStoreWebApplic.Controllers
{
    public class CadastroEnderecoController : Controller
    {
        // GET: CadastroEndereco
        public ActionResult Index()
        {
            if (Session["idUsuario"] == null || Session["TipoDePagamento"] == null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public ActionResult Index(Endereco endereco)
        {
            try
            {
                endereco.Pais = "Brasil";

                try { Convert.ToInt32(endereco.Numero); }
                catch
                {
                    Response.Write("<script language=javascript> alert('Preencha o campo numero corretamente') </script>");
                    return View();
                }

                endereco.CadastraBanco();
                return RedirectToAction("Index", "Enderecos");
            }
            catch
            {
                return RedirectToAction("Index", "Ops");
            }
        }
    }

}