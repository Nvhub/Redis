namespace Redis.Graphql.Utils
{
    public class ErrorHandler : IErrorFilter
    {
        public IError OnError(IError error)
        {
            return error.WithMessage(error.Exception.Message);
        }
    }
}
