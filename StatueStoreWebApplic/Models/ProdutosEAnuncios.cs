using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StatueStoreWebApplic.Models;

namespace StatueStoreWebApplic.Models {
    public class ProdutosEAnuncios {

        public List<AnuncioDisplay> anuncios = new List<AnuncioDisplay>();
        public List<ProdutoDisplay> produtos = new List<ProdutoDisplay>();

        public ProdutosEAnuncios(int QuantidadeAnuncios, int QuantidadeProdutos) {
            Anuncios anun = new Anuncios();
            anuncios = anun.PegarAnuncios(QuantidadeAnuncios);

            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.CommandText = "SELECT TOP @#QUANTIDADE idProduto,nome,imagem,precoVenda FROM PRODUTO ORDER BY NEWID()";
            conexao.command.CommandText = conexao.command.CommandText.Replace("@#QUANTIDADE", QuantidadeProdutos.ToString());

            System.Data.SqlClient.SqlDataReader dr = conexao.command.ExecuteReader();
            if (dr.HasRows) {
                while (dr.Read()) {
                    ProdutoDisplay prod = new ProdutoDisplay() {
                        Id = dr.GetInt32(0),
                        nome = dr.GetString(1),
                        imagem = dr.GetString(2),
                        preco = dr.GetDecimal(3)
                    };
                    produtos.Add(prod);
                }
            }
            conexao.connection.Close();
            dr.Close();
        }

        public void teste() {
            string a = "eae";
        }
    }
}