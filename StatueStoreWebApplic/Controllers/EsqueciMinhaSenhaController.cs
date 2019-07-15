using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StatueStoreWebApplic.Models;

namespace StatueStoreWebApplic.Controllers
{
    public class EsqueciMinhaSenhaController : Controller
    {
        // GET: EsqueciMinhaSenha
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string email) {
            Login login = new Login();
            if (string.IsNullOrWhiteSpace(email)) {
                Response.Write("<script language=javascript> alert('Email inválido') </script>");
                return View();
            }

            string NomeUsuario = login.verificaEmail(email);

            if (string.IsNullOrWhiteSpace(NomeUsuario)) {
                Response.Write("<script language=javascript> alert('Email inválido') </script>");
                return View();
            }
           

            string chave = keyGen.GetRandomString(30);

            login.SetRestoreKeyOnUser(email, chave);

            statueEmailSender emailSender = new statueEmailSender();

            emailSender.Nome = NomeUsuario;
            emailSender.Assunto = "Redefinição de senha";
            emailSender.setBodyEsqueciSenha(chave);
            emailSender.AdicionarEmail(email);
            emailSender.Enviar();

            return RedirectToAction("Index", "Entrar");
        }
    }
}