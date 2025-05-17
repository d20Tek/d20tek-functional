using D20Tek.Functional;
using D20Tek.LowDb.Repositories;
using MemberService.Controllers.Members;

namespace MemberService.Common;

public interface IMemberRepository : IRepository<MemberEntity>
{
    Result<MemberEntity> GetByEmail(string email);
}
