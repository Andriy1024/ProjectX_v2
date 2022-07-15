using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectX.AspNetCore.Http;
using ProjectX.Core;
using ProjectX.Identity.API.Authentication;
using ProjectX.Identity.API.Database.Models;
using ProjectX.Identity.API.Requests;

namespace ProjectX.Identity.API.Controllers;

[Route("api/auth")]
public class AuthorizeController : ProjectXController
{
    private readonly UserManager<AccountEntity> _userManager;
    private readonly AuthorizationService _jwtService;

    public AuthorizeController(UserManager<AccountEntity> userManager, AuthorizationService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    [HttpPost("sign-in")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<IActionResult> SignIn([FromForm] SignInRequest request)
    {
        var userExist = await _userManager.FindByEmailAsync(request.Email);

        if (userExist == null)
        {
            return MapResponse(new ResultOf<Unit>(Error.NotFound(message: "User not found")));
        }

        var isPasswordCorrect = await _userManager.CheckPasswordAsync(userExist, request.Password);

        if (isPasswordCorrect == false)
        {
            return MapResponse(new ResultOf<Unit>(Error.InvalidData(message: "Invalid password")));
        }

        var authResult = await _jwtService.GenerateTokenAsync(userExist);

        return MapResponse(authResult);
    }

    [HttpPost("sign-up")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<IActionResult> SignUp([FromForm] SignUpRequest request)
    {
        if (!ModelState.IsValid)
        {
            return MapResponse(new ResultOf<Unit>(Error.InvalidData(message: "Invalid payload")));
        }

        var userExist = await _userManager.FindByEmailAsync(request.Email);
        if (userExist != null)
        {
            return MapResponse(new ResultOf<Unit>(Error.NotFound(message: "Email already in use.")));
        }

        var newUser = new AccountEntity()
        {
            Email = request.Email,
            UserName = request.Email,
            EmailConfirmed = false,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        var isCreated = await _userManager.CreateAsync(newUser, request.Password);
        if (isCreated.Succeeded == false)
        {
            var error = string.Join(", ", isCreated.Errors.Select(e => e.Code));

            return MapResponse(new ResultOf<Unit>(Error.InvalidData(message: error)));
        }

        var authResult = await _jwtService.GenerateTokenAsync(newUser);

        return MapResponse(authResult);
    }

    [HttpPost("refresh-token")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<IActionResult> RefreshToken([FromForm] RefreshTokenRequest request)
    {
        if (!ModelState.IsValid)
        {
            return MapResponse(new ResultOf<Unit>(Error.InvalidData(message: "Invalid payload")));
        }

        var authResult = await _jwtService.RefreshTokenAsync(request);

        return MapResponse(authResult);
    }
}