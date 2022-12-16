using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectX.AspNetCore.Http;
using ProjectX.Dashboard.Application;

namespace ProjectX.Dashboard.API.Controllers;

[Route("api/notes")]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class NotesController : ProjectXController
{
    [HttpGet]
    public Task<IActionResult> GetNotes([FromQuery] NotesQuery query, CancellationToken cancellationToken)
        => Send(query, cancellationToken);

    [HttpGet("{id:long:min(1)}")]
    public Task<IActionResult> FindNote([FromRoute] int id, CancellationToken cancellationToken)
      => Send(new FindNoteQuery { Id = id }, cancellationToken);

    [HttpPost]
    public Task<IActionResult> CreateNote([FromBody] CreateNoteCommand command)
        => Send(command);

    [HttpPut]
    public Task<IActionResult> UpdateNote([FromBody] UpdateNoteCommand command)
        => Send(command);

    [HttpDelete]
    public Task<IActionResult> DeleteNote([FromBody] DeleteNoteCommand command)
        => Send(command);
}