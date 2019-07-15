using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace StatueStoreWebApplic.Models {
    public class Login {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Senha { get; set; }

        public bool DoIt() {

            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.Parameters.Clear();
            conexao.command.CommandText = "SELECT idCliente,nome FROM Cliente WHERE EMAIL = @EMAIL AND SENHA = @SENHA";
            conexao.command.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = Email;
            conexao.command.Parameters.Add("@SENHA", SqlDbType.VarChar).Value = StatueStoreEncrypt.Encrypt(Senha);


            System.Data.SqlClient.SqlDataReader dr = conexao.command.ExecuteReader();

            if (dr.HasRows) {
                dr.Read();
                HttpContext.Current.Session["idUsuario"] = dr.GetInt32(0);
                HttpContext.Current.Session["nomeUsuario"] = dr.GetString(1);
                conexao.connection.Close();
                return true;
            }

            conexao.connection.Close();
            return false;
        }

        public void CarCookietoUser(string cookie) {
            BDConexao conexao = new BDConexao();
            conexao.connection.Open();

            conexao.command.CommandText = "DELETE FROM CARCLIENTE WHERE IDCLIENTE = @IDCLIENTE";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@IDCLIENTE", SqlDbType.Int).Value = (int)HttpContext.Current.Session["idUsuario"];
            conexao.command.ExecuteNonQuery();

            conexao.command.CommandText = "INSERT INTO CARCLIENTE (idProduto, quantidade, idCliente, idTamanho) " +
                "SELECT idProduto, quantidade,@IDCLIENTE,idTamanho FROM Detalhe_CarPublico WHERE IDCARPUBLICO = (SELECT IDCARPUBLICO FROM CARPUBLICO WHERE COOKIEVALUE = @COOKIE)";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@IDCLIENTE", SqlDbType.Int).Value = (int)HttpContext.Current.Session["idUsuario"];
            conexao.command.Parameters.Add("@COOKIE", SqlDbType.VarChar).Value = cookie;

            conexao.command.ExecuteNonQuery();
        }

        public bool verificaSenha(string senha) {

            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.Parameters.Clear();
            conexao.command.CommandText = "SELECT COUNT(*) FROM CLIENTE WHERE IDCLIENTE = @IDCLIENTE AND SENHA = @SENHA";
            conexao.command.Parameters.Add("@IDCLIENTE", SqlDbType.Int).Value = (int)HttpContext.Current.Session["idUsuario"]; ;
            conexao.command.Parameters.Add("@SENHA", SqlDbType.VarChar).Value = StatueStoreEncrypt.Encrypt(senha);

            int ok = (int)conexao.command.ExecuteScalar();

            conexao.connection.Close();

            if (ok == 0)
                return false;

            else
                return true;
        }

        public void AlterarSenha(string novaSenha) {
            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.Parameters.Clear();
            conexao.command.CommandText = "UPDATE CLIENTE SET SENHA = @NOVASENHA WHERE IDCLIENTE = @IDCLIENTE";
            conexao.command.Parameters.Add("@IDCLIENTE", SqlDbType.Int).Value = (int)HttpContext.Current.Session["idUsuario"];
            conexao.command.Parameters.Add("@NOVASENHA", SqlDbType.VarChar).Value = StatueStoreEncrypt.Encrypt(novaSenha);

            conexao.command.ExecuteNonQuery();
            conexao.connection.Close();
        }

        public string verificaEmail(string email) {

            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.Parameters.Clear();
            conexao.command.CommandText = "SELECT NOME FROM CLIENTE WHERE email = @EMAIL";
            conexao.command.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = email;

            var nome = conexao.command.ExecuteScalar();

            conexao.connection.Close();

            if (nome == null)
                return "";
            else
                return (string)nome;
        }

        public void SetRestoreKeyOnUser(string email, string key) {
            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.Parameters.Clear();
            conexao.command.CommandText = "UPDATE CLIENTE SET codSenha = @CODSENHA WHERE EMAIL = @EMAIL";
            conexao.command.Parameters.Add("@CODSENHA", SqlDbType.VarChar).Value = key;
            conexao.command.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = email;

            conexao.command.ExecuteNonQuery();
            conexao.connection.Close();
        }

        public void RedefinirSenhaPorkey(string key, string novaSenha) {
            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.Parameters.Clear();
            conexao.command.CommandText = "UPDATE CLIENTE SET senha = @SENHA WHERE codSenha = @CODSENHA";
            conexao.command.Parameters.Add("@SENHA", SqlDbType.VarChar).Value = StatueStoreEncrypt.Encrypt(novaSenha);
            conexao.command.Parameters.Add("@CODSENHA", SqlDbType.VarChar).Value = key;

            conexao.command.ExecuteNonQuery();
            conexao.connection.Close();
        }

        public bool VerificarCodigo(string key) {
            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.Parameters.Clear();
            conexao.command.CommandText = "SELECT COUNT(*) FROM CLIENTE WHERE CODSENHA = @CODSENHA";
            conexao.command.Parameters.Add("@CODSENHA", SqlDbType.VarChar).Value = key;

            int cont = (int)conexao.command.ExecuteScalar();

            conexao.connection.Close();

            if (cont == 0)
                return false;
            else
                return true;
        }
    }
}