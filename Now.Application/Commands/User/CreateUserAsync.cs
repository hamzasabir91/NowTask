using Authentication.Application.Common.Exceptions;
using AutoMapper;
using Domain.IRepository.Command;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Commands.User
{
   
    public class CreateUserAsync : IRequest<int>
    {
        public string username { get; set; }
        public string password { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string device { get; set; }
        public string ipaddress { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserAsync, int>
    {
        private readonly ICommandRepo<Now.Domain.Entites.User> _repo;
        private readonly IMapper _mapper;
        public CreateUserCommandHandler(ICommandRepo<Now.Domain.Entites.User> repo,IMapper mapper)
        {
            _mapper = mapper;
            _repo =repo;
        }
        public async Task<int> Handle(CreateUserAsync request, CancellationToken cancellationToken)
        {
            try
            {
                await _repo.AddAsync(_mapper.Map<Now.Domain.Entites.User>(request));

                return 1;
            }
            catch(Exception ex)
            {
                throw new BadRequestException("During mappee exception occur"+ex.Message);
            }
        }
    }
}
