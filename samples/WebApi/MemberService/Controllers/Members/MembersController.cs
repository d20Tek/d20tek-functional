using D20Tek.Functional;
using D20Tek.Functional.AspNetCore.WebApi;
using MemberService.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace MemberService.Controllers.Members;

[Route("api/v1/members")]
[ApiController]
public sealed class MembersController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(MemberResponse[]), StatusCodes.Status200OK)]
    [ActionName("GetAllMembers")]
    public ActionResult<MemberResponse[]> Get([FromServices] IMemberRepository repo) =>
        repo.GetEntities().Select(x => MemberMapper.Convert(x)).ToArray();

    [HttpGet("email/{email}")]
    [ProducesResponseType(typeof(MemberResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ActionName("GetMemberByEmail")]
    public ActionResult<MemberResponse> Get([FromRoute] string email, [FromServices] IMemberRepository repo) =>
        repo.GetByEmail(email).ToActionResult(MemberMapper.Convert, this);

    [HttpGet("{id:int}")]
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
        request.Validate()
            .Map(r => MemberEntity.Create(0, r.FirstName, r.LastName, r.Email))
            .Bind(entity => repo.Create(entity))
            .Pipe(result => result.ToCreatedActionResult(
                MemberMapper.Convert, this, "GetMemberById", GetRouteValuesForCreate(result)));

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(MemberResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ActionName("UpdateMember")]
    public ActionResult<MemberResponse> Put(
        [FromRoute] int id,
        [FromBody] UpdateMemberRequest request,
        [FromServices] IMemberRepository repo) =>
        request.Validate(id)
            .Bind(r => repo.Update(new(id, request.FirstName, request.LastName, request.Email)))
            .ToActionResult(MemberMapper.Convert, this);

    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(MemberResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ActionName("DeleteMember")]
    public ActionResult<MemberResponse> Delete([FromRoute] int id, [FromServices] IMemberRepository repo) =>
        repo.Delete(id).ToActionResult(MemberMapper.Convert, this);

    private static object? GetRouteValuesForCreate(IResultMonad result) =>
        (result.GetValue() is MemberEntity e) ? new { id = e.Id } : null;
}
