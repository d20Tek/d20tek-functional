namespace D20Tek.Functional;

public readonly struct Error
{
    public int Type { get; }

    public string Code { get; }

    public string Message { get; }

    private Error(string code, string message, int errorType) => (Type, Code, Message) = (errorType, code, message);

    public static Error Unexpected(string code, string message) => new(code, message, ErrorType.Unexpected);

    public static Error Failure(string code, string message) => new(code, message, ErrorType.Failure);

    public static Error Validation(string code, string message) => new(code, message, ErrorType.Validation);

    public static Error NotFound(string code, string message) => new(code, message, ErrorType.NotFound);

    public static Error Conflict(string code, string message) => new(code, message, ErrorType.Conflict);

    public static Error Unauthorized(string code, string message) => new(code, message, ErrorType.Unauthorized);

    public static Error Forbidden(string code, string message) => new(code, message, ErrorType.Forbidden);

    public static Error Invalid(string code, string message) => new(code, message, ErrorType.Invalid);

    public static Error Exception(Exception ex) => new(Constants.GeneralExceptionCode, ex.Message, ErrorType.Unexpected);

    public static Error Create(string code, string message, int errorType) => new(code, message, errorType);

    public override string ToString() => $"Error ({Code} [{Type}]): {Message}";
}