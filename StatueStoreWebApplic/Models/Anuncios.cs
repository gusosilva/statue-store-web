using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace StatueStoreWebApplic.Models
{
    public class Anuncios
    {
        List<AnuncioDisplay> DaoAnuncios;

        public List<AnuncioDisplay> PegarAnuncios(int quantidade)
        {
            DaoAnuncios = new List<AnuncioDisplay>();
            BDConexao conexao = new BDConexao();
            conexao.connection.Open();

            //conexao.command.CommandText = "SELECT TOP @#QUANTIDADE idDetalhe_Anuncio,imagem,linkSite,cliquesUteis,cliquesContados FROM ANUNCIO JOIN Detalhe_Anuncio " +
            //    "ON ANUNCIO.idAnuncio = Detalhe_Anuncio.idAnuncio WHERE idStatusAnuncio = 1 ORDER BY NEWID()";

            conexao.command.CommandText = "SELECT TOP @#QUANTIDADE idDetalhe_Anuncio,imagem,linkSite FROM ANUNCIO JOIN Detalhe_Anuncio " +
               "ON ANUNCIO.idAnuncio = Detalhe_Anuncio.idAnuncio WHERE idStatusAnuncio = 1 ORDER BY NEWID()";

            conexao.command.CommandText = conexao.command.CommandText.Replace("@#QUANTIDADE", quantidade.ToString());
            SqlDataReader dr = conexao.command.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    DaoAnuncios.Add(new AnuncioDisplay {
                        IdDetalhe = dr.GetInt32(0),
                        urlImagem = dr.GetString(1),
                        Link = dr.GetString(2),
                    });
                }
            }


            conexao.connection.Close();
            return DaoAnuncios;
        }

        public void TouchAnuncio(int detalhe) {
            
            BDConexao conexao = new BDConexao();
            conexao.connection.Open();

            int cliquesDisponiveis = 0;
            int cliquesAtuais = 0;

            conexao.command.CommandText = "UPDATE Detalhe_Anuncio SET cliquesContados = cliquesContados + 1 " +
                "OUTPUT inserted.cliquesUteis,inserted.cliquesContados WHERE idDetalhe_Anuncio = @IDDETALHE";

            conexao.command.Parameters.Add("@IDDETALHE", System.Data.SqlDbType.Int).Value = detalhe;
            SqlDataReader dr = conexao.command.ExecuteReader();

            if (dr.HasRows) {
                while (dr.Read()) {
                    cliquesDisponiveis = dr.GetInt32(0);
                    cliquesAtuais = dr.GetInt32(1);
                }
            }
            dr.Close();
            if(cliquesAtuais >= cliquesDisponiveis) {
                conexao.command.CommandText = "UPDATE Detalhe_Anuncio SET idStatusAnuncio = 2 WHERE idDetalhe_Anuncio = @IDDETALHE";
                conexao.command.ExecuteNonQuery();
            }

            conexao.connection.Close();
        }
    }
}