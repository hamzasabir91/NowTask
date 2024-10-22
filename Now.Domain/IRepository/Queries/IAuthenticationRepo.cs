using Now.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Now.Domain.IRepository.Queries
{
    public interface IAuthenticationRepo
    {
        Task<User> AuthenticatueUser(string username,string password);
    }
}
