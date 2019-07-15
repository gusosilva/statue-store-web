using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using StatueStoreWebApplic.Models;
using System.Data;
using System.Data.SqlClient;
using StatueStoreWebApplic.Models.DBModels;

namespace StatueStoreWebApplic.Controllers
{
    public class ApiController : Controller
    {

        // GET: Api
        public JsonResult Index()
        {
            RepoProdutos _repo = new RepoProdutos();
            List<ProdutoDisplay> produto = new List<ProdutoDisplay>();
            produto = _repo.Tudo();
            return Json(produto, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult validaUsuario(string email, string senha)
        {
            Cliente cliente = new Cliente();
            BDConexao conexao = new BDConexao();

            conexao.connection.Open();
            conexao.command.CommandText = "SELECT idCliente FROM CLIENTE WHERE EMAIL = @EMAIL AND SENHA = @SENHA";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = email;
            conexao.command.Parameters.Add("@SENHA", SqlDbType.VarChar).Value = StatueStoreEncrypt.Encrypt(senha);

            var result = conexao.command.ExecuteScalar();
            int id;

            Console.WriteLine(result);
            if (result != null && Int32.TryParse(result.ToString(), out id))
            {
                cliente.GetClientById(id);
                return Json(cliente);
            }
            else
            {
                return Json("false");
            }
        }

        [HttpPost]
        public JsonResult cadastraUsuario(string email, string nome, string sobrenome, string senha, string cpf, string sexo, string dataNascimento)
        {
            Cliente cliente = new Cliente();
            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.CommandText = "INSERT INTO CLIENTE OUTPUT INSERTED.IDCLIENTE VALUES(@EMAIL, @SENHA, @NOME, @SOBRENOME, @SEXO, @CPF, @DATANASC, GETDATE(), null)"; conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = email;
            conexao.command.Parameters.Add("@SENHA", SqlDbType.VarChar).Value = StatueStoreEncrypt.Encrypt(senha);
            conexao.command.Parameters.Add("@NOME", SqlDbType.VarChar).Value = nome;
            conexao.command.Parameters.Add("@SOBRENOME", SqlDbType.VarChar).Value = sobrenome;
            conexao.command.Parameters.Add("@SEXO", SqlDbType.VarChar).Value = sexo;
            conexao.command.Parameters.Add("@CPF", SqlDbType.VarChar).Value = cpf;
            conexao.command.Parameters.Add("@DATANASC", SqlDbType.VarChar).Value = dataNascimento.Replace("/", "-");

            try
            {
                var id = conexao.command.ExecuteScalar();
                Console.WriteLine(id);
                conexao.connection.Close();
                if (id == null)
                {
                    return Json(null);
                }
                else
                {
                    cliente.GetClientById(Convert.ToInt32(id));
                    return Json(cliente);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetHashCode());
                return Json(null);
            }
        }

        [HttpPost]
        public JsonResult cadastraProduto(string nome, string imagem, string imagemModelo, string hexaCor, string sexo, int idSubgrupo, int idCliente)
        {
            Cliente cliente = new Cliente();
            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.CommandText = "EXECUTE cadastraProdutoPersonalizado @NOME, @IMAGEM, @MODELO, @COR, @SEXO, @SUBGRUPO, @CLIENTE";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@NOME", SqlDbType.VarChar).Value = nome;
            conexao.command.Parameters.Add("@IMAGEM", SqlDbType.VarChar).Value = imagem;
            conexao.command.Parameters.Add("@MODELO", SqlDbType.VarChar).Value = imagemModelo;
            conexao.command.Parameters.Add("@COR", SqlDbType.Char).Value = hexaCor;
            conexao.command.Parameters.Add("@SEXO", SqlDbType.Char).Value = sexo;
            conexao.command.Parameters.Add("@SUBGRUPO", SqlDbType.Int).Value = idSubgrupo;
            conexao.command.Parameters.Add("@CLIENTE", SqlDbType.Int).Value = idCliente;

            try
            {
                var id = conexao.command.ExecuteScalar();
                conexao.connection.Close();

                if (id == null)
                {
                    return Json(null);
                }

                return Json(new
                {
                    Id = id,
                    Nome = nome,
                    Imagem = imagem,
                    ImgModelo = imagemModelo,
                    Cor = hexaCor,
                    Sexo = sexo,
                    Subgrupo = idSubgrupo,
                    Cliente = idCliente
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return Json(null);
            }
        }

        [HttpGet]
        public JsonResult listaProduto(int idCliente)
        {
            List<ProdutoPersonalizado> produtos = new List<ProdutoPersonalizado>();
            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.CommandText = "SELECT * FROM ProdutoPersonalizado WHERE idCliente = @IDCLIENTE";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@IDCLIENTE", SqlDbType.Int).Value = idCliente;

            SqlDataReader dr = conexao.command.ExecuteReader();
            while (dr.Read())
            {
                ProdutoPersonalizado prod = new ProdutoPersonalizado();
                prod.Id = (int)dr["idProdutoPersonalizado"];
                prod.Nome = dr["nome"].ToString();
                prod.Imagem = dr["imagem"].ToString();
                prod.ImgModelo = dr["imagemModelo"].ToString();
                prod.Cor = dr["hexaCor"].ToString();
                prod.Sexo = dr["sexo"].ToString();
                prod.Subgrupo = (int)dr["idSubgrupo"];
                prod.Cliente = (int)dr["idCliente"];

                produtos.Add(prod);
            }
            conexao.connection.Close();

            return Json(produtos, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public Boolean deleteProduto(int idProdutoPersonalizado)
        {
            BDConexao conexao = new BDConexao();
            conexao.connection.Open();
            conexao.command.CommandText = "DELETE FROM ProdutoPersonalizado WHERE idProdutoPersonalizado = @idProduto";
            conexao.command.Parameters.Clear();
            conexao.command.Parameters.Add("@idProduto", SqlDbType.Int).Value = idProdutoPersonalizado;

            try
            {
                conexao.command.ExecuteNonQuery();
                conexao.connection.Close();
                return true;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.StackTrace);
                conexao.connection.Close();
                return false;
            }
        }
    }
}