using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace StatueStoreWebApplic.Models {
    public class VisaoGeral {

        public string TipoDePagamento { get; set; } 
        public string FinalCartao { get; set ; }
        public List<ProdutoCarrinho> produtos { get; set; }
        public decimal Total { get; set; }
        public decimal Desconto { get; set; }
        public Endereco endereco { get; set; }
        public string DataPrevista { get; set; }
        public string Meio { get; set; }
        public decimal PrecoFrete { get; set; }

        public void CompraVisaoGeral() {
            Total = 0;
            FinalCartao = "0"; //0 significa que a compra não foi utilizada cartão
            TipoDePagamento = (string)HttpContext.Current.Session["NomeTipoDePagamento"]; //Sessão pega o tipo de Pagamento apra ser exibido

            if((int)HttpContext.Current.Session["TipoDePagamento"] == 2) { //Verifica se o id do tipo de pgmto é a do Cartão de Crédito

                CartaoDeCredito CartaoCredito;

                //Pega os ultimos digitos no cartão. Tanto sessão tanto banco.
                if (HttpContext.Current.Session["InfoCartao"] != null) { //Verifica se o usuario escolheu nao cadastrar no banco

                    CartaoCredito = (CartaoDeCredito)HttpContext.Current.Session["InfoCartao"]; // Instancia o cartão da sessão
                    FinalCartao = CartaoCredito.NumeroCartao.Substring(Convert.ToInt32(CartaoCredito.NumeroCartao.Length) - 4, 4); //Ultimos 4 digitos
                }
                else {
                    CartaoCredito = new CartaoDeCredito();
                    FinalCartao = CartaoCredito.UltimosDigitos();
                }
            }

            //Populando a lista de produtos que estão no carrinho do usuario
            MeuCarrinhoModel carrinho = new MeuCarrinhoModel();
            produtos = carrinho.GetByUserId((int)HttpContext.Current.Session["idUsuario"]);

            //Percorrendo os produtos do cliente e somando os valores para obter o total c:
            foreach (var produto in produtos) Total += produto.Preco * produto.Quantidade;
            Total += 25;
            Desconto = 0;

            //Alimentando classe endereco e fornecendo para a classe
            Endereco enderecoClass = new Endereco();
            enderecoClass.GetEnderecoByIdEnd((int)HttpContext.Current.Session["idEndereco"]);
            endereco = enderecoClass;

            DataPrevista = DateTime.Now.AddDays(15).ToString();

            PrecoFrete = 25.00m;
            Meio = "Correios";
        }

        public void DetalhePedido(int idPedido) {
            Total = 0m;
            BDConexao conexao = new BDConexao();
            conexao.connection.Open();

            Meio = "Correios";

            conexao.command.CommandText = "SELECT MEIO,FRETE,DATAPREVISAO FROM ENVIO WHERE IDENVIO  = (SELECT IDENVIO FROM Pedido WHERE idPedido = @IDPEDIDO)";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@IDPEDIDO", System.Data.SqlDbType.Int).Value = idPedido;

            SqlDataReader dr = conexao.command.ExecuteReader();

            dr.Read();

            Meio = dr.GetString(0);
            PrecoFrete = dr.GetDecimal(1);
            DateTime data = dr.GetDateTime(2);
            DataPrevista = data.ToString().Substring(0, 11);

            dr.Close();

            conexao.command.CommandText = "SELECT nomeTipo FROM TipoPgto WHERE idTipoPgto = (SELECT IDTIPOPGTO FROM PEDIDO WHERE IDPEDIDO = @IDPEDIDO)";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@IDPEDIDO", System.Data.SqlDbType.Int).Value = idPedido;

            TipoDePagamento = (string)conexao.command.ExecuteScalar();

            MeuCarrinhoModel carrinho = new MeuCarrinhoModel();
            produtos = carrinho.GetDetalhePedidoPorId(idPedido);

            //Percorrendo os produtos do cliente e somando os valores para obter o total c:
            foreach (var produto in produtos)
                Total += produto.Preco;

            Total += 25m;
            Desconto = 0m;

            conexao.command.CommandText = "SELECT IDENDERECO FROM ENDERECO_CLIENTE WHERE IDENDERECO_CLIENTE = (SELECT IDENDERECO_CLIENTE FROM " +
                "PEDIDO WHERE IDPEDIDO = @IDPEDIDO)";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@IDPEDIDO", System.Data.SqlDbType.Int).Value = idPedido;

            int idEndereco = (int)conexao.command.ExecuteScalar();

            Endereco enderecoClass = new Endereco();
            enderecoClass.GetEnderecoByIdEnd(idEndereco);
            endereco = enderecoClass;
        }

    }
}