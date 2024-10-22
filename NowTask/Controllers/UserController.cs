using Application.Commands.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Now.Application.Dtos;
using Now.Application.Queries.Authentication;
using Now.Application.Queries.User;

namespace NowTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("signup")]
        [ProducesDefaultResponseType(typeof(int))]
        public async Task<ActionResult> signup(CreateUserAsync command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpPost("authenticate")]
        [ProducesDefaultResponseType(typeof(AuthenticateDto))]
        public async Task<ActionResult> authenticate(authenticateAsync command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpGet("auth/balance")]
        [ProducesDefaultResponseType(typeof(decimal))]
        public async Task<ActionResult> Balance([FromQuery] UserBalanceAsync query)
        {
            return Ok(await _mediator.Send(query));
        }
    }
}
