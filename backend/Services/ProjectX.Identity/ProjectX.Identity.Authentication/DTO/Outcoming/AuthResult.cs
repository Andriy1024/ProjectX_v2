﻿namespace ProjectX.Identity.Authentication.DTO.Outcoming;

public class AuthResult
{
    public string Token { get; set; }

    public string RefreshToken { get; set; }

    public bool IsSuccess { get; set; }

    public IEnumerable<string> Errors { get; set; }

    public static AuthResult Success(string token, string refreshToken) => new()
    {
        IsSuccess = true,
        Token = token,
        RefreshToken = refreshToken
    };

    public static AuthResult Failed(params string[] errors) => new() 
    {
        IsSuccess = false,
        Errors = errors
    };
}