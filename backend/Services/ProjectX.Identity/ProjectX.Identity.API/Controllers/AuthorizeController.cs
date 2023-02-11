using Microsoft.AspNetCore.Mvc;
using ProjectX.AspNetCore.Http;
using ProjectX.Identity.Application;

namespace ProjectX.Identity.API.Controllers;

[Route("api/auth")]
public class AuthorizeController : ProjectXController
{
    [HttpPost("sign-in")]
    [Consumes("application/x-www-form-urlencoded")]
    public Task<IActionResult> SignIn([FromForm] SignInCommand request)
    {
        return Send(request);
    }

    [HttpPost("sign-up")]
    [Consumes("application/x-www-form-urlencoded")]
    public Task<IActionResult> SignUp([FromForm] SignUpCommand request)
    {
        return Send(request);
    }

    [HttpPost("refresh-token")]
    [Consumes("application/x-www-form-urlencoded")]
    public Task<IActionResult> RefreshToken([FromForm] RefreshTokenCommand request)
    {
        return Send(request);
    }
}