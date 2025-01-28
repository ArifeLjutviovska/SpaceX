namespace SpacexServer.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SpacexServer.Common.Models;

    public class ExtendedApiController : ControllerBase
    {
        protected IActionResult OkOrError<T>(Result<T> result)
        {
            IActionResult errorResponse = GetErrorResponse(result);

            if (errorResponse != null)
            {
                return errorResponse;
            }

            return Ok(result);
        }

        protected IActionResult OkOrError(ResultCommonLogic result)
        {
            IActionResult errorResponse = GetErrorResponse(result);

            if (errorResponse != null)
            {
                return errorResponse;
            }

            return Ok(result);
        }

        private IActionResult GetErrorResponse(ResultCommonLogic result)
        {
            if (result.IsFailure)
            {
                IActionResult errorResponse = new ObjectResult(result)
                {
                    DeclaredType = typeof(ResultCommonLogic),
                    StatusCode = (int)result.HttpStatusCode
                };

                return errorResponse;
            }

            return null;
        }
    }
}