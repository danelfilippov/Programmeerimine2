using System.Threading.Tasks;
using KooliProjekt.Application.Features.TodoLists;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class TournamentsController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public TournamentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var query = new TournamentsQuery();
            var result = await _mediator.Send(query);

            return Result(result);
        }
    }
}
