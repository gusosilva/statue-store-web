using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StatueStoreWebApplic.Models.DBModels
{
    public class StatusPedido
    {
        public int Id { get; set; }
        public string NomeStatus { get; set; }
        public string Descricao { get; set; }

        public void SetStatus()
        {
        }
    }
}