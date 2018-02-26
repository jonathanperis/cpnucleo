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
        public SistemaRepository(IConfiguration configuration) : base(configuration) { }

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
            throw new NotImplementedException();
        }

        public async Task<SistemaItem> Consultar(int idSistema)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<SistemaItem>> Listar()
        {
            throw new NotImplementedException();
        }

        public async Task Remover(SistemaItem sistema)
        {
            throw new NotImplementedException();
        }
    }
}