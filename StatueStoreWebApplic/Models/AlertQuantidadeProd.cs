using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StatueStoreWebApplic.Models;
using System.Data.SqlClient;
using System.Data;

namespace StatueStoreWebApplic.Models {
    public class AlertQuantidadeProd {

        public static void Alertar(int id, int tamanho, int quantidadeAtual, int QuantidadeMinima) {
            List<string> emails = new List<string>();

            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.CommandText = "SELECT EMAIL FROM FUNCIONARIO WHERE idNivelAcesso = 4";

            SqlDataReader dr = conexao.command.ExecuteReader();

            if (dr.HasRows)
                while (dr.Read())
                    emails.Add(dr.GetString(0));

            dr.Close();

            conexao.command.CommandText = "SELECT NOME FROM PRODUTO WHERE IDPRODUTO = @IDPRODUTO";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@IDPRODUTO", SqlDbType.Int).Value = id;
            
            string nomeProduto = (string)conexao.command.ExecuteScalar();

            conexao.command.CommandText = "SELECT TAMANHO FROM TAMANHO WHERE IDTAMANHO = @IDTAMANHO";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@IDTAMANHO", SqlDbType.Int).Value = tamanho;

            string nomeTamanho = (string)conexao.command.ExecuteScalar();

            statueEmailSender emailSender = new statueEmailSender();

            emailSender.setBodyAlertProduto(id, nomeProduto, quantidadeAtual, QuantidadeMinima, nomeTamanho);

            emailSender.Assunto = "Produto: " + id + " - " + nomeProduto + " acabando!";

            foreach (var email in emails)
                emailSender.AdicionarEmail(email);

            emailSender.Enviar();
        }
    }
}