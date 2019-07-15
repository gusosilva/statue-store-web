using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StatueStoreWebApplic.Models.DBModels
{

    public class ProdutoPersonalizado
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Imagem { get; set; }
        public string ImgModelo { get; set; }
        public string Cor { get; set; }
        public string Sexo { get; set; }
        public int Subgrupo { get; set; }
        public int Cliente { get; set; }

        public ProdutoPersonalizado() { }
    }
}