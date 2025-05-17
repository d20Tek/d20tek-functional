using D20Tek.Functional;
using D20Tek.LowDb;
using D20Tek.LowDb.Repositories;
using MemberService.Common;
using MemberService.Controllers.Members;

namespace MemberService.Persistence;

internal class MemberRepository : LowDbRepository<MemberEntity, MemberDbDocument>, IMemberRepository
{
    public MemberRepository(LowDb<MemberDbDocument> db)
        : base(db, m => m.Members)
    {
    }

    public Result<MemberEntity> GetByEmail(string email) =>
        Find(p => p.Email == email)
            .Bind(m => m.FirstOrDefault() ??
                Result<MemberEntity>.Failure(
                    Error.NotFound("Member.NotFound", $"Member entity with email={email} was not found.")));
}
