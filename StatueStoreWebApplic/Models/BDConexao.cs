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

        public SqlConnection connection = new SqlConnection("Server=statuestore-server.database.windows.net;Database=statuestore;User ID=gustavo123;Password=Dd2460500;Trusted_Connection=False;");
        public SqlCommand command = new SqlCommand();

    }
}