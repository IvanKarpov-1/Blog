using Blog.API.Extensions;
using Blog.BLL.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        private IMediator _mediator;

        protected IMediator Mediator => _mediator ??= 
            HttpContext.RequestServices.GetService<IMediator>();

        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if (result == null) return NotFound();
            switch (result.IsSuccess)
            {
                case true when result.Value != null:
                    return Ok(result.Value);
                case true when result.Value == null:
                    return NotFound();
                case false when result.Value == null && result.Error == string.Empty:
                    return Unauthorized();
                case false when result.ValidationError:
                    ModelState.AddModelError("error", result.Error);
                    return ValidationProblem(ModelState);
                default:
                    return BadRequest();
            }
        }

        protected ActionResult HandlePagedResult<T>(Result<PagedList<T>> result)
        {
            if (result == null) return NotFound();
            switch (result.IsSuccess)
            {
                case true when result.Value != null:
                {
                    Response.AddPaginationHeader(result.Value.CurrentPage, result.Value.PageSize,
                        result.Value.TotalCount, result.Value.TotalPages);
                    return Ok(result.Value);
                }
                case true when result.Value == null:
                    return NotFound();
                case false when result.Value == null && result.Error == string.Empty:
                    return Unauthorized();
                case false when result.ValidationError:
                    ModelState.AddModelError("error", result.Error);
                    return ValidationProblem(ModelState);
                default:
                    return BadRequest();
            }
        }
    }
}
