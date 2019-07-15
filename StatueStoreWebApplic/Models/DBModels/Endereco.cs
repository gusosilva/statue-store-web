using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data;

namespace StatueStoreWebApplic.Models
{
    public class Endereco
    {

        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "CEP")]
        public string Cep { get; set; }
        [Required]
        [Display(Name = "País")]
        public string Pais { get; set; }
        [Required]
        public string Estado { get; set; }
        [Required]
        public string Cidade { get; set; }
        [Required]
        public string Bairro { get; set; }
        [Required]
        public string Logradouro { get; set; }
        [Required]
        [Display(Name = "Tipo de Logradouro")]
        public string TipoLogradouro { get; set; }
        [Required]
        [Display(Name = "Número")]
        public int Numero { get; set; }
        public string Complemento { get; set; }

        public Endereco() {
            Complemento = "nenhum";
        }

        public void CadastraBanco()
        {

            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.CommandText = "INSERT INTO ENDERECO OUTPUT INSERTED.IDENDERECO VALUES(@CEP, @PAIS, @ESTADO, @CIDADE, @BAIRRO, @LOGRADOURO, @TIPOLOGRADOURO, @NUMERO, @COMPLEMENTO)";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@CEP", SqlDbType.VarChar).Value = Cep.Trim().Replace(".", "").Replace("-", "").Replace(" ", "");
            conexao.command.Parameters.Add("@PAIS", SqlDbType.VarChar).Value = Pais;
            conexao.command.Parameters.Add("@ESTADO", SqlDbType.VarChar).Value = Estado;
            conexao.command.Parameters.Add("@CIDADE", SqlDbType.VarChar).Value = Cidade;
            conexao.command.Parameters.Add("@BAIRRO", SqlDbType.VarChar).Value = Bairro;
            conexao.command.Parameters.Add("@LOGRADOURO", SqlDbType.VarChar).Value = Logradouro;
            conexao.command.Parameters.Add("@TIPOLOGRADOURO", SqlDbType.VarChar).Value = TipoLogradouro;
            conexao.command.Parameters.Add("@NUMERO", SqlDbType.VarChar).Value = Numero;
            if(Complemento == null)
                conexao.command.Parameters.Add("@COMPLEMENTO", SqlDbType.VarChar).Value = "N/A";
            else
                conexao.command.Parameters.Add("@COMPLEMENTO", SqlDbType.VarChar).Value = Complemento;

            int idEndereco = (int)conexao.command.ExecuteScalar();

            conexao.command.Parameters.Clear();
            conexao.command.CommandText = "INSERT INTO ENDERECO_CLIENTE VALUES(@IDCLIENTE, @IDENDERECO)";
            conexao.command.Parameters.Add("@IDCLIENTE", SqlDbType.Int).Value = (int)HttpContext.Current.Session["idUsuario"];
            conexao.command.Parameters.Add("@IDENDERECO", SqlDbType.Int).Value = idEndereco;
            conexao.command.ExecuteNonQuery();

            conexao.connection.Close();
        }

        public void DeletaEndereco(int idde)
        {
            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.CommandText = "DELETE FROM Endereco_Cliente WHERE IDCLIENTE = @IDCLIENTE AND idEndereco = @IDENDERECO";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@IDCLIENTE", SqlDbType.VarChar).Value = HttpContext.Current.Session["idUsuario"];
            conexao.command.Parameters.Add("@IDENDERECO", SqlDbType.VarChar).Value = idde;

            conexao.command.ExecuteNonQuery();

            conexao.command.Parameters.Clear();
            conexao.command.CommandText = "DELETE FROM ENDERECO WHERE IDENDERECO = @IDENDERECO";
            conexao.command.Parameters.Add("@IDENDERECO", SqlDbType.VarChar).Value = idde;
            conexao.command.ExecuteNonQuery();

            conexao.connection.Close();
        }


        public List<Endereco> EnderecosDoCliente()
        {

            List<Endereco> enderecos = new List<Endereco>();
            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.CommandText = "SELECT * FROM Endereco WHERE idEndereco IN (SELECT idEndereco FROM Endereco_Cliente WHERE idCliente = @ID)";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@ID", SqlDbType.VarChar).Value = (int)HttpContext.Current.Session["idUsuario"];

            System.Data.SqlClient.SqlDataReader dr = conexao.command.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    enderecos.Add(new Endereco
                    {
                        Id = dr.GetInt32(0),
                        Cep = dr.GetString(1),
                        Pais = dr.GetString(2),
                        Estado = dr.GetString(3),
                        Cidade = dr.GetString(4),
                        Bairro = dr.GetString(5),
                        Logradouro = dr.GetString(6),
                        TipoLogradouro = dr.GetString(7),
                        Numero = dr.GetInt32(8),
                        Complemento = dr.GetString(9)
                    });
                }
            }

            dr.Close();
            conexao.connection.Close();
            return enderecos;
        }
        public bool TemEndereco()
        {
            List<Endereco> enderecos = new List<Endereco>();
            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.CommandText = "SELECT COUNT(*) from Endereco_Cliente WHERE IDCLIENTE = @ID";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@ID", SqlDbType.VarChar).Value = (int)HttpContext.Current.Session["idUsuario"];

            bool tem;
            int count = (int)conexao.command.ExecuteScalar();

            if (count != 0)
                tem = true;
            else
                tem = false;

            conexao.connection.Close();

            return tem;
        }

        public List<EnderecoVisualiza> CepDosClientes()
        {

            List<EnderecoVisualiza> enderecos = new List<EnderecoVisualiza>();
            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.CommandText = "SELECT idEndereco,Estado,CEP FROM Endereco WHERE idEndereco IN (SELECT idEndereco FROM Endereco_Cliente WHERE idCliente = @ID)";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@ID", SqlDbType.VarChar).Value = (int)HttpContext.Current.Session["idUsuario"];

            System.Data.SqlClient.SqlDataReader dr = conexao.command.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    enderecos.Add(new EnderecoVisualiza
                    {
                        Id = dr.GetInt32(0),
                        Estado = dr.GetString(1),
                        Cep = dr.GetString(2)
                    });
                }
            }

            dr.Close();
            conexao.connection.Close();
            return enderecos;
        }

        public void GetEnderecoByIdEnd(int id)
        {
            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            //conexao.command.CommandText = "SELECT * FROM ENDERECO WHERE idEndereco = @IDENDERECO AND idEndereco = (SELECT idEndereco FROM Endereco_Cliente WHERE idCliente = @IDCLIENTE)";
            conexao.command.CommandText = "SELECT * FROM ENDERECO WHERE idEndereco = @IDENDERECO";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@IDENDERECO", SqlDbType.VarChar).Value = id;
            //conexao.command.Parameters.Add("@IDCLIENTE", SqlDbType.Int).Value = (int)HttpContext.Current.Session["idUsuario"];

            System.Data.SqlClient.SqlDataReader dr = conexao.command.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Id = dr.GetInt32(0);
                    Cep = dr.GetString(1);
                    Pais = dr.GetString(2);
                    Estado = dr.GetString(3);
                    Cidade = dr.GetString(4);
                    Bairro = dr.GetString(5);
                    Logradouro = dr.GetString(6);
                    TipoLogradouro = dr.GetString(7);
                    Numero = dr.GetInt32(8);
                    Complemento = dr.GetString(9);
                }
            }

            dr.Close();
            conexao.connection.Close();
        }
    }

    public enum Estados
    {
        AC, AL, AM, AP, BA, CE, DF, ES, GO, MA, MT, MS, MG, PA, PB, PR, PE, PI, RJ, RN, RO, RS, RR, SC, SE, SP, TO
    }

    public enum TipoLogradouros
    {
        Residencial,
        Avenida,
        Vila,
        Fazenda,
        Estrada,
        Sítio
    }

    public class EnderecoVisualiza
    {
        public int Id { get; set; }
        public string Estado { get; set; }
        public string Cep { get; set; }
    }
}