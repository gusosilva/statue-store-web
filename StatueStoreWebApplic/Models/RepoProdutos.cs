using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace StatueStoreWebApplic.Models {
    public class RepoProdutos {

        public string filtrarPor { get; set; }  //Orders By
        public string buscarPor { get; set; }  //Filtrar pelo usuario


        List<ProdutoDisplay> repoProdutos = new List<ProdutoDisplay>(); //Lista que vai retornar
        BDConexao conexao = new BDConexao(); //Banco

        //Construtor
        public RepoProdutos() {

        }

        //Filtros
        public List<ProdutoDisplay> Tudo() {
            conexao.connection.Open();
            conexao.command.CommandText = "SELECT idProduto,nome,imagem,precoVenda FROM PRODUTO" + stringFiltrarPor();
            return populate();
        }

        public List<ProdutoDisplay> TudoFeminino() {
            conexao.connection.Open();
            conexao.command.CommandText = "SELECT idProduto,nome,imagem,precoVenda FROM PRODUTO WHERE (sexo = 'F' or sexo = 'U')" + stringBuscaPor() + stringFiltrarPor();
            return populate();
        }

        public List<ProdutoDisplay> tudoMasculino() {
            conexao.connection.Open();
            conexao.command.CommandText = "SELECT idProduto,nome,imagem,precoVenda FROM PRODUTO WHERE (sexo = 'M' or sexo = 'U')" + stringBuscaPor() + stringFiltrarPor();
            return populate();
        }

        public List<ProdutoDisplay> TudoFemininoPorGrupo(string grupo) {
            conexao.connection.Open();
            conexao.command.Parameters.Clear();
            conexao.command.CommandText = "SELECT idProduto,nome,imagem,precoVenda FROM Produto WHERE idSubgrupo IN " +
                "(SELECT idSubgrupo FROM Subgrupo WHERE idGrupo IN " +
                "(SELECT idGrupo FROM Grupo WHERE nomeGrupo = @GRUPO)) and sexo = 'F' or sexo = 'U'" + stringBuscaPor() + stringFiltrarPor();
            conexao.command.Parameters.Add("@GRUPO", SqlDbType.VarChar).Value = grupo;
            return populate();
        }

        public List<ProdutoDisplay> tudoMasculinoPorGrupo(string grupo) {
            conexao.connection.Open();
            conexao.command.Parameters.Clear();
            conexao.command.CommandText = "SELECT idProduto,nome,imagem,precoVenda FROM Produto WHERE idSubgrupo IN " +
                "(SELECT idSubgrupo FROM Subgrupo WHERE idGrupo IN " +
                "(SELECT idGrupo FROM Grupo WHERE nomeGrupo = @GRUPO)) and (sexo = 'M' or sexo = 'U')" + stringBuscaPor() + stringFiltrarPor();

            conexao.command.Parameters.Add("@GRUPO", SqlDbType.VarChar).Value = grupo;
            return populate();
        }

        // Feminino por grupo e tamanho
        public List<ProdutoDisplay> FemininoPorGrupoETamanho(string grupo, string tamanho) {

            conexao.connection.Open();
            conexao.command.Parameters.Clear();

            String command = "SELECT idProduto,nome,imagem,precoVenda FROM Produto WHERE idSubgrupo IN " +
                "(SELECT idSubgrupo FROM Subgrupo WHERE idGrupo IN (SELECT idGrupo FROM Grupo WHERE nomeGrupo = @GRUPO)) " +
                "and idProduto in (SELECT idProduto FROM Detalhe_Tamanho WHERE idTamanho in (SELECT idTamanho FROM Tamanho WHERE tamanho = @TAM)) " +
                "and sexo = 'F' or sexo = 'U'";

            conexao.command.CommandText = command + stringFiltrarPor();

            conexao.command.Parameters.Add("@GRUPO", SqlDbType.VarChar).Value = grupo;
            conexao.command.Parameters.Add("@TAM", SqlDbType.VarChar).Value = tamanho;
            return populate();
        }

        // Masculino por grupo tamanho
        public List<ProdutoDisplay> MasculinoPorGrupoETamanho(string grupo, string tamanho) {
            conexao.connection.Open();

            conexao.command.Parameters.Clear();
            String command = "SELECT idProduto,nome,imagem,precoVenda FROM Produto WHERE idSubgrupo IN " +
                "(SELECT idSubgrupo FROM Subgrupo WHERE idGrupo IN (SELECT idGrupo FROM Grupo WHERE nomeGrupo = @GRUPO)) " +
                "and idProduto in (SELECT idProduto FROM Detalhe_Tamanho WHERE idTamanho in (SELECT idTamanho FROM Tamanho WHERE tamanho = @TAM)) " +
                "and sexo = 'M' or sexo = 'U'";

            conexao.command.CommandText = command + stringFiltrarPor();

            conexao.command.Parameters.Add("@GRUPO", SqlDbType.VarChar).Value = grupo;
            conexao.command.Parameters.Add("@TAM", SqlDbType.VarChar).Value = tamanho;
            return populate();
        }

        public List<ProdutoDisplay> porTamanho(string tamanho) {

            conexao.connection.Open();

            conexao.command.Parameters.Clear();

            conexao.command.CommandText = "SELECT idProduto,nome,imagem,precoVenda FROM Produto as p inner " +
                "JOIN Detalhe_Tamanho as pt on p.idProduto = pt.idProduto " +
                "WHERE pt.idTamanho IN(SELECT idTamanho FROM Tamanho WHERE TAMANHO = @TAM)" + stringFiltrarPor();

            conexao.command.Parameters.Add("@TAM", SqlDbType.VarChar).Value = tamanho;
            return populate();
        }

        public List<ProdutoDisplay> MasculinoPorTamanho(string tamanho) {

            conexao.connection.Open();

            conexao.command.Parameters.Clear();

            conexao.command.CommandText = "SELECT idProduto,nome,imagem,precoVenda FROM Produto WHERE idProduto in " +
                "(SELECT idProduto FROM Detalhe_Tamanho WHERE idTamanho = (SELECT idTamanho FROM Tamanho WHERE tamanho = @TAM))  " +
                "and sexo = 'M' or sexo = 'U'" + stringFiltrarPor();

            conexao.command.Parameters.Add("@TAM", SqlDbType.VarChar).Value = tamanho;
            return populate();
        }
        public List<ProdutoDisplay> FemininoPorTamanho(string tamanho) {

            conexao.connection.Open();

            conexao.command.Parameters.Clear();

            conexao.command.CommandText = "SELECT idProduto,nome,imagem,precoVenda FROM Produto as p inner " +
                "JOIN Detalhe_Tamanho as pt on p.idProduto = pt.idProduto " +
                "WHERE pt.idTamanho IN(SELECT idTamanho FROM Tamanho WHERE TAMANHO = @TAM) " +
                "and sexo = 'F' or sexo = 'U'" + stringFiltrarPor();

            conexao.command.Parameters.Add("@TAM", SqlDbType.VarChar).Value = tamanho;
            return populate();
        }

        //Funções Auxiliares
        List<ProdutoDisplay> populate() { //Popula a lista, encerra conexão e o reader.
            SqlDataReader dr = conexao.command.ExecuteReader();
            if (dr.HasRows) {
                while (dr.Read()) {
                    ProdutoDisplay prod = new ProdutoDisplay() {
                        Id = dr.GetInt32(0),
                        nome = dr.GetString(1),
                        imagem = dr.GetString(2),
                        preco = dr.GetDecimal(3)
                    };
                    repoProdutos.Add(prod);
                }
            }
            conexao.connection.Close();
            dr.Close();
            return repoProdutos;
        }


        //Adiciona o ORDER BY no final caso o usuário tenha especifiado algum filtro (atributo que é adicionado pela classe auxiliar)
        string stringFiltrarPor() {

            switch (filtrarPor) {
                case "porNomeAz":
                    return " ORDER BY NOME";
                case "porNomeZa":
                    return " ORDER BY NOME DESC";
                case "menorValor":
                    return " ORDER BY precoVenda ASC";
                case "maiorValor":
                    return " ORDER BY precoVenda desc";
            }

            return "";
        }

        string stringBuscaPor() {
            if (!string.IsNullOrWhiteSpace(buscarPor)) {
                conexao.command.Parameters.Add("@BUSCA", SqlDbType.VarChar).Value = buscarPor;
                return " AND NOME = @BUSCA";
            }

            return "";
        }

    }
}


/*
 * 
 * NOME TAMANHO FILTRAR
 * NOME TAMANHO
 * NOME FILTRAR
 * TAMANHO FILTRAR
 * TAMANHO
 * FILTRAR
 * 
*/
