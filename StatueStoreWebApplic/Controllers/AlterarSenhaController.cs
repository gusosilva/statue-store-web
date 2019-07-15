using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StatueStoreWebApplic.Models;

namespace StatueStoreWebApplic.Controllers
{
    public class AlterarSenhaController : Controller
    {
        // GET: AlterarSenha
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string senha1, string senha2, string senha3)
        {
            try
            {
                Login login = new Login();

                if (!login.verificaSenha(senha1.Trim()))
                {
                    Response.Write("<script language=javascript> alert('Senha atual incorreta') </script>");
                    return View();
                }

                if (!senha2.Equals(senha3))
                {
                    Response.Write("<script language=javascript> alert('Novas senhas não coincidem') </script>");
                    return View();
                }

                login.AlterarSenha(senha2);
                Response.Write("<script language=javascript> alert('Senha alterada com sucesso!') </script>");

                return View();
            }
            catch
            {
                return RedirectToAction("Index", "Ops");
            }
        }
    }
}