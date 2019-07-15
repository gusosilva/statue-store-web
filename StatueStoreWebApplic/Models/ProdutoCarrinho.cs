using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StatueStoreWebApplic.Models
{
    public class ProdutoCarrinho
    {
        public int IdProduto { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public int Quantidade { get; set; }
        public string Tamanho { get; set; }
        public string Imagem { get; set; }
    }
}