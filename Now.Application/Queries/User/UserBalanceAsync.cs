
using AutoMapper;
using MediatR;
using Now.Application.Common.Interfaces;
using Domain.IRepository.Queries;
using Now.Domain.IRepository.Queries;
using Authentication.Application.Common.Exceptions;

namespace Now.Application.Queries.User
{
    public class UserBalanceAsync : IRequest<decimal>
    {
        public string token { get; set; }
    }

    public class UserBalanceAsyncCommandHandler : IRequestHandler<UserBalanceAsync, decimal>
    {
       
        private readonly IBalanceQueryRepo _repo;
        private readonly IMapper _mapper;
        private readonly ITokenGenerator _tokenrepo;
        public UserBalanceAsyncCommandHandler(IBalanceQueryRepo repo, IMapper mapper, ITokenGenerator tokenrepo)
        {
            _mapper = mapper;
            _repo = repo;
            _tokenrepo = tokenrepo;
        }
        public async Task<decimal> Handle(UserBalanceAsync request, CancellationToken cancellationToken)
        {
          var userid=_tokenrepo.GetUserIdFromToken(request.token);
            var balance = await _repo.GetUserBalance(Convert.ToInt32(userid));
            if (balance!=null)
            {
                return balance.amount;
            }
            throw new NotFoundException("User Balance Not Exist");


        }
    }


   
}
