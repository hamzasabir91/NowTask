using Authentication.Application.Common.Exceptions;
using AutoMapper;
using Domain.IRepository.Command;
using MediatR;
using Now.Application.Common.Interfaces;
using Now.Application.Dtos;
using Now.Domain.IRepository.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Now.Application.Queries.Authentication
{
    public class authenticateAsync : IRequest<AuthenticateDto>
    {
        public string username { get; set; }
        public string password { get; set; }
        public string browser { get; set; }
        public string device { get; set; }
        public string ipaddress { get; set; }
    }

    public class authenticateAsyncCommandHandler : IRequestHandler<authenticateAsync, AuthenticateDto>
    {
        private readonly IAuthenticationRepo _repo;
        private readonly IMapper _mapper;
        private readonly ITokenGenerator _tokenrepo;
        public authenticateAsyncCommandHandler(IAuthenticationRepo repo, IMapper mapper, ITokenGenerator tokenrepo)
        {
            _mapper = mapper;
            _repo = repo;
            _tokenrepo = tokenrepo;
        }
        public async Task<AuthenticateDto> Handle(authenticateAsync request, CancellationToken cancellationToken)
        {
            var res = await _repo.AuthenticatueUser(request.username, request.password);
            if (res != null)
            {
                var mappedres = _mapper.Map<AuthenticateDto>(res);
                if (mappedres != null)
                {
                    mappedres.token = _tokenrepo.GenerateJWTToken((res?.id, res?.username, request.ipaddress, request.browser));
                    return mappedres;
                }

                throw new BadRequestException("Error during Mapped");
            }
            else
            {
                throw new BadRequestException("Invalid username or password");
            }
        }
    }
}
