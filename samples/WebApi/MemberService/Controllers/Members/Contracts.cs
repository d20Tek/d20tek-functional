namespace MemberService.Controllers.Members;

public sealed record CreateMemberRequest(string FirstName, string LastName, string Email);

public sealed record UpdateMemberRequest(int Id, string FirstName, string LastName, string Email);

public sealed record MemberResponse(int Id, string FirstName, string LastName, string Email);
