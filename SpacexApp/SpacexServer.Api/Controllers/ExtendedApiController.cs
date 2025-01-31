namespace SpacexServer.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SpacexServer.Api.Common.Models;

    public class ExtendedApiController : ControllerBase
    {
        protected IActionResult OkOrError(Result result)
        {
            if (result.IsFailure)
            {
                return new ObjectResult(result)
                {
                    DeclaredType = typeof(ResultCommonLogic),
                    StatusCode = (int)result.HttpStatusCode
                };
            }

            return Ok(result);
        }

        protected IActionResult OkOrError<T>(Result<T> result)
        {
            if (result.IsFailure)
            {
                return new ObjectResult(result)
                {
                    DeclaredType = typeof(ResultCommonLogic),
                    StatusCode = (int)result.HttpStatusCode
                };
            }

            return Ok(result);
        }
    }

}