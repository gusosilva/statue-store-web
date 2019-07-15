using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StatueStoreWebApplic.Models;

namespace StatueStoreWebApplic.Controllers {
    public class RedefinindoSenhaController : Controller {
        // GET: RedefinindoSenha

        public ActionResult Index(string codigo) {
            if (string.IsNullOrWhiteSpace(codigo))
                return RedirectToAction("Index", "Entrar");

            Login login = new Login();


            if(!login.VerificarCodigo(codigo))
                return RedirectToAction("Index", "Entrar");


            Session["RestoreCodigo"] = codigo;
            return View();
        }

        [HttpPost]
        public ActionResult Index(string senha1, string senha2) {
            if (string.IsNullOrWhiteSpace(senha1) || string.IsNullOrWhiteSpace(senha2)) {
                Response.Write("<script language=javascript> alert('As senhas devem coincidir') </script>");
                return View();
            }

            if (!senha1.Equals(senha2)) {
                Response.Write("<script language=javascript> alert('As senhas devem coincidir') </script>");
                return View();
            }

            Login login = new Login();

            login.RedefinirSenhaPorkey((string)Session["RestoreCodigo"], senha2);

            return RedirectToAction("Index", "Entrar");
        }
    }
}