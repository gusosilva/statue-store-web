using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace StatueStoreWebApplic.Models {
    public class AcompanharPedidos {

        public int IdPedido { get; set; }
        public string TipoDePagamento { get; set; }
        public DateTime DataPedido { get; set; }
        public string StatusPedido { get; set; }

        public List<AcompanharPedidos> PedidosById() {
            List<AcompanharPedidos> pedidos = new List<AcompanharPedidos>();

            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.CommandText = "SELECT idPedido,dataPedido,nomeTipo,nomeStatus FROM pedido " +
                "JOIN TipoPgto on pedido.idTipoPgto = TipoPgto.IdTipoPgto JOIN StatusPedido " +
                "ON pedido.IdStatusPedido = StatusPedido.idStatusPedido WHERE idCliente = @IDCLIENTE";

            try
            {
                conexao.command.Parameters.Clear();
                conexao.command.Parameters.Add("@IDCLIENTE", SqlDbType.Int).Value = (int)HttpContext.Current.Session["idUsuario"];
            }
            catch (Exception e) {
                Console.WriteLine(e.StackTrace);
            }
            SqlDataReader dr = conexao.command.ExecuteReader();

            if(dr.HasRows) {
                while(dr.Read()) {
                    pedidos.Add(new AcompanharPedidos {
                        IdPedido = dr.GetInt32(0),
                        DataPedido = dr.GetDateTime(1),
                        TipoDePagamento = dr.GetString(2),
                        StatusPedido = dr.GetString(3)
                    });
                }
            }

            return pedidos;
        }
    }
}