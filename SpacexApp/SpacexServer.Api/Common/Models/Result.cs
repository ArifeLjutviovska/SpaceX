﻿namespace SpacexServer.Api.Common.Models
{
    using SpacexServer.Api.Common.Enums;

    /// <summary>
    /// Represents the outcome of an operation, encapsulating success or failure states.
    /// Provides predefined static methods for common result types.
    /// </summary>
    public class Result : ResultCommonLogic
    {
        private Result()
            : base(ResultType.Ok, isFailure: false, string.Empty)
        {
        }

        internal Result(ResultType resultType, string message)
            : base(resultType, isFailure: true, message)
        {
        }

        public static Result Ok()
        {
            return new Result();
        }

        public static Result Conflicted(string message)
        {
            return new Result(ResultType.Conflicted, message);
        }

        public static Result Failed(string message)
        {
            return new Result(ResultType.InternalError, message);
        }

        public static Result Forbidden(string message)
        {
            return new Result(ResultType.Forbidden, message);
        }

        public static Result Invalid(string message)
        {
            return new Result(ResultType.Invalid, message);
        }

        public static Result NotFound(string message)
        {
            return new Result(ResultType.NotFound, message);
        }

        public static Result Unauthorized(string message)
        {
            return new Result(ResultType.Unauthorized, message);
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value);
        }

        public static Result<T> Conflicted<T>(string message)
        {
            return new Result<T>(ResultType.Conflicted, message);
        }

        public static Result<T> Failed<T>(string message)
        {
            return new Result<T>(ResultType.InternalError, message);
        }

        public static Result<T> Forbidden<T>(string message)
        {
            return new Result<T>(ResultType.Forbidden, message);
        }

        public static Result<T> Invalid<T>(string message)
        {
            return new Result<T>(ResultType.Invalid, message);
        }

        public static Result<T> NotFound<T>(string message)
        {
            return new Result<T>(ResultType.NotFound, message);
        }

        public static Result<T> Unauthorized<T>(string message)
        {
            return new Result<T>(ResultType.Unauthorized, message);
        }

        public static Result<T> FromError<T>(ResultCommonLogic result)
        {
            return new Result<T>(result.ResultType, result.Message);
        }

        /// <summary>
        /// Returns the first failure from a list of results, or a success if none failed.
        /// </summary>
        /// <param name="results">An array of results.</param>
        public static Result FirstFailureOrOk(params Result[] results)
        {
            if (results.Any((Result f) => f.IsFailure))
            {
                return results.First((Result f) => f.IsFailure);
            }

            return Ok();
        }

        /// <summary>
        /// Returns the first failure from a list of generic results, or a success if none failed.
        /// </summary>
        /// <typeparam name="T">The type of the result value.</typeparam>
        /// <param name="results">An enumerable of generic results.</param>
        public static Result FirstFailureOrOk<T>(IEnumerable<Result<T>> results)
        {
            if (results.Any((Result<T> x) => x.IsFailure))
            {
                return results.First((Result<T> x) => x.IsFailure);
            }

            return Ok();
        }
    }

    /// <summary>
    /// Represents the outcome of an operation that returns a value of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value returned by the operation.</typeparam>
    public class Result<T> : ResultCommonLogic
    {
        public bool IsEmpty
        {
            get
            {
                T value = Value;
                if (value == null)
                {
                    return true;
                }

                return value.Equals(Empty);
            }
        }

        public T Value { get; }

        private static T Empty => default(T);

        internal Result(ResultType resultType, string message)
            : base(resultType, isFailure: true, message)
        {
            Value = Empty;
        }

        internal Result(T value)
            : base(ResultType.Ok, isFailure: false, string.Empty)
        {
            Value = value;
        }

        public static implicit operator T(Result<T> result)
        {
            return result.Value;
        }

        public static implicit operator Result(Result<T> result)
        {
            if (result.IsSuccess)
            {
                return Result.Ok();
            }

            return new Result(result.ResultType, result.Message);
        }
    }
}