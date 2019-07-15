using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace StatueStoreWebApplic.Models {
    public class Cliente {
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }

        [Required]
        public string Senha { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Sobrenome { get; set; }

        [Required]
        public string Sexo { get; set; }
        [Required]
        [DisplayName("CPF")]
        public string Cpf { get; set; }
        [Required]
        public DateTime DataNascimento { get; set; }

        [Required]
        public string DataInscricao = DateTime.Now.ToString().Replace("/", "-");


        public void GetClientById(int id) {

            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.CommandText = "SELECT * FROM CLIENTE WHERE idCliente = @ID";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@ID", SqlDbType.Int).Value = id;

            System.Data.SqlClient.SqlDataReader dataReader = conexao.command.ExecuteReader();

            if (dataReader.HasRows) {
                while (dataReader.Read()) {
                    Id = dataReader.GetInt32(0);
                    Email = dataReader.GetString(1);
                    Senha = dataReader.GetString(2);
                    Nome = dataReader.GetString(3);
                    Sobrenome = dataReader.GetString(4);
                    Sexo = dataReader.GetString(5);
                    Cpf = dataReader.GetString(6);
                    DataNascimento = dataReader.GetDateTime(7);
                    DataInscricao = dataReader.GetDateTime(8).ToString();
                }
            }

            dataReader.Close();
            conexao.connection.Close();
        }

        public int BdSetClient() {
            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.CommandText = "INSERT INTO CLIENTE OUTPUT INSERTED.IDCLIENTE VALUES " +
                "(@EMAIL, @SENHA, @NOME, @SOBRENOME, @SEXO, @CPF, @DATANASC, @DATAINSC, null)";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = Email;
            conexao.command.Parameters.Add("@SENHA", SqlDbType.VarChar).Value = StatueStoreEncrypt.Encrypt(Senha);
            conexao.command.Parameters.Add("@NOME", SqlDbType.VarChar).Value = Nome;
            conexao.command.Parameters.Add("@SOBRENOME", SqlDbType.VarChar).Value = Sobrenome;
            conexao.command.Parameters.Add("@SEXO", SqlDbType.VarChar).Value = TrataSexo(Sexo);
            conexao.command.Parameters.Add("@CPF", SqlDbType.VarChar).Value = Cpf.Trim().Replace(".", "").Replace("-", "").Replace(" ", "");
            conexao.command.Parameters.Add("@DATANASC", SqlDbType.Date).Value = DataNascimento;
            conexao.command.Parameters.Add("@DATAINSC", SqlDbType.Date).Value = DataInscricao;

            int idCliente = (int)conexao.command.ExecuteScalar();

            conexao.connection.Close();

            EnviarEmailParaCliente(Email, Nome);

            return idCliente;
        }

        public bool EnviarEmailParaCliente(string email, string nome) {
            try {
                statueEmailSender sender = new statueEmailSender();
                sender.AdicionarEmail(email);
                sender.SetBodyNovoCliente(nome);
                sender.Assunto = "Bem vindo a Statue Store!";
                sender.Enviar();
                return true;
            } catch {
                return false;
            }
        }

        private string TrataSexo(string s) {

            if (s.Trim() == "Masculino")
                return "M";
            if (s.Trim() == "Feminino")
                return "F";

            return String.Empty;
        }
    }
}