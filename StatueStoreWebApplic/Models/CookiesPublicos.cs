using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace StatueStoreWebApplic.Models
{
    public class CookiesPublicos
    {
        //Nome do cookie
        //Caso haja necessidade de alteração, basta faze-la aqui
        private string NomeDoCookie = "StatueStoreCookie";

        public CookiesPublicos()
        {

        }


        public string Cookie()
        {
            if(HttpContext.Current.Session["idUsuario"] == null) //Usuario NÃO está logado
            {
                if (!CheckForCookie()) //Caso não detecte nenhum cookie no navegador
                {
                    String cookieValue = SetCookie();
                    CadastraCookieNoBanco(cookieValue);
                    return cookieValue;
                }
                else
                {
                    if (!VerificaCadastroCookie(GetCookieValue()))
                    {
                        CadastraCookieNoBanco(GetCookieValue());
                    }
                    return GetCookieValue();
                }
            }

            return String.Empty;
        }

        public void RemoveCookie() {
            HttpContext.Current.Response.Cookies[NomeDoCookie].Expires = DateTime.Now.AddDays(-1);
        }
        // FUNÇÕES PARA A CLASSE
        private bool CheckForCookie() //Verifica se há algum cookie no navegador
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[NomeDoCookie];

            if (cookie != null)
                return true;

            return false;
        }

        private string GetCookieValue()
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[NomeDoCookie];
            return cookie.Value;
        }

        private string SetCookie() //Cadastra um cookie no navegador
        {
            HttpCookie cookie = new HttpCookie(NomeDoCookie, keyGen.GetRandomString(16));
            cookie.Expires = DateTime.Now.AddMinutes(30);
            HttpContext.Current.Response.SetCookie(cookie);
            return cookie.Value;
        }

        private void CadastraCookieNoBanco(string cookieValue)
        {
            if (cookieValue != null)
            {
                BDConexao conexao = new BDConexao();

                conexao.connection.Open();
                conexao.command.CommandText = "INSERT INTO CarPublico VALUES (@COOKIE, @DATAVAL)";
                conexao.command.Parameters.Clear();
                conexao.command.Parameters.Add("@COOKIE", SqlDbType.VarChar).Value = cookieValue;
                conexao.command.Parameters.Add("@DATAVAL", SqlDbType.DateTime).Value = DateTime.Now.AddMinutes(30).ToString().Replace("/", "-");
                conexao.command.ExecuteNonQuery();
                conexao.connection.Close();
            }
        }
        private bool VerificaCadastroCookie(string cookieValue)
        {
            if (cookieValue != null)
            {
                BDConexao conexao = new BDConexao();

                conexao.connection.Open();
                conexao.command.CommandText = "SELECT COUNT(*) FROM CARPUBLICO  WHERE cookieValue = @COOKIE";
                conexao.command.Parameters.Clear();
                conexao.command.Parameters.Add("@COOKIE", SqlDbType.VarChar).Value = cookieValue;

                if ((int)conexao.command.ExecuteScalar() != 0)
                {
                    conexao.connection.Close();
                    return true;
                }
                else
                {
                    conexao.connection.Close();
                    return false;
                }
            }
            return false;
        }

    }
}