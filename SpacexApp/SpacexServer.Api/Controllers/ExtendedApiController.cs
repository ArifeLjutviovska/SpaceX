namespace SpacexServer.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SpacexServer.Api.Common.Models;

    /// <summary>
    /// Base API controller that provides utility methods for returning standardized API responses.
    /// Ensures consistent handling of successful and error responses across all controllers.
    /// </summary>
    public class ExtendedApiController : ControllerBase
    {

        /// <summary>
        /// Returns an HTTP response based on the operation result.
        /// If the operation fails, returns an error response with the appropriate HTTP status code.
        /// If successful, returns an HTTP 200 OK response with the result.
        /// </summary>
        /// <param name="result">The operation result containing success or failure data.</param>
        /// <returns>An `IActionResult` representing the HTTP response.</returns>
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

        /// <summary>
        /// Returns an HTTP response based on the operation result, with a typed result.
        /// If the operation fails, returns an error response with the appropriate HTTP status code.
        /// If successful, returns an HTTP 200 OK response with the result data.
        /// </summary>
        /// <typeparam name="T">The type of the result data.</typeparam>
        /// <param name="result">The operation result containing success or failure data.</param>
        /// <returns>An `IActionResult` representing the HTTP response.</returns>
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