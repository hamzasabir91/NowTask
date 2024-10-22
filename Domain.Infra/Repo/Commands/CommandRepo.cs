using Domain.IRepository.Command;
using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Authentication.Application.Common.Exceptions;

namespace Infra.Repo.Commands
{
    public class CommandRepo<T> : ICommandRepo<T>
    {
        private readonly IDbConnection _db;

        public CommandRepo(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            _db = new SqlConnection(connectionString);
        }
        public async Task AddAsync(T entity)
        {
            try
            {
                var properties = typeof(T).GetProperties()
                                          .Where(p => p.Name != "id");
                var columns = string.Join(", ", properties.Select(p => p.Name));
                var parameters = string.Join(", ", properties.Select(p => "@" + p.Name));
                var query = $"INSERT INTO {typeof(T).Name}s ({columns}) VALUES ({parameters})";

                await _db.ExecuteAsync(query, entity);
            }
            catch (Exception ex)
            {
                throw new BadRequestException("Error adding entity: " + ex.Message);
            }
        }

        public async Task UpdateAsync(T entity)
        {
            try
            {
                var properties = typeof(T).GetProperties()
                                          .Where(p => p.Name != "Id");
                var setClause = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));
                var query = $"UPDATE {typeof(T).Name}s SET {setClause} WHERE Id = @Id";

                await _db.ExecuteAsync(query, entity);
            }
            catch (Exception ex)
            {
                throw new BadRequestException("Error updating entity: " + ex.Message);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var query = $"DELETE FROM {typeof(T).Name}s WHERE Id = @Id";
                await _db.ExecuteAsync(query, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new BadRequestException("Error deleting entity: " + ex.Message);
            }
        }
    }
}

