using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace StatueStoreWebApplic.Models {
    public class Envio {
        public int Id { get; set; }
        public decimal Decimal { get; set; }
        public string Meio { get; set; }
        public string DataEnvio { get; set; }
        public string DataEntrega { get; set; }
        public string DataPrevisao { get; set; }


        public int SetEnvio() {
            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.CommandText = "INSERT INTO ENVIO OUTPUT INSERTED.IDENVIO VALUES (@FRETE, @MEIO, @DATAENVIO, @DATAENTREGA, @DATAPREVISAO)";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@FRETE", SqlDbType.Decimal).Value = 25.00m;
            conexao.command.Parameters.Add("@MEIO", SqlDbType.VarChar).Value = "Correios";
            conexao.command.Parameters.Add("@DATAENVIO", SqlDbType.Date).Value = DateTime.Now.AddDays(2);
            conexao.command.Parameters.Add("@DATAENTREGA", SqlDbType.Date).Value = DateTime.Now.AddDays(15);
            conexao.command.Parameters.Add("@DATAPREVISAO", SqlDbType.Date).Value = DateTime.Now.AddDays(16);
            int id = (int)conexao.command.ExecuteScalar();
            conexao.connection.Close();

            return id;
        }
    }

    public class ProdutoCarCliente
    {
        public int IdTamanho { get; set; }
        public int IdProduto { get; set; }
        public int Quantidade { get; set; }

    }
}