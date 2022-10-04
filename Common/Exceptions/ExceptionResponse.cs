namespace Common.Exceptions;

public class ExceptionResponse
{
    public int Code { get; set; }
    public string? Message { get; set; }
    public Dictionary<object, object>? Args { get; set; }
    public DevExceptionDetails? Dev { get; set; }
}