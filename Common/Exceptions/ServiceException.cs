using System.Net;
using Common.Enums;

namespace Common.Exceptions;

public class ServiceException : Exception
{
    public HttpStatusCode StatusCode { get; set; }
    public ExceptionType Type { get; set; }
    public Dictionary<object, object> Args { get; set; }

    public ServiceException()
    {
        Type = ExceptionType.Unknown;
        StatusCode = HttpStatusCode.InternalServerError;

    }

    public ServiceException(ExceptionType type, string message, HttpStatusCode statusCode, params (object, object)[] args) : base(message)
    {
        StatusCode = statusCode;
        Type = type;
        Args = args.ToDictionary(x => x.Item1, x => x.Item2);
    }
}
