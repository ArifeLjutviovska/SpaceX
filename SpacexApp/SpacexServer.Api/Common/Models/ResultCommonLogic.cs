namespace SpacexServer.Api.Common.Models
{
    using SpacexServer.Api.Common.Enums;
    using System.Net;

    /// <summary>
    /// Represents the base class for operation results, providing a structured approach to handling success and failure cases.
    /// This class ensures that results are properly categorized and associated with relevant HTTP status codes.
    /// </summary>
    public abstract class ResultCommonLogic
    {
        private readonly string _message;

        public bool IsFailure { get; }

        public bool IsSuccess => !IsFailure;

        public bool IsNotFound
        {
            get
            {
                if (IsFailure)
                {
                    return HttpStatusCode == HttpStatusCode.NotFound;
                }

                return false;
            }
        }

        public string Message => _message;

        public ResultType ResultType { get; }

        public HttpStatusCode HttpStatusCode => ResultType switch
        {
            ResultType.Ok => HttpStatusCode.OK,
            ResultType.NotFound => HttpStatusCode.NotFound,
            ResultType.Forbidden => HttpStatusCode.Forbidden,
            ResultType.Conflicted => HttpStatusCode.Conflict,
            ResultType.Invalid => HttpStatusCode.NotAcceptable,
            ResultType.Unauthorized => HttpStatusCode.Unauthorized,
            _ => HttpStatusCode.InternalServerError,
        };

        protected ResultCommonLogic(ResultType resultType, bool isFailure, string message)
        {
            if (isFailure)
            {
                if (string.IsNullOrWhiteSpace(message))
                {
                    throw new ArgumentNullException("message", "There must be error message for failure.");
                }

                if (resultType == ResultType.Ok)
                {
                    throw new ArgumentException("There should be error type for failure.", "resultType");
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(message))
                {
                    throw new ArgumentException("There should be no error message for success.", "message");
                }

                if (resultType != ResultType.Ok)
                {
                    throw new ArgumentException("There should be no error type for success.", "resultType");
                }
            }

            ResultType = resultType;
            IsFailure = isFailure;
            _message = message;
        }
    }
}
