using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using StatueStoreWebApplic.Models;
using System.Diagnostics;

namespace StatueStoreWebApplic.Models {
    public class MeuCarrinhoModel {
        public List<ProdutoCarrinho> GetByCookie(string cookieValue) {
            List<ProdutoCarrinho> produtosCarrinho = new List<ProdutoCarrinho>();

            if (!string.IsNullOrWhiteSpace(cookieValue)) {
                BDConexao conexao = new BDConexao();

                conexao.connection.Open();

                conexao.command.CommandText = "SELECT Produto.idProduto,nome,precoVenda,quantidade,idTamanho,imagem FROM Produto JOIN " +
                    "Detalhe_CarPublico ON Produto.idProduto = Detalhe_CarPublico.idProduto WHERE " +
                    "Detalhe_CarPublico.idCarPublico = (SELECT idCarPublico FROM CarPublico WHERE cookieValue = @COOKIE)";

                conexao.command.Parameters.Clear();
                conexao.command.Parameters.Add("@COOKIE", SqlDbType.VarChar).Value = cookieValue;

                SqlDataReader dataReader = conexao.command.ExecuteReader();

                if (dataReader.HasRows) {
                    while (dataReader.Read()) {
                        produtosCarrinho.Add(new ProdutoCarrinho() {
                            IdProduto = dataReader.GetInt32(0),
                            Nome = dataReader.GetString(1),
                            Preco = dataReader.GetDecimal(2),
                            Quantidade = dataReader.GetInt32(3),
                            Tamanho = retornaTamanho(dataReader.GetInt32(4)),
                            Imagem = dataReader.GetString(5)
                        }); // fim do Add

                    } // fim do while

                } //fim do if

                dataReader.Close();
                conexao.connection.Close();
            }

            return produtosCarrinho;
        }

        public List<ProdutoCarrinho> GetByUserId(int userId) {

            List<ProdutoCarrinho> produtosCarrinho = new List<ProdutoCarrinho>();

            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.CommandText = "SELECT Produto.idProduto,nome,precoVenda,quantidade,idTamanho,imagem FROM Produto JOIN " +
                "CarCliente ON Produto.idProduto = CarCliente.idProduto WHERE CarCliente.idCliente = @ID";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@ID", SqlDbType.Int).Value = userId;

            SqlDataReader dataReader = conexao.command.ExecuteReader();

            if (dataReader.HasRows) {
                while (dataReader.Read()) {
                    produtosCarrinho.Add(new ProdutoCarrinho() {
                        IdProduto = dataReader.GetInt32(0),
                        Nome = dataReader.GetString(1),
                        Preco = dataReader.GetDecimal(2),
                        Quantidade = dataReader.GetInt32(3),
                        Tamanho = retornaTamanho(dataReader.GetInt32(4)),
                        Imagem = dataReader.GetString(5)
                    }); // fim do Add

                } // fim do while

            } //fim do if

            dataReader.Close();
            conexao.connection.Close();


            return produtosCarrinho;
        }

        public List<ProdutoCarrinho> GetDetalhePedidoPorId(int idPedido) {

            List<ProdutoCarrinho> produtosCarrinho = new List<ProdutoCarrinho>();

            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.CommandText = "SELECT Produto.idProduto,nome,precoVenda,quantidade,idTamanho,imagem FROM Produto JOIN " +
                "Detalhe_Pedido ON Produto.idProduto = Detalhe_Pedido.idProduto WHERE Detalhe_pedido.idPedido = @IDPEDIDO";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@IDPEDIDO", SqlDbType.Int).Value = idPedido;

            SqlDataReader dataReader = conexao.command.ExecuteReader();

            if (dataReader.HasRows) {
                while (dataReader.Read()) {
                    produtosCarrinho.Add(new ProdutoCarrinho() {
                        IdProduto = dataReader.GetInt32(0),
                        Nome = dataReader.GetString(1),
                        Preco = dataReader.GetDecimal(2),
                        Quantidade = dataReader.GetInt32(3),
                        Tamanho = retornaTamanho(dataReader.GetInt32(4)),
                        Imagem = dataReader.GetString(5)
                    }); // fim do Add

                } // fim do while

            } //fim do if

            dataReader.Close();
            conexao.connection.Close();


            return produtosCarrinho;
        }

        public void SetProdutoByUserId(int userId, int quantidade, int idProduto, string tamanho) {

            BDConexao conexao = new BDConexao();
            conexao.connection.Open();

            //Pega o id do tamanho escolhido
            conexao.command.CommandText = "SELECT idTamanho FROM Tamanho WHERE tamanho = @TAM";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@TAM", SqlDbType.VarChar).Value = tamanho;

            int idTamanho = 0;
            try {
                idTamanho = (int)conexao.command.ExecuteScalar();
            } catch {
                return;
            }

            conexao.command.CommandText = "SELECT COUNT(*) FROM CarCliente WHERE IDPRODUTO = @IDPRODUTO AND IDTAMANHO = @TAMANHO AND IDCLIENTE = @ID";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@IDPRODUTO", SqlDbType.VarChar).Value = idProduto;
            conexao.command.Parameters.Add("@TAMANHO", SqlDbType.VarChar).Value = idTamanho;
            conexao.command.Parameters.Add("@ID", SqlDbType.VarChar).Value = userId;


            if ((int)conexao.command.ExecuteScalar() != 0) {
                conexao.command.CommandText = "UPDATE CarCliente SET quantidade = @QUANTIDADE WHERE idProduto = @IDPRODUTO AND idTamanho = @TAMANHO";
                conexao.command.Parameters.Clear();
                conexao.command.Parameters.Add("@QUANTIDADE", SqlDbType.VarChar).Value = quantidade;
                conexao.command.Parameters.Add("@IDPRODUTO", SqlDbType.VarChar).Value = idProduto;
                conexao.command.Parameters.Add("@TAMANHO", SqlDbType.VarChar).Value = idTamanho;
                conexao.command.ExecuteNonQuery();
                return;
            }

            conexao.command.CommandText = "INSERT INTO CarCliente VALUES(@IDPRODUTO, @QUANTIDADE, @IDCLIENTE, @IDTAMANHO)";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@IDPRODUTO", SqlDbType.VarChar).Value = idProduto;
            conexao.command.Parameters.Add("@QUANTIDADE", SqlDbType.VarChar).Value = quantidade;
            conexao.command.Parameters.Add("@IDCLIENTE", SqlDbType.VarChar).Value = userId;
            conexao.command.Parameters.Add("@IDTAMANHO", SqlDbType.VarChar).Value = idTamanho;
            conexao.command.ExecuteNonQuery();
        }

        public void SetProdutoByCookie(string cookie, int quantidade, int idProduto, string tamanho) {

            BDConexao conexao = new BDConexao();
            conexao.connection.Open();

            conexao.command.CommandText = "SELECT idTamanho FROM Tamanho WHERE tamanho = @TAM";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@TAM", SqlDbType.VarChar).Value = tamanho;

            int idTamanho = 0;
            try {
                idTamanho = (int)conexao.command.ExecuteScalar();
            } catch {
                return;
            }

            conexao.command.CommandText = "SELECT COUNT(*) FROM Detalhe_CarPublico WHERE idProduto = @IDPRODUTO AND idTamanho = @IDTAMANHO AND idCarPublico = " +
                "(SELECT idCarPublico FROM CarPublico WHERE cookieValue = @COOKIE)";

            conexao.command.Parameters.Clear();

            conexao.command.Parameters.Add("@IDPRODUTO", SqlDbType.Int).Value = idProduto;
            conexao.command.Parameters.Add("@IDTAMANHO", SqlDbType.Int).Value = idTamanho;
            conexao.command.Parameters.Add("@COOKIE", SqlDbType.VarChar).Value = cookie;


            if ((int)conexao.command.ExecuteScalar() != 0) {
                conexao.command.CommandText = "UPDATE Detalhe_CarPublico SET quantidade = @QUANTIDADE WHERE idProduto = @IDPRODUTO AND idTamanho = @IDTAMANHO";
                conexao.command.Parameters.Clear();
                conexao.command.Parameters.Add("@IDPRODUTO", SqlDbType.Int).Value = idProduto;
                conexao.command.Parameters.Add("@IDTAMANHO", SqlDbType.Int).Value = idTamanho;
                conexao.command.Parameters.Add("@QUANTIDADE", SqlDbType.Int).Value = quantidade;
                conexao.command.ExecuteNonQuery();
                return;
            }

            conexao.command.CommandText = "SELECT idCarPublico FROM CarPublico WHERE cookieValue = @COOKIE";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@COOKIE", SqlDbType.VarChar).Value = cookie;

            int idCarrinho = 0;
            try {
                idCarrinho = (int)conexao.command.ExecuteScalar();
            } catch {
                return;
            }


            conexao.command.CommandText = "INSERT INTO Detalhe_CarPublico VALUES (@QTD, @IDCARRINHO, @IDPRODUTO, @IDTAMANHO)";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@QTD", SqlDbType.VarChar).Value = quantidade;
            conexao.command.Parameters.Add("@IDCARRINHO", SqlDbType.VarChar).Value = idCarrinho;
            conexao.command.Parameters.Add("@IDPRODUTO", SqlDbType.VarChar).Value = idProduto;
            conexao.command.Parameters.Add("@IDTAMANHO", SqlDbType.VarChar).Value = idTamanho;
            conexao.command.ExecuteNonQuery();
            conexao.connection.Close();
        }

        public void DeleteProductById(int userid, int idproduto, string tamanho)
        {
            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.CommandText = "DELETE FROM CarCliente WHERE idCliente = @IDCLIENTE AND idProduto = @IDPRODUTO AND idTamanho = @IDTAMANHO";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@IDCLIENTE", SqlDbType.VarChar).Value = userid;
            conexao.command.Parameters.Add("@IDPRODUTO", SqlDbType.VarChar).Value = idproduto;
            conexao.command.Parameters.Add("@IDTAMANHO", SqlDbType.VarChar).Value = retornaIdTamanho(tamanho);
            conexao.command.ExecuteNonQuery();
        }

        public void DeleteProductByCookie(string cookie, int idproduto, string tamanho)
        {
            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.CommandText = "DELETE FROM Detalhe_CarPublico WHERE IDCARPUBLICO = " +
                "(SELECT idCarPublico FROM CarPublico WHERE cookieValue = @COOKIE) AND idProduto = @IDPRODUTO AND idTamanho = @IDTAMANHO";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@COOKIE", SqlDbType.VarChar).Value = cookie;
            conexao.command.Parameters.Add("@IDPRODUTO", SqlDbType.VarChar).Value = idproduto;
            conexao.command.Parameters.Add("@IDTAMANHO", SqlDbType.VarChar).Value = retornaIdTamanho(tamanho);
            conexao.command.ExecuteNonQuery();
        }

        private string retornaTamanho(int id) {
            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.CommandText = "SELECT TAMANHO FROM Tamanho WHERE idTamanho = @ID";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@ID", SqlDbType.Int).Value = id;

            string tamanho = (string)conexao.command.ExecuteScalar();

            conexao.connection.Close();

            return tamanho;
        }

        private int retornaIdTamanho(string Tamanho)
        {
            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.CommandText = "SELECT idTamanho FROM Tamanho WHERE Tamanho = @TAMANHO";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@TAMANHO", SqlDbType.VarChar).Value = Tamanho;

            int tamanho = (int)conexao.command.ExecuteScalar();

            conexao.connection.Close();

            return tamanho;
        }

        public bool QuantidadePorTamanho(int idProduto, string tamanho, int quantidade) {

            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.CommandText = "SELECT quantidade FROM DETALHE_TAMANHO WHERE idProduto = @IDPRODUTO and idTamanho = " +
                "(SELECT idTamanho FROM TAMANHO WHERE tamanho = @TAMANHO)";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@IDPRODUTO", SqlDbType.Int).Value = idProduto;
            conexao.command.Parameters.Add("@TAMANHO", SqlDbType.VarChar).Value = tamanho;

            int qtd = (int)conexao.command.ExecuteScalar();
            conexao.connection.Close();

            if(quantidade > qtd) {
                return false;
            }

            return true;
        }
    }
}
