namespace MemberService.Controllers.Members;

public sealed record CreateMemberRequest(string FirstName, string LastName, string Email);

public sealed record UpdateMemberRequest(Guid Id, string FirstName, string LastName, string Email);

public sealed record MemberResponse(Guid Id, string FirstName, string LastName, string Email);
