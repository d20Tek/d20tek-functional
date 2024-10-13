using D20Tek.Functional;
using MemberService.Controllers.Members;

namespace MemberService.Persistence;

internal static class MemberRepositoryExtensions
{
    public static Result<MemberEntity> GetByEmail(this IMemberRepository repo, string email) =>
        repo.GetEntities().FirstOrDefault(p => p.Email == email) ??
            Result<MemberEntity>.Failure(
                Error.NotFound("Member.NotFound", $"Member entity with email={email} was not found."));
}
