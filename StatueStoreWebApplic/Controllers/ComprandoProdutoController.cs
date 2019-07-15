using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StatueStoreWebApplic.Models;

namespace StatueStoreWebApplic.Controllers {
    public class ComprandoProdutoController : Controller {
        // GET: ComprandoProduto


        public ActionResult Index(int id, int qtd, string tamanho) {
            try { 
            MeuCarrinhoModel meuCarrinho = new MeuCarrinhoModel();
            CookiesPublicos cookies = new CookiesPublicos();

            if (tamanho == "Selecione")
                return Redirect("/ProdutoDetalhes?id=" + id + "&n=2");

            if (!meuCarrinho.QuantidadePorTamanho(id, tamanho, qtd))
                return Redirect("/ProdutoDetalhes?id=" + id + "&n=1");


            if (Session["idUsuario"] != null) {
                //cookies.RemoveCookie(); // Remove o cookie do navegador. Usuario logado, não é mais necessário o uso de cookies.
                meuCarrinho.SetProdutoByUserId((int)Session["idUsuario"], qtd, id, tamanho); //Cadastra o produto do usuario no banco do carrinho cliente.
            }
            else {
                String cookieValue = cookies.Cookie();
                meuCarrinho.SetProdutoByCookie(cookieValue, qtd, id, tamanho);
            }

            return RedirectToAction("Index", "MeuCarrinho");
            }
            catch
            {
                return RedirectToAction("Index", "Ops");
            }
        }
    }
}