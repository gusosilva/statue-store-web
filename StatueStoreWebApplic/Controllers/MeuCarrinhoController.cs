using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StatueStoreWebApplic.Models;
using System.Data;
using System.Data.SqlClient;

namespace StatueStoreWebApplic.Controllers
{
    public class MeuCarrinhoController : Controller
    {
        // GET: MeuCarrinho
        public ActionResult Index()
        {
            MeuCarrinhoModel carrinho = new MeuCarrinhoModel();
            List<ProdutoCarrinho> produtos = new List<ProdutoCarrinho>();
            CookiesPublicos cookies = new CookiesPublicos();


            if (Session["idUsuario"] != null)
            {
                produtos = carrinho.GetByUserId((int)Session["idUsuario"]);
            }
            else
            {
                produtos = carrinho.GetByCookie(cookies.Cookie());
            }

            return View(produtos);
        }

        /*
        private string TrataNullString(SqlDataReader dr, int index)
        {

            if (!dr.IsDBNull(index))
                return dr.GetString(index);


            return string.Empty;
        }

        private int TrataNullInt(SqlDataReader dr, int index)
        {

            if (!dr.IsDBNull(index))
                return dr.GetInt32(index);


            return 0;
        }
        private decimal TrataNullDecimal(SqlDataReader dr, int index)
        {

            if (!dr.IsDBNull(index))
                return dr.GetDecimal(index);


            return 0m;
        }

    */
    }
}