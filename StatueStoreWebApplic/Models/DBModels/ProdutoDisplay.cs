using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StatueStoreWebApplic.Models {
    public class ProdutoDisplay {
        public int Id { get; set; }
        public string nome { get; set; }
        public string imagem { get; set; }
        public decimal preco { get; set; }
    }
}