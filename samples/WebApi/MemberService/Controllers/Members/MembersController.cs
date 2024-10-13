using D20Tek.Functional;
using D20Tek.Functional.AspNetCore.WebApi;
using MemberService.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace MemberService.Controllers.Members;

[Route("api/v1/members")]
[ApiController]
public sealed class MembersController : ControllerBase
{
    [HttpGet("email/{email}")]
    [ProducesResponseType(typeof(MemberResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ActionName("GetMemberByEmail")]
    public ActionResult<MemberResponse> Get([FromRoute] string email, [FromServices] IMemberRepository repo) =>
        repo.GetByEmail(email).ToActionResult(MemberMapper.Convert, this);

    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(MemberResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ActionName("GetMemberById")]
    public ActionResult<MemberResponse> Get([FromRoute] int id, [FromServices] IMemberRepository repo) =>
        repo.GetEntityById(id).ToActionResult(MemberMapper.Convert, this);

    [HttpPost]
    [ProducesResponseType(typeof(MemberResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ActionName("CreateMember")]
    public ActionResult<MemberResponse> Post(
    [FromBody] CreateMemberRequest request,
    [FromServices] IMemberRepository repo) =>
    new MemberEntity(0, request.FirstName, request.LastName, request.Email).ToIdentity()
        .Map(entity => repo.Create(entity))
        .Map(result => result.ToCreatedActionResult(
            MemberMapper.Convert, this, "GetMemberById", new { id = result.GetValue() }));

    [HttpPut("{id:Guid}")]
    [ProducesResponseType(typeof(MemberResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ActionName("UpdateMember")]
    public ActionResult<MemberResponse> Put(
        [FromRoute] int id,
        [FromBody] UpdateMemberRequest request,
        [FromServices] IMemberRepository repo) =>
        repo.Update(new(id, request.FirstName, request.LastName, request.Email))
            .ToActionResult(MemberMapper.Convert, this);

    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(typeof(MemberResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ActionName("DeleteMember")]
    public ActionResult<MemberResponse> Delete([FromRoute] int id, [FromServices] IMemberRepository repo) =>
        repo.Delete(id).ToActionResult(MemberMapper.Convert, this);
}
