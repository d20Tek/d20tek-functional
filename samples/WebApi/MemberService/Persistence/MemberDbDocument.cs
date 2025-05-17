using D20Tek.LowDb.Repositories;
using MemberService.Controllers.Members;

namespace MemberService.Persistence;

public class MemberDbDocument : DbDocument
{
    public HashSet<MemberEntity> Members { get; set; } = [];
}
