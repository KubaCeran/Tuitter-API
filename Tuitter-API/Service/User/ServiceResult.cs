﻿namespace Tuitter_API.Service
{
    public class ServiceResult
    {
        public bool IsError { get; protected set; }
        public List<string> Errors { get; protected set; }
        public string ErrorsMessage => string.Join(' ', Errors);

        protected ServiceResult() { }

        public static ServiceResult WithSuccess()
        {
            return new ServiceResult
            {
                IsError = false,
                Errors = new List<string>()
            };
        }

        public static ServiceResult WithErrors(params string[] errros)
        {
            return new ServiceResult
            {
                IsError = true,
                Errors = errros.ToList()
            };
        }
    }

    public class ServiceResult<T> : ServiceResult
    {
        public T Result { get; private set; }

        public static ServiceResult<T> WithSuccess(T result)
        {
            return new ServiceResult<T>
            {
                IsError = false,
                Errors = new List<string>(),
                Result = result
            };
        }

        public static new ServiceResult<T> WithErrors(params string[] errros)
        {
            return new ServiceResult<T>
            {
                IsError = true,
                Errors = errros.ToList()
            };
        }
    }
}
