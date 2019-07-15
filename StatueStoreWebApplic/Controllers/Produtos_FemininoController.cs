using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StatueStoreWebApplic.Models;

namespace StatueStoreWebApplic.Controllers
{
    public class Produtos_FemininoController : Controller
    {
        // GET: Produtos_Feminino
        public ActionResult Index(string g, string filtro, string buscar, string tamanho)
        {

            ModelProdutos modelProdutos = new ModelProdutos('F', g, filtro, tamanho, buscar);
      
            return View(modelProdutos);
        }
    }
}