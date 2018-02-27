using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace dotnet_cpnucleo_pages.Repository2.Sistema
{
    public class SistemaRepository : Context, IRepository<SistemaItem>
    {
        public SistemaRepository(IConfiguration configuration)
            : base(configuration)
        { }

        public async Task Incluir(SistemaItem sistema)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                sistema.DataInclusao = DateTime.Now;

                string query = @"INSERT INTO CPN_TB_SISTEMA
                                (
                                    SIS_NOME,
                                    SIS_DESCRICAO,
                                    SIS_DATA_INCLUSAO
                                )
                                VALUES
                                (
                                    @Nome,
                                    @Descricao,
                                    @DataInclusao
                                )";

                dbConnection.Open();
                await dbConnection.ExecuteAsync(query, sistema);
            }
        }

        public async Task Alterar(SistemaItem sistema)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                sistema.DataAlteracao = DateTime.Now;

                string query = @"UPDATE CPN_TB_SISTEMA SET 
                                    SIS_NOME = @Nome, 
                                    SIS_DESCRICAO= @Descricao,
                                    SIS_DATA_ALTERACAO = @DataAlteracao
                                WHERE SIS_ID = @IdSistema;";

                dbConnection.Open();
                await dbConnection.ExecuteAsync(query, sistema);
            }
        }

        public async Task<SistemaItem> Consultar(int idSistema)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = @"SELECT 
                                    SIS_ID AS IdSistema,
                                    SIS_NOME AS Nome,
                                    SIS_DESCRICAO AS Descricao,
                                    SIS_DATA_INCLUSAO AS DataInclusao
                                FROM 
                                    CPN_TB_SISTEMA
                                WHERE 
                                    SIS_ID = @idSistema";

                dbConnection.Open();
                return await dbConnection.QueryFirstOrDefaultAsync<SistemaItem>(query, new { idSistema });
            }
        }

        public async Task<IEnumerable<SistemaItem>> Listar()
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = @"SELECT 
                                    SIS_ID AS IdSistema,
                                    SIS_NOME AS Nome,
                                    SIS_DESCRICAO AS Descricao,
                                    SIS_DATA_INCLUSAO AS DataInclusao
                                FROM 
                                    CPN_TB_SISTEMA";

                dbConnection.Open();
                return await dbConnection.QueryAsync<SistemaItem>(query);
            }
        }

        public async Task Remover(SistemaItem sistema)
        {
            using (IDbConnection dbConnection = new SqlConnection(ConnectionString))
            {
                string query = @"DELETE FROM CPN_TB_SISTEMA
                                WHERE SIS_ID = @IdSistema;";

                dbConnection.Open();
                await dbConnection.ExecuteAsync(query, new { sistema.IdSistema });
            }
        }
    }
}