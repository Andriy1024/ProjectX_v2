using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectX.AspNetCore.Http;
using ProjectX.Core;
using ProjectX.Messenger.Application.UseCases;
using ProjectX.Messenger.Domain;

namespace ProjectX.Messenger.API.Controllers;

[Route("api/conversations")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ConversationsController : ProjectXController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResultOf<Unit>), 200)]
    public Task<IActionResult> SendMessageAsync([FromBody] SendMessageCommand command) 
        => Send(command);

    [HttpPut]
    [ProducesResponseType(typeof(ResultOf<Unit>), 200)]
    public Task<IActionResult> UpdateMessageAsync([FromBody] UpdateMessageCommand command) 
        => Send(command);

    [HttpDelete]
    [ProducesResponseType(typeof(ResultOf<Unit>), 200)]
    public Task<IActionResult> DeleteMessageAsync([FromBody] DeleteMessageCommand command) 
        => Send(command);

    [HttpGet]
    [ProducesResponseType(typeof(ResultOf<ConversationView>), 200)]
    public Task<IActionResult> GetConversationViewAsync([FromQuery] ConversationViewQuery query, CancellationToken cancellation) 
        => Send(query, cancellation);

    [HttpGet("my")]
    [ProducesResponseType(typeof(IEnumerable<UserConversationsView.Conversation>), 200)]
    public Task<IActionResult> GetConversationViewAsync(CancellationToken cancellation) 
        => Send(new UserConversationsQuery(), cancellation);
}