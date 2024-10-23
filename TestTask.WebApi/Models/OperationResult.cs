namespace TestTask.WebApi.Models
{
    public class OperationResult<T>
    {
        public bool IsSuccess { get; private set; }
        public string? Message { get; private set; }
        public T? Data { get; private set; }
        public static OperationResult<T> Success(T data, string? message = null)
        {
            return new OperationResult<T>
            {
                IsSuccess = true,
                Data = data,
                Message = message
            };
        }

        // Factory method for a failed operation
        public static OperationResult<T> Failure(string errorMessage)
        {
            return new OperationResult<T>
            {
                IsSuccess = false,
                Data = default,
                Message = errorMessage
            };
        }
    }
}
