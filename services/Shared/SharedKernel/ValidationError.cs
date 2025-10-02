﻿namespace ElementLogiq.SharedKernel;

public sealed record ValidationError(Error[] Errors) : Error(
    "Validation.General",
    "One or more validation errors occurred",
    ErrorType.Validation)
{
    public static ValidationError FromResults(IEnumerable<Result> results)
    {
        return new ValidationError(
            [.. results.Where(r => r.IsFailure).Select(r => r.Error)]);
    }
}
