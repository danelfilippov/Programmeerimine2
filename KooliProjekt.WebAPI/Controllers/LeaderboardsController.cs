using System.Threading.Tasks;
using KooliProjekt.Application.Features.TodoLists;
using KooliProjekt.Application.Features.ToDoLists;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class LeaderboardsController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public LeaderboardsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List([FromQuery] LeaderboardsQuery query)
        {
            var response = await _mediator.Send(query);

            return Result(response);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetLeaderboardsQuery { Id = id };
            var response = await _mediator.Send(query);

            return Result(response);
        }

        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(SaveLeaderboardsCommand command)
        {
            var response = await _mediator.Send(command);

            return Result(response);
        }
    }
}
