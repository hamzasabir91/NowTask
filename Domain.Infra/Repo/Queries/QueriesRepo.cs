using Domain.IRepository.Command;
using System.Data;
using Domain.IRepository.Queries;
using Dapper;
using Microsoft.Data.SqlClient;
using Authentication.Application.Common.Exceptions;

namespace Infra.Repo.Queries
{
    
    public class QueriesRepo<T> : IQueriesRepo<T>
    {
        private readonly IDbConnection _db;

        public QueriesRepo(string connectionString)
        {
            _db = new SqlConnection(connectionString);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            try
            {
                var query = $"SELECT * FROM {typeof(T).Name}s WHERE Id = @Id";
                return await _db.QueryFirstOrDefaultAsync<T>(query, new { Id = id });
            }
            catch (Exception ex) {
                throw new BadRequestException("Error getting data: " + ex.Message);
            }

        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                var query = $"SELECT * FROM {typeof(T).Name}s";
                return await _db.QueryAsync<T>(query);
            }
            catch (Exception ex)
            {
                throw new BadRequestException("Error getting data: " + ex.Message);
            }
        }

      
    }
}
