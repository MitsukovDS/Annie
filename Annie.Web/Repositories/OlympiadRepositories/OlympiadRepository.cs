using Annie.Data.DatabaseProvider;
using Annie.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Annie.Web.Repositories.OlympiadRepositories
{
    public class OlympiadRepository : IOlympiadRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public OlympiadRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<List<Olympiad>> GetOlympiadsAsync(DateTime startDate)
        {
            using IDbConnection dbConnection = _dbConnectionFactory.CreateConnection();
            #region query
            string query = @"SELECT o.*
                             FROM public.""Olympiad"" o
                             WHERE o.""StartDate"" >= @startDate;";

            return (await dbConnection.QueryAsync<Olympiad>(query, new
            {
                startDate
            })).ToList();
            #endregion
        }

        public async Task<List<Olympiad>> GetOlympiadsAsync(OlympiadTypes olympiadType, DateTime startDate)
        {
            using IDbConnection dbConnection = _dbConnectionFactory.CreateConnection();
            #region query
            string query = @"SELECT o.*
                             FROM public.""Olympiad"" o
                             WHERE o.""OlympiadTypeId"" = @olympiadTypeId AND o.""StartDate"" >= @startDate;";

            return (await dbConnection.QueryAsync<Olympiad>(query, new
            {
                olympiadTypeId = (int)olympiadType,
                startDate
            })).ToList();
            #endregion
        }

    }
}
