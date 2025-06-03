using Microsoft.AspNetCore.Mvc;
using NHibernate;

namespace API.Controllers.HealthCheck
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthCheckController(ISessionFactory sessionFactory) : ControllerBase
    {
        [HttpGet]
        [Route("live")]
        public IActionResult Live()
        {
            return Ok(new
            {
                Status = "Serviço Ativo",
                Time = DateTime.UtcNow
            });
        }

        [HttpGet]
        [Route("ready")]
        public IActionResult Ready()
        {
            bool conectado = CheckConexaoBancoDados();

            if (conectado)
            {
                return Ok(new
                {
                    Status = "Conexao externas realizadas com sucesso",
                    Time = DateTime.UtcNow
                });
            }

            return StatusCode(503, new
            {
                Status = "Sem Conexao externas realizadas com sucesso",
                Time = DateTime.UtcNow,
                Issues = new
                {
                    Database = conectado ? "Healthy" : "Unavailable",
                }
            });
        }

        private bool CheckConexaoBancoDados()
        {
            try
            {
                using var session = sessionFactory.OpenSession();
                session.CreateSQLQuery("SELECT 1").UniqueResult();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
