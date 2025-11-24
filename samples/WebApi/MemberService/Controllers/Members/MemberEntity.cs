namespace MemberService.Controllers.Members;

public sealed class MemberEntity(int id, string firstName, string lastName, string email)
{
    public int Id { get; private set; } = id;

    public string FirstName { get; private set; } = firstName;

    public string LastName { get; private set; } = lastName;

    public string Email { get; private set; } = email;

    public void SetId(int id) => Id = id;

    public MemberEntity Update(string firstName, string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;

        return this;
    }

    public static MemberEntity Create(int id, string firstName, string lastName, string email) =>
        new(id, firstName, lastName, email);

    public static MemberEntity Default => new(0, string.Empty, string.Empty, string.Empty);
}
