namespace VehicleRegistrationSystem.Results
{
    public class Result<T>
    {
        public T? Data { get; set; }

        public bool Success { get; set; }

        public string? Message { get; set; }

        public string? ErrorCode { get; set; }

        public List<string> Errors { get; set; } = new List<string>();

        public static Result<T> Ok() =>
        new Result<T> { Success = true };

        public static Result<T> Ok(T data) =>
            new Result<T> { Data = data, Success = true };

        public static Result<T> Ok(T data, string message)
        {
            return new Result<T> { Data = data, Success = true, Message = message };
        }

        public static Result<T> Fail(string errorCode, string message) =>
            new Result<T> {Success = false, ErrorCode = errorCode ,Message = message};

        public static Result<T> Fail(IEnumerable<string> errors)
        {
            return new Result<T>
            {
                Success = false,
                Errors = errors.ToList()
            };
        }

    }
}
