namespace mydental.application.Common
{
    public class ServiceResult<T>
    {
        public int StatusCode { get; private set; }
        public string Message { get; private set; }
        public List<string>? ErrorMessages { get; private set; } // ✅ Array of error messages
        public T? Data { get; private set; }

        private ServiceResult(int statusCode, string message, List<string>? errors, T? data)
        {
            StatusCode = statusCode;
            Message = message;
            ErrorMessages = errors;
            Data = data;
        }

        // ✅ Success responses
        public static ServiceResult<T> Success(T data, string message)
        {
            return new ServiceResult<T>(200, message, null, data);
        }

        public static ServiceResult<T> Created(T data, string message)
        {
            return new ServiceResult<T>(201, message, null, data);
        }

        // ❌ Validation errors
        public static ServiceResult<T> BadRequest(string message, List<string> errors)
        {
            return new ServiceResult<T>(400, message, errors, default);
        }
        public static ServiceResult<T> Unauthorized(string message, List<string> errors)
        {
            return new ServiceResult<T>(401, message, errors, default);
        }

        public static ServiceResult<T> Forbidden(string message, List<string> errors)
        {
            return new ServiceResult<T>(403, message, errors, default);
        }
        public static ServiceResult<T> NotFound(string message, List<string> errors)
        {
            return new ServiceResult<T>(404, message, errors, default);
        }

        public static ServiceResult<T> Conflict(string message, List<string> errors)
        {
            return new ServiceResult<T>(409, message, errors, default);
        }

        public static ServiceResult<T> Error(string message, List<string> errors)
        {
            return new ServiceResult<T>(500, message, errors, default);
        }


    }
}
