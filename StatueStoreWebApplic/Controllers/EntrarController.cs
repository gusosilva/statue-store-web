using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StatueStoreWebApplic.Models;

namespace StatueStoreWebApplic.Controllers
{
    public class EntrarController : Controller
    {
        // GET: Entrar
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Login usuario) 
        {

            if(string.IsNullOrWhiteSpace(usuario.Email) || string.IsNullOrWhiteSpace(usuario.Senha)) {
                Response.Write("<script language=javascript> alert('Preencha os campos corretamente') </script>");
                return View();
            }

            if (!usuario.DoIt()) { 
                Response.Write("<script language=javascript> alert('Usuario e/ou senha incorretos') </script>");
                return View();
            }


            HttpCookie cookie = Request.Cookies["StatueStoreCookie"];

            if (cookie != null) usuario.CarCookietoUser(cookie.Value);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout() {
            if (Session["idUsuario"] != null)
                Session["idUsuario"] = null;

            return RedirectToAction("Index", "Entrar");
        }
    }
}