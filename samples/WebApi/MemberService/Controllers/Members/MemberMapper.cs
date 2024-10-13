namespace MemberService.Controllers.Members;

internal sealed class MemberMapper
{
    public static MemberResponse Convert(MemberEntity member) =>
        new(member.Id, member.FirstName, member.LastName, member.Email);
}
