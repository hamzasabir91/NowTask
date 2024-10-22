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
    public class AuthenticationRepo:IAuthenticationRepo
    {
        private readonly IDbConnection _db;

        public AuthenticationRepo(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            _db = new SqlConnection(connectionString);
        }
       
        public async Task<User> AuthenticatueUser(string username, string password)
        {
            try
            {
                var query = "SELECT * FROM Users WHERE username = @username AND password = @password";


                var user = await _db.QuerySingleOrDefaultAsync<User>(query, new { username = username, password = password });

                return user;
            }
            catch(Exception ex)
            {
                throw new BadRequestException("Invalid username or password");
            }
        }
    }
}
