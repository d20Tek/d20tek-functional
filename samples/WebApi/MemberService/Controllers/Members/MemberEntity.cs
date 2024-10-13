using Apps.Repositories;

namespace MemberService.Controllers.Members;

public sealed class MemberEntity : IEntity
{
    public int Id { get; private set; }

    public string FirstName { get; }

    public string LastName { get; }

    public string Email { get; }

    public MemberEntity(int id, string firstName, string lastName, string email)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public void SetId(int id) => Id = id;
}
