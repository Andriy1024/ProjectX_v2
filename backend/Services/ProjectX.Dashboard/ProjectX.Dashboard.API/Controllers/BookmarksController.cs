using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectX.AspNetCore.Http;
using ProjectX.Dashboard.Application;

namespace ProjectX.Dashboard.API.Controllers;

[Route("api/bookmarks")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class BookmarksController : ProjectXController
{
    [HttpGet]
    public Task<IActionResult> GetBookmarks([FromQuery] BookmarksQuery query, CancellationToken cancellationToken)
        => Send(query, cancellationToken);

    [HttpGet("{id:long:min(1)}")]
    public Task<IActionResult> FindBookmark([FromRoute] int id, CancellationToken cancellationToken)
      => Send(new FindBookmarkQuery { Id = id }, cancellationToken);

    [HttpPost]
    public Task<IActionResult> CreateBookmark([FromBody] CreateBookmarkCommand command)
        => Send(command);

    [HttpPut]
    public Task<IActionResult> UpdateBookmark([FromBody] UpdateBookmarkCommand command)
        => Send(command);

    [HttpDelete("{id:long:min(1)}")]
    public Task<IActionResult> DeleteBookmark([FromRoute] int id)
        => Send(new DeleteBookmarkCommand(id));
}