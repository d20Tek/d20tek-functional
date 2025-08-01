using D20Tek.Functional;
using System.Text.RegularExpressions;

namespace MemberService.Controllers.Members;

internal static class MemberValidator
{
    public static Result<CreateMemberRequest> Validate(this CreateMemberRequest request) =>
        new[]
        {
            ValidateField(!string.IsNullOrEmpty(request.FirstName), "Member.FirstName.Required", "Member first name is required."),
            ValidateField(!string.IsNullOrEmpty(request.LastName), "Member.Last.Required", "Member last name is required."),
            ValidateField(!string.IsNullOrEmpty(request.Email), "Member.Email.Required", "Member email is required."),
            ValidateField(request.Email.IsValidEmailFormat(), "Member.Email.Invalid", "Member email is not the expected format (name@company.com).")
        }.ConvertToResult(request);

    public static Result<UpdateMemberRequest> Validate(this UpdateMemberRequest request, int routeId) =>
        new[]
        {
            ValidateField(request.Id == routeId, "Member.Id.Mismatch", "The member Id in the request body must match the id in the endpoint route."),
            ValidateField(!string.IsNullOrEmpty(request.FirstName), "Member.FirstName.Required", "Member first name is required."),
            ValidateField(!string.IsNullOrEmpty(request.LastName), "Member.Last.Required", "Member last name is required."),
            ValidateField(!string.IsNullOrEmpty(request.Email), "Member.Email.Required", "Member email is required."),
            ValidateField(request.Email.IsValidEmailFormat(), "Member.Email.Invalid", "Member email is not the expected format (name@company.com).")
        }.ConvertToResult(request);

    private static Optional<Error> ValidateField(bool condition, string errorCode, string message) =>
        condition ? Optional<Error>.None() : Error.Validation(errorCode, message);

    private static readonly Regex _emailRegex =
        new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private static bool IsValidEmailFormat(this string text) =>
        !string.IsNullOrWhiteSpace(text) && _emailRegex.IsMatch(text);

    private static Result<T> ConvertToResult<T>(this IEnumerable<Optional<Error>> possibleErrors, T request)
        where T : notnull =>
        possibleErrors.Where(o => o.IsSome)
                      .Select(s => s.Get())
                      .ToArray() is var errors && errors.Length > 0
                        ? Result<T>.Failure(errors)
                        : Result<T>.Success(request);
}
