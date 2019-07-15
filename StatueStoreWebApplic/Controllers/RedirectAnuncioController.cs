using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StatueStoreWebApplic.Models;

namespace StatueStoreWebApplic.Controllers
{
    public class RedirectAnuncioController : Controller
    {
        // GET: RedirectAnuncio
        public void Index(int id, string link)
        {
            Anuncios anuncios = new Anuncios();
            anuncios.TouchAnuncio(id);
            if (!link.Contains("http://") || !link.Contains("https://")) link = "http://" + link;
            Response.Redirect(link, true);
        }
    }
}