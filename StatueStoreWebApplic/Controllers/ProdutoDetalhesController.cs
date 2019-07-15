using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StatueStoreWebApplic.Models;

namespace StatueStoreWebApplic.Controllers
{
    public class ProdutoDetalhesController : Controller
    {
        // GET: ProdutoDetalhes
        public ActionResult Index(int? id, int? n) {
            if(id == null || !CheckId(id)) 
                return RedirectToAction("Index", "Home");

            if (n == 1)
                Response.Write("<script language=javascript> alert('A quantidade excede a disponível nesse tamanho')</script>");
            if(n == 2)
                Response.Write("<script language=javascript> alert('Selecione corretamente')</script>");

            DetalheProduto detProduto = new DetalheProduto();
            detProduto.GetById(id);

            return View(detProduto);
        }

        //Verifica se o parametro id é um numero
        private bool CheckId(int? id) {

            try {
                Convert.ToInt32(id);
                return true;
            } 
            
            catch {
                return false;
            }
        }
    }
}