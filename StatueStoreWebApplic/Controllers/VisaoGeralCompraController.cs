using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StatueStoreWebApplic.Models;

namespace StatueStoreWebApplic.Controllers {
    public class VisaoGeralCompraController : Controller {
        // GET: VisaoGeralCompra
        public ActionResult Index() {
            try {
                if (Session["TipoDePagamento"] == null || Session["idUsuario"] == null || Session["idEndereco"] == null) {
                    return RedirectToAction("Index", "Entrar");
                }

                VisaoGeral visaoGeral = new VisaoGeral();
                visaoGeral.CompraVisaoGeral();
                return View(visaoGeral);
            } 
            catch {
                return RedirectToAction("Index", "Ops");
            }
        }

        // FINALIZA A COMPRA 
        public ActionResult Ok() {
            try {
                Pedido pedido = new Pedido();
                CartaoDeCredito cartao = new CartaoDeCredito();


                Envio envio = new Envio();
                pedido.IdEnvio = envio.SetEnvio();

                pedido.IdTipoPagamento = (int)Session["TipoDePagamento"];

                if (pedido.IdTipoPagamento == 1) {
                    pedido.IdStatus = 1;
                    pedido.PagamentoAtivo = 0;
                }
                else {
                    if (Session["InfoCartao"] == null)
                        pedido.IdCartaoCredito = cartao.RetornaId();

                    else
                        pedido.IdCartaoCredito = 0;

                    pedido.IdStatus = 3;
                    pedido.PagamentoAtivo = 1;
                }
                pedido.IdCliente = (int)Session["idUsuario"];

                pedido.IdEnderecoCliente = pedido.retornaIdEnderecoCliente((int)Session["idEndereco"]);

                pedido.ConcluirPedido(); //Cadastra

                return RedirectToAction("Index", "CompraRealizada");
            } 
            
            catch {
                return RedirectToAction("Index", "Ops");
            }
        }
    }
}