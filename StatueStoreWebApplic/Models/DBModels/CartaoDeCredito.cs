using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace StatueStoreWebApplic.Models {
    public class CartaoDeCredito {

        [DisplayName("Nome do titular")]
        [Required]
        public string Titular { get; set; }
        [Required]
        public string Bandeira { get; set; }
        [Required]
        [DisplayName("Número do Cartão")]
        public string NumeroCartao { get; set; }
        [Required]
        [DisplayName("Código de Segurança")]
        public string CVV { get; set; }
        [Required]
        public string Validade { get; set; }
        [Required]
        public int IdCliente { get; set; }
        [Required]
        [DisplayName("Salvar Informações de cartão de crédito")]
        public bool SalvarInfo { get; set; }


        public void CadastraCartao() {
            BDConexao conexao = new BDConexao();
            conexao.connection.Open();

            conexao.command.CommandText = "DELETE FROM CARTAOCREDITO WHERE IDCLIENTE = @ID";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@ID", SqlDbType.VarChar).Value = IdCliente;
            conexao.command.ExecuteNonQuery();

            conexao.command.CommandText = "INSERT INTO CARTAOCREDITO VALUES(@BANDEIRA, @NUMCARTAO, @CVV, @VALIDADE, @IDCLIENTE, @TITULAR)";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@BANDEIRA", SqlDbType.VarChar).Value = StatueStoreEncrypt.Encrypt(Bandeira);
            conexao.command.Parameters.Add("@NUMCARTAO", SqlDbType.VarChar).Value = StatueStoreEncrypt.Encrypt(NumeroCartao.ToString().Trim().Replace(".", "").Replace("-", "").Replace(" ", ""));
            conexao.command.Parameters.Add("@CVV", SqlDbType.VarChar).Value = StatueStoreEncrypt.Encrypt(CVV.Trim().Replace(".", "").Replace("-", "").Replace(" ", ""));
            conexao.command.Parameters.Add("@VALIDADE", SqlDbType.VarChar).Value = StatueStoreEncrypt.Encrypt(Validade);
            conexao.command.Parameters.Add("@IDCLIENTE", SqlDbType.VarChar).Value = IdCliente;
            conexao.command.Parameters.Add("@TITULAR", SqlDbType.VarChar).Value = StatueStoreEncrypt.Encrypt(Titular);
            conexao.command.ExecuteNonQuery();
            conexao.connection.Close();
        }

        public bool PegaCartao() {
            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.CommandText = "SELECT * FROM CARTAOCREDITO WHERE IDCLIENTE = @ID";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@ID", SqlDbType.Int).Value = (int)HttpContext.Current.Session["idUsuario"];

            System.Data.SqlClient.SqlDataReader dr = conexao.command.ExecuteReader();
            
            if (dr.HasRows) {
                dr.Read();
                Bandeira = dr.GetString(1);
                NumeroCartao = dr.GetString(3);
                CVV = dr.GetString(3);
                Validade = dr.GetString(4);

                conexao.connection.Close();
                dr.Close();
                return true;
            }

            conexao.connection.Close();
            dr.Close();
            return false;
        }

        public bool Verifica() {
            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.CommandText = "SELECT COUNT(*) FROM CARTAOCREDITO WHERE IDCLIENTE = @ID";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@ID", SqlDbType.Int).Value = (int)HttpContext.Current.Session["idUsuario"];

            int cont = (int)conexao.command.ExecuteScalar();
            conexao.connection.Close();

            if (cont != 0)
                return true;
            else
                return false;

        }

        public int RetornaId() {
            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.CommandText = "SELECT idCartao FROM CARTAOCREDITO WHERE IDCLIENTE = @ID";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@ID", SqlDbType.Int).Value = (int)HttpContext.Current.Session["idUsuario"];

            int idDoCartao = (int)conexao.command.ExecuteScalar();
            conexao.connection.Close();

            return idDoCartao;
        }

        public string UltimosDigitos() {
            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.CommandText = "SELECT numCartao FROM CARTAOCREDITO WHERE IDCLIENTE = @ID";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@ID", SqlDbType.Int).Value = (int)HttpContext.Current.Session["idUsuario"];

            string numero = string.Empty;

            System.Data.SqlClient.SqlDataReader dr = conexao.command.ExecuteReader();

            if (dr.HasRows) {
                dr.Read();
                numero = StatueStoreEncrypt.Decrypt(dr.GetString(0));
            }


            conexao.connection.Close();
            dr.Close();
            return numero.Substring(numero.Length - 4, 4);
        }

    }

    public enum Bandeira {
          Visa,
          MasterCard,
          Elo,
          HiperCard,
          DinersClub,
          Sorocred,
          AmericanExpress
    }
}