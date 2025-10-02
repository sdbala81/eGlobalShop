﻿namespace ElementLogiq.SharedKernel;

#pragma warning disable CA1716
public record Error(string Code, string Description, ErrorType Type)
#pragma warning restore CA1716
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);

    public static readonly Error NullValue = new("General.Null", "Null value was provided", ErrorType.Failure);

    public static Error Failure(string code, string description)
    {
        return new Error(code, description, ErrorType.Failure);
    }

    public static Error NotFound(string code, string description)
    {
        return new Error(code, description, ErrorType.NotFound);
    }

    public static Error Problem(string code, string description)
    {
        return new Error(code, description, ErrorType.Problem);
    }

    public static Error Conflict(string code, string description)
    {
        return new Error(code, description, ErrorType.Conflict);
    }
}
