using Microsoft.AspNetCore.Mvc;
using Pit.Api.Dto;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Pit.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PitController : ControllerBase
    {
        [HttpPost("persistir-compra")]
        public IActionResult Persistir([FromBody] Compra compra)
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:4200");
            try
            {
                this.PersistirNoBanco(compra);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            return Ok();
        }

        [HttpGet("get-compras")]
        public IActionResult GetCompras()
        {
            try
            {
                var dados = PegarCompras();  
                return Ok(dados);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);  
            }
        }

        [HttpPost("login")]
        public IActionResult login(Login login)
        {
            try
            {
                var result = verificarLogin(login);
                return Ok(result);
            } 
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        private string PersistirNoBanco(Compra compra)
        {
            string connectionString = "Server=DESKTOP-6STAMVN;Database=Pit;User Id=sa;Password=123456;TrustServerCertificate=true;";

            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                string sql = @"INSERT INTO TBLCOMPRAS (Quantidade, Valor, Endereco, Telefone, Produto)
                      VALUES (@Quantidade, @Valor, @Endereco, @Telefone, @Produto)";

                dbConnection.Execute(sql, compra);

                return "Persistido com sucesso";
            }
        }

        private List<Compra> PegarCompras()
        {
            string connectionString = "Server=DESKTOP-6STAMVN;Database=Pit;User Id=sa;Password=123456;TrustServerCertificate=true;";

            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                string sql = @"SELECT 
                            [ID],
                            [QUANTIDADE],
                            [VALOR],
                            [ENDERECO],
                            [TELEFONE],
                            [PRODUTO]
                       FROM [TBLCOMPRAS]";

                List<Compra> compras = dbConnection.Query<Compra>(sql).ToList();

                return compras;
            }
        }

        private bool verificarLogin(Login login)
        {
            string connectionString = "Server=DESKTOP-6STAMVN;Database=Pit;User Id=sa;Password=123456;TrustServerCertificate=true;";

            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                string sql = @"SELECT 
                        USUARIO as Usuario,
                        SENHA as Senha
                    FROM 
                        [TBLADMSUSUARIOS]
                    WHERE 
                        USUARIO = @Usuario
                        AND SENHA = @Senha";

                List<Login> logins = dbConnection.Query<Login>(sql, new { Usuario = login.Usuario, Senha = login.Senha }).ToList();

                // Verifica se há pelo menos um registro correspondente
                return logins.Count > 0;
            }
        }





    }
}
