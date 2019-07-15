using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data;

namespace StatueStoreWebApplic.Models {
    public static class Tamanhos {
        public static List<string> GetAll(char sexo = 'U', string grupo = "nenhum") {

            List<String> tamanhos = new List<string>();


            BDConexao conexao = new BDConexao();
            conexao.connection.Open();

            String command = "SELECT tamanho FROM Tamanho WHERE idTamanho IN" +
                "(SELECT Detalhe_Tamanho.idTamanho FROM Produto INNER JOIN Detalhe_Tamanho on Produto.idProduto = Detalhe_Tamanho.idProduto WHERE idSubgrupo IN " +
                "(SELECT idSubgrupo FROM Subgrupo WHERE idGrupo in (SELECT idGrupo FROM Grupo WHERE nomeGrupo = @GRUPO) AND (sexo = @SEXO or sexo = 'U')))";

            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@GRUPO", SqlDbType.VarChar).Value = grupo;
            conexao.command.Parameters.Add("@SEXO", SqlDbType.VarChar).Value = sexo;

            conexao.command.CommandText = command;

            SqlDataReader dr = conexao.command.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    tamanhos.Add(dr.GetString(0));
                }
            }

            return tamanhos;
        }
    }
}