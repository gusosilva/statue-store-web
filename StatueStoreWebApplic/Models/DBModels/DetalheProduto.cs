using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace StatueStoreWebApplic.Models
{
    public class DetalheProduto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Imagem { get; set; }
        public decimal PrecoVenda { get; set; }
        public string Descricao { get; set; }
        public string DescricaoRed { get; set; }
        public string Modelo { get; set; }
        public string Marca { get; set; }
        public string Sexo { get; set; }
        public int IdSubgrupo { get; set; }
        public int quantidade { get; set; }
        public int quantidadeMin { get; set; }

        public List<string> tamanhos = new List<string>();


        public void GetById(int? id) {

            if (id == null)
                return;

            BDConexao conexao = new BDConexao();
            conexao.command.CommandText = "SELECT produto.idProduto,nome,imagem,precoVenda,descricao,descricaoRed,modelo,marca,sexo,idSubgrupo,quantidade FROM PRODUTO " +
                "LEFT JOIN Detalhe_Tamanho ON produto.idProduto = Detalhe_Tamanho.idProduto WHERE produto.idProduto = @IDPRODUTO";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@IDPRODUTO", SqlDbType.Int).Value = id;
            conexao.connection.Open();
            SqlDataReader dr = conexao.command.ExecuteReader();

            if (dr.HasRows) {
                while (dr.Read()) {
                    Id = TrataNullInt(dr, 0);
                    Nome = TrataNullString(dr ,1);
                    Imagem = TrataNullString(dr, 2);
                    PrecoVenda = TrataNullDecimal(dr, 3);
                    Descricao = TrataNullString(dr, 4);
                    DescricaoRed = TrataNullString(dr, 5);
                    Modelo = TrataNullString(dr, 6);
                    Marca = TrataNullString(dr, 7);
                    Sexo = TrataNullString(dr, 8);
                    IdSubgrupo = TrataNullInt(dr, 9);
                    quantidade = TrataNullInt(dr, 10);
                }
            }
            dr.Close();

            conexao.command.CommandText = "SELECT TAMANHO FROM TAMANHO WHERE idTamanho IN(SELECT IDTAMANHO FROM Detalhe_Tamanho WHERE idProduto = @IDPRODUTO)";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@IDPRODUTO", SqlDbType.Int).Value = id;

            dr = conexao.command.ExecuteReader();
            tamanhos.Clear();

            if (dr.HasRows)
                while (dr.Read())
                    tamanhos.Add(dr.GetString(0));


            conexao.connection.Close();
        }


        //Funções de tratamento para casos de retornos nulos (NULL)
        //Cada Função trata um tipo específico de retorno (string, int, Decimal)
        public string TrataNullString(SqlDataReader dr, int index) {

            if (!dr.IsDBNull(index))
                return dr.GetString(index);


            return string.Empty;
        }

        private int TrataNullInt(SqlDataReader dr, int index) {

            if (!dr.IsDBNull(index))
                return dr.GetInt32(index);


            return 0;
        }

        private decimal TrataNullDecimal(SqlDataReader dr, int index) {

            if (!dr.IsDBNull(index))
                return dr.GetDecimal(index);


            return 0m;
        }

    }
}