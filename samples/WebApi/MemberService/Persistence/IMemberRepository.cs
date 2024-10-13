global using IMemberRepository = Apps.Repositories.IRepository<MemberService.Controllers.Members.MemberEntity>;

using Apps.Repositories;
using MemberService.Controllers.Members;

namespace MemberService.Persistence;

internal sealed class TodoDataStore : DataStoreElement<MemberEntity>
{
}
