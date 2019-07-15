using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StatueStoreWebApplic.Controllers
{
    public class ExcluirItemCarrinhoController : Controller
    {
        // GET: ExcluirItemCarrinho
        public ActionResult Index(int idProduto, string tamanho)
        {
            Models.MeuCarrinhoModel meuCarrinho = new Models.MeuCarrinhoModel();

            if (Session["idUsuario"] != null)
                meuCarrinho.DeleteProductById((int)Session["idUsuario"], idProduto, tamanho);

            else 
                meuCarrinho.DeleteProductByCookie(Request.Cookies["StatueStoreCookie"].Value, idProduto, tamanho);
            

            return RedirectToAction("Index", "MeuCarrinho");
        }
    }
}