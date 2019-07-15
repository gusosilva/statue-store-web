using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StatueStoreWebApplic.Models;

namespace StatueStoreWebApplic.Controllers
{
    public class FazendoPedidoController : Controller
    { 
        // GET: FazendoPedido
        public ActionResult Index()
        {
            if (Session["idUsuario"] == null) return RedirectToAction("Index", "Entrar");
            //
            return View(Pedido.TiposDePgmto());
        }

        [HttpPost]
        public ActionResult Index(string tipopgmto) 
        {
            if (string.IsNullOrWhiteSpace(tipopgmto)) {
                Response.Write("<script language=javascript> alert('Selecione corretamente o tipo de pagamento') </script>");
                return View();
            }

            int idTipoDePagamento = verificaBanco(tipopgmto); //Consulta no banco para pegar o id do tipo de pagamento (parametro)

            if(idTipoDePagamento == 0) {
                Response.Write("<script language=javascript> alert('Tipo de pagamento não encontrado. Contate-nos a cerca desse problema !') </script>");
                return View();
            }

            Session["TipoDePagamento"] = idTipoDePagamento; //REGISTRA NA SESSION
            Session["NomeTipoDePagamento"] = tipopgmto;

            if (idTipoDePagamento == 2)
                return RedirectToAction("Index", "CartaoCredito");



            return RedirectToAction("Index", "Enderecos");
        }


        private int verificaBanco(string nome) {
            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.CommandText = "SELECT IDTIPOPGTO FROM TipoPgto WHERE nomeTipo = @TIPO";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@TIPO", System.Data.SqlDbType.VarChar).Value = nome;
            var resposta = conexao.command.ExecuteScalar();

            if(resposta == null) {
                conexao.connection.Close();
                return 0;
            }
            else {

                try { Convert.ToInt32(resposta); }
                catch {
                    conexao.connection.Close();
                    return 0;
                }

                if ((int)resposta == 0) {
                    conexao.connection.Close();
                    return 0;
                }

                conexao.connection.Close();
                return (int)resposta;
            }
        }
    }
}