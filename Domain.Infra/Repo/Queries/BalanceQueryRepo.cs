using Authentication.Application.Common.Exceptions;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Now.Domain.Entites;
using Now.Domain.IRepository.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Now.Infra.Repo.Queries
{
    public class BalanceQueryRepo:IBalanceQueryRepo
    {
        private readonly IDbConnection _db;

        public BalanceQueryRepo(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            _db = new SqlConnection(connectionString);
        }
       
        public async Task<Balance> GetUserBalance(int userid)
        {
            try
            {
                var query = "SELECT * FROM Balance WHERE userid = @userid";


                var balance = await _db.QuerySingleOrDefaultAsync<Balance>(query, new { userid = userid });

                return balance;
            }
            catch (Exception ex)
            {
                throw new BadRequestException("Error getting balance: " + ex.Message);
            }
        }
    }
}
