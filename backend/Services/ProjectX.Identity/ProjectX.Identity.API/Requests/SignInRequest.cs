﻿using System.ComponentModel.DataAnnotations;

namespace ProjectX.Identity.API.Requests;

public class SignInRequest
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}