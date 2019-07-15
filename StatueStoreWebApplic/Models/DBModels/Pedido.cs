using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace StatueStoreWebApplic.Models {
    public class Pedido {

        public int Id { get; set; } //
        public int IdCartaoCredito { get; set; } // Session["InfoCartao"]
        public int IdEnvio { get; set; } // class Envio
        public int IdTipoPagamento { get; set; }// TipoDePagamento
        public int IdStatus { get; set; } // 0
        public int IdCliente { get; set; } //tenho
        public int IdEnderecoCliente { get; set; } //tenho
        public int PagamentoAtivo { get; set; } //Caso de boleto: 0 ou 1 para a validação do pagamento do boleto c:


        public void ConcluirPedido()
        {
            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.CommandText = "INSERT INTO PEDIDO OUTPUT INSERTED.IDPEDIDO VALUES (@DATAPEDIDO, @IDENVIO, @IDTIPOPGTO, @IDCLIENTE, " + 
                "@IDENDERECO_CLIENTE, @IDCARTAOCREDITO, null,@PAGAMENTOATIVO , @IDSTATUSPEDIDO)";

            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@DATAPEDIDO", SqlDbType.DateTime).Value = DateTime.Now.ToString().Replace("/", "-");
            conexao.command.Parameters.Add("@IDENVIO", SqlDbType.Int).Value = IdEnvio;
            conexao.command.Parameters.Add("@IDTIPOPGTO", SqlDbType.Int).Value = IdTipoPagamento;
            conexao.command.Parameters.Add("@IDCLIENTE", SqlDbType.Int).Value = IdCliente;
            conexao.command.Parameters.Add("@IDENDERECO_CLIENTE", SqlDbType.Int).Value = IdEnderecoCliente;
            conexao.command.Parameters.Add("@IDSTATUSPEDIDO", SqlDbType.Int).Value = IdStatus;


            if(IdCartaoCredito == 0)
                conexao.command.CommandText = conexao.command.CommandText.Replace("@IDCARTAOCREDITO", "null");
            else
                conexao.command.Parameters.Add("@IDCARTAOCREDITO", SqlDbType.Int).Value = IdCartaoCredito;

            conexao.command.Parameters.Add("@PAGAMENTOATIVO", SqlDbType.TinyInt).Value = PagamentoAtivo;

            int idPedido = (int)conexao.command.ExecuteScalar();

            conexao.command.Parameters.Clear();
            conexao.command.CommandText = "INSERT INTO Detalhe_Pedido(idProduto, quantidade, idPedido, idTamanho) " +
                "SELECT idProduto, quantidade,@IDPEDIDO,idTamanho FROM CarCliente WHERE idCliente = @IDCLIENTE";
            conexao.command.Parameters.Clear();

            conexao.command.Parameters.Add("@IDPEDIDO", SqlDbType.Int).Value = idPedido;
            conexao.command.Parameters.Add("@IDCLIENTE", SqlDbType.Int).Value = IdCliente;
            conexao.command.ExecuteNonQuery();

            produtoComprado compras = new produtoComprado();
            compras.SubtrairDaQuantidade(IdCliente);


            conexao.command.CommandText = "DELETE FROM CARCLIENTE WHERE IDCLIENTE = @IDCLIENTE";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@IDCLIENTE", SqlDbType.Int).Value = IdCliente;
            conexao.command.ExecuteNonQuery();

            conexao.connection.Close();
        }

        public static List<string> TiposDePgmto() {
            BDConexao conexao = new BDConexao();
            List<String> tipos = new List<string>();
            conexao.connection.Open();
            conexao.command.CommandText = "select nomeTipo FROM TipoPgto";
            SqlDataReader dataReader = conexao.command.ExecuteReader();
            tipos.Clear();
            if (dataReader.HasRows)
                while (dataReader.Read())
                    tipos.Add(dataReader.GetString(0));

            conexao.connection.Close();
            dataReader.Close();
            return tipos;
        }

        public int retornaIdEnderecoCliente(int id) {
            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.CommandText = "SELECT idEndereco_Cliente FROM Endereco_Cliente WHERE idEndereco = @IDENDERECO";

            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@IDENDERECO", SqlDbType.Int).Value = id;

            return (int)conexao.command.ExecuteScalar();
        }

        public List<AcompanharPedidos> AcompanhaPorId(int id) {
            List<AcompanharPedidos> pedidos = new List<AcompanharPedidos>();
            List<String> idpedidos = new List<String>();
            BDConexao conexao = new BDConexao();
            conexao.connection.Open();

            conexao.command.CommandText = "SELECT idPedido,dataPedido,nomeTipo FROM pedido JOIN statusPedido on pedido.idTipoPgto = TipoPgto.IdTipoPgto";

            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@IDENDERECO", SqlDbType.Int).Value = id;

            return pedidos;
        }

    }

    class ProdutoCarClientee {
        public int Quantidade { get; set; }
        public int IdProduto { get; set; }
        public int IdPedido { get; set; }
        public int IdTamanho { get; set; }
    }

    class produtoComprado {
        public int IdProduto { get; set; }
        public int Quantidade { get; set; }
        public int Tamanho { get; set; }


        public void SubtrairDaQuantidade(int idCliente) {
            List<produtoComprado> produtosComprados = new List<produtoComprado>();


            BDConexao conexao = new BDConexao();
            conexao.command.CommandText = "SELECT IDPRODUTO,QUANTIDADE,IDTAMANHO FROM CARCLIENTE WHERE IDCLIENTE = @IDCLIENTE";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@IDCLIENTE", SqlDbType.Int).Value = idCliente;
            conexao.connection.Open();
            SqlDataReader dr = conexao.command.ExecuteReader();

            if (dr.HasRows) {
                while (dr.Read()) {
                    produtosComprados.Add(new produtoComprado {
                        IdProduto = dr.GetInt32(0),
                        Quantidade = dr.GetInt32(1),
                        Tamanho = dr.GetInt32(2)
                    });
                }
            }
            dr.Close();


            string subtrairDoTamanho = "UPDATE Detalhe_Tamanho SET QUANTIDADE = QUANTIDADE - @#QUANTIDADE OUTPUT INSERTED.quantidade,Inserted.quantidadeMin WHERE IDTAMANHO = @IDTAMANHO AND IDPRODUTO = @IDPRODUTO";

            conexao.command.CommandText = subtrairDoTamanho;
            conexao.command.Parameters.Clear();


            foreach(var item in produtosComprados) {
                conexao.command.Parameters.Add("@#QUANTIDADE", SqlDbType.Int).Value = item.Quantidade;
                conexao.command.Parameters.Add("@IDTAMANHO", SqlDbType.Int).Value = item.Tamanho;
                conexao.command.Parameters.Add("@IDPRODUTO", SqlDbType.Int).Value = item.IdProduto;


                dr = conexao.command.ExecuteReader();

                dr.Read();
                int qtdAtual = dr.GetInt32(0);
                int qtdMinima = dr.GetInt32(1);

                dr.Close();

                if(qtdAtual <= qtdMinima) {
                    AlertQuantidadeProd.Alertar(item.IdProduto, item.Tamanho, qtdAtual, qtdMinima);
                }
            }


            conexao.connection.Close();
        }
    }
}