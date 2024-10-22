using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Now.Application.Dtos
{
    public class AuthenticateDto
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string token { get; set; }
    }
}
