using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StatueStoreWebApplic.Models;

namespace StatueStoreWebApplic.Controllers
{
    public class CadastroCartaoController : Controller
    {
        // GET: CadastroCartao
        public ActionResult Index()
        {
            if (Session["idUsuario"] == null || Session["TipoDePagamento"] == null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public ActionResult Index(CartaoDeCredito cartao)
        {
            try
            {
                List<string> bandeiras = Enum.GetValues(typeof(Bandeira)).Cast<Bandeira>().Select(v => v.ToString()).ToList();

                if (!bandeiras.Contains(cartao.Bandeira.ToString()))
                {
                    Response.Write("<script language=javascript> alert('Selecione uma bandeira valida') </script>");
                    return View();
                }


                if (Convert.ToInt32(cartao.Validade.Substring(3, 4)) < DateTime.Today.Year)
                {
                    Response.Write("<script language=javascript> alert('Validade Incorreta') </script>");
                    return View();
                }

                if (Convert.ToInt32(cartao.Validade.Substring(3, 4)) == DateTime.Today.Year)
                {
                    if (Convert.ToInt32(cartao.Validade.Substring(0, 2)) < DateTime.Today.Month)
                    {
                        Response.Write("<script language=javascript> alert('Validade Incorreta') </script>");
                        return View();
                    }
                }


                cartao.IdCliente = (int)Session["idUsuario"];


                if (cartao.SalvarInfo)
                {
                    cartao.CadastraCartao();
                    Session["InfoCartao"] = null;
                    return RedirectToAction("Index", "CartaoCredito");
                }

                Session["InfoCartao"] = cartao;
                return RedirectToAction("Index", "Enderecos");
            }
            catch
            {
                return RedirectToAction("Index", "Ops");
            }
        }
    }
}