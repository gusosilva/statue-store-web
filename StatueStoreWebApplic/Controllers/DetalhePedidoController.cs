using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StatueStoreWebApplic.Models;

namespace StatueStoreWebApplic.Controllers
{
    public class DetalhePedidoController : Controller
    {
        // GET: DetalhePedido
        public ActionResult Index(int? id)
        {
            try {  

            if (Session["idUsuario"] == null) return RedirectToAction("Index", "Entrar");
            if (id == null || !(id is int)) return RedirectToAction("Index", "MeusPedidos");

            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.CommandText = "SELECT * FROM PEDIDO WHERE idPedido = @IDPEDIDO AND IDCLIENTE = @IDCLIENTE";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@IDPEDIDO", System.Data.SqlDbType.Int).Value = id;
            conexao.command.Parameters.Add("@IDCLIENTE", System.Data.SqlDbType.Int).Value = (int)Session["idUsuario"];
            
            VisaoGeral visao = new VisaoGeral();

            visao.DetalhePedido((int)id);

            return View(visao);
            }
            catch
            {
                return RedirectToAction("Index", "Ops");
            }
        }
    }
}