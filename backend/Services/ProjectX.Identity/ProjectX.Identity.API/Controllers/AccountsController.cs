﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectX.Identity.API.Authentication;
using ProjectX.Identity.API.Database;
using ProjectX.Identity.API.Database.Models;
using ProjectX.Identity.API.Requests;
using ProjectX.Identity.API.Results;

namespace ProjectX.Identity.API.Controllers;

[Route("api/accounts")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly ProjectXIdentityDbContext _dbContext;
    private readonly UserManager<UserEntity> _userManager;
    private readonly JwtService _jwtService;

    public AccountsController(
        ProjectXIdentityDbContext dbContext, 
        UserManager<UserEntity> userManager,
        JwtService jwtService)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _jwtService = jwtService;
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetAccounts() 
    {
        var users = await _userManager.Users.ToListAsync();

        return Ok(users.Select(u => new 
        {
            u.Id,
            u.Email,
            u.EmailConfirmed,
            u.FirstName,
            u.LastName
        }));
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var userExist = await _userManager.FindByEmailAsync(request.Email);

        if(userExist == null) 
        {
            return BadRequest(AuthResult.Failed("User not found")); 
        }

        var isPasswordCorrect = await _userManager.CheckPasswordAsync(userExist, request.Password);

        if (isPasswordCorrect == false) 
        {
            return BadRequest(AuthResult.Failed("Invalid password"));
        }

        var authResult = await _jwtService.GenerateJwtToken(userExist);

        return authResult.IsSuccess
            ? Ok(authResult)
            : BadRequest(authResult);
    }

    [HttpPost]
    [Route("registration")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequest request) 
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(AuthResult.Failed("Invalid payload"));
        }

        var userExist = await _userManager.FindByEmailAsync(request.Email);
        if(userExist != null) 
        {
            return BadRequest(AuthResult.Failed("Email already in use."));
        }

        var newUser = new UserEntity() 
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
            return BadRequest(new 
            {
                Errors = isCreated.Errors.Select(e => e.Code).ToArray()
            });
        }

        var authResult = await _jwtService.GenerateJwtToken(newUser);

        return authResult.IsSuccess
            ? Ok(authResult)
            : BadRequest(authResult);
    }

    [HttpPost]
    [Route("refreshtoken")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        if (!ModelState.IsValid) 
        {
            return BadRequest(AuthResult.Failed("Invalid payload"));
        }

        var authResult = await _jwtService.VerifyAndGenerateToken(request);

        return authResult.IsSuccess
            ? Ok(authResult)
            : BadRequest(authResult);
    }
}