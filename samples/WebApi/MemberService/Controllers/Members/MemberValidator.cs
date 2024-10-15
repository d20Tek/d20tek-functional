using D20Tek.Functional;

namespace MemberService.Controllers.Members;

internal static class MemberValidator
{

    public static Result<CreateMemberRequest> Validate(this CreateMemberRequest request) =>
        string.IsNullOrEmpty(request.FirstName)
            ? Result<CreateMemberRequest>.Failure(Error.Validation("Member.FirstName.Invalid", "Member first name is required."))
            : request;
}
