using D20Tek.LowDb.Repositories;
using MemberService.Controllers.Members;

namespace MemberService.Persistence;

internal class MemberDbDocument : DbDocument
{
    public HashSet<MemberEntity> Members { get; set; } = [];
}
