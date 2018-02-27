using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_cpnucleo_pages.Repository2.Projeto
{
    public class ProjetoRepository : Context, IRepository<ProjetoItem>
    {
        public ProjetoRepository(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task Incluir(ProjetoItem projeto)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                projeto.DataInclusao = DateTime.Now;

                string query = @"INSERT INTO CPN_TB_PROJETO
                                (
                                    PROJ_NOME,
                                    PROJ_DATA_INCLUSAO,
                                    SIS_ID
                                )
                                VALUES
                                (
                                    @Nome,
                                    @DataInclusao,
                                    @IdSistema
                                )";

                dbConnection.Open();
                await dbConnection.ExecuteAsync(query, projeto);
            }
        }

        public async Task Alterar(ProjetoItem projeto)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                projeto.DataAlteracao = DateTime.Now;

                string query = @"UPDATE CPN_TB_PROJETO SET 
                                    PROJ_NOME = @Nome, 
                                    PROJ_DATA_ALTERACAO= @DataAlteracao,
                                    SIS_ID = @IdSistema
                                WHERE PROJ_ID = @IdProjeto;";

                dbConnection.Open();
                await dbConnection.ExecuteAsync(query, projeto);
            }
        }

        public async Task<ProjetoItem> Consultar(int idProjeto)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = @"SELECT 
                                    PROJETO.PROJ_ID AS IdProjeto,
                                    PROJETO.PROJ_NOME AS Nome,
                                    PROJETO.PROJ_DATA_INCLUSAO AS DataInclusao,
                                    SISTEMA.SIS_ID AS Sistema.IdSistema,
                                    SISTEMA.SIS_NOME AS Sistema.Nome
                                FROM 
                                    CPN_TB_PROJETO PROJETO
                                INNER JOIN 
                                    CPN_TB_SISTEMA SISTEMA ON PROJETO.SIS_ID = SISTEMA.SIS_ID
                                WHERE 
                                    PROJETO.PROJ_ID = @idProjeto";

                dbConnection.Open();
                return await dbConnection.QueryFirstOrDefaultAsync<ProjetoItem>(query, new { idProjeto });
            }
        }

        public async Task<IEnumerable<ProjetoItem>> Listar()
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = @"SELECT 
                                    PROJETO.PROJ_ID AS IdProjeto,
                                    PROJETO.PROJ_NOME AS Nome,
                                    PROJETO.PROJ_DATA_INCLUSAO AS DataInclusao,
                                    SISTEMA.SIS_ID AS Sistema.IdSistema,
                                    SISTEMA.SIS_NOME AS Sistema.Nome
                                FROM 
                                    CPN_TB_PROJETO PROJETO
                                INNER JOIN 
                                    CPN_TB_SISTEMA SISTEMA ON PROJETO.SIS_ID = SISTEMA.SIS_ID";

                dbConnection.Open();
                return await dbConnection.QueryAsync<ProjetoItem>(query);
            }
        }

        public async Task Remover(ProjetoItem projeto)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = @"DELETE FROM CPN_TB_PROJETO
                                WHERE PROJ_ID = @IdProjeto;";

                dbConnection.Open();
                await dbConnection.ExecuteAsync(query, new { projeto.IdProjeto });
            }
        }
    }
}