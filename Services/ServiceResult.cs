namespace Battleship_API.Services
{
    public class ServiceResult
    {
        public bool IsError { get; set; }
        public List<string> Errors { get; set; }
        public string ErrorsMessage => string.Join('|', Errors);

        public static ServiceResult WithSuccess()
        {
            return new ServiceResult
            {
                IsError = false,
                Errors = new List<string>()
            };
        }

        public static ServiceResult WithErrors(params string[] errors)
        {
            return new ServiceResult 
            { 
                IsError = true, 
                Errors = errors.ToList() 
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
