using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace StatueStoreWebApplic.Models {
    public class BDConexao {

        public BDConexao() {
            command.Connection = connection;
        }

        public SqlConnection connection = new SqlConnection("Server=localhost;Database=StatueStore;Trusted_Connection=True;");
        public SqlCommand command = new SqlCommand();

    }
}