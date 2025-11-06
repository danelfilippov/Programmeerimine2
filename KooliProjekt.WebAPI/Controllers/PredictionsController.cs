using System.Threading.Tasks;
using KooliProjekt.Application.Features.TodoLists;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class PredictionsController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public PredictionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] PredictionsQuery query)
        {
            var result = await _mediator.Send(query);

            return Result(result);
        }
    }
}
