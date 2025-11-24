using D20Tek.Functional;
using D20Tek.LowDb;
using D20Tek.LowDb.Repositories;
using MemberService.Common;
using MemberService.Controllers.Members;

namespace MemberService.Persistence;

internal class MemberRepository(LowDb<MemberDbDocument> db) :
    LowDbRepository<MemberEntity, MemberDbDocument>(db, m => m.Members), IMemberRepository
{
    public Result<MemberEntity> GetByEmail(string email) =>
        Find(p => p.Email == email)
            .Bind(m => m.FirstOrDefault() ??
                Result<MemberEntity>.Failure(
                    Error.NotFound("Member.NotFound", $"Member entity with email={email} was not found.")));
}
