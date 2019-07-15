using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StatueStoreWebApplic.Models;

namespace StatueStoreWebApplic.Controllers
{
    public class Produtos_MasculinoController : Controller
    {
        // GET: Produtos_Masculino
        public ActionResult Index(string g, string filtro, string buscar, string tamanho) {

            CookiesPublicos cookie = new CookiesPublicos();
            ModelProdutos modelProdutos = new ModelProdutos('M', g, filtro, tamanho, buscar);

            return View(modelProdutos);
        }

    }
}

/*
 * 
 *  CARACATER ESPECIAL 
 *  TERMO PARA ASSINAR
 *  PERIODICIDADE PARA TROCA DE SENHA
 */