using KooliProjekt.Application.Infrastructure.Results;
using KooliProjekt.Application.Infrastructure.Results;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]    
    public abstract class ApiControllerBase : Controller
    {
        protected IActionResult Result(OperationResult result)
        {
            if (result.HasErrors)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        protected IActionResult Result<T>(OperationResult<T> result)
        {
            if (result.HasErrors)
            {
                return BadRequest(result);
            }

            if (result.Value == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}