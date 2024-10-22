using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Now.Application.Common.Interfaces
{
    public interface ITokenGenerator
    {
       
        public string GenerateJWTToken((int? userId, string userName, string ipaddress,string browser) userDetails);
        public string GetUserIdFromToken(string token);
    }
}
