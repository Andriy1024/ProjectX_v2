﻿namespace ProjectX.Identity.Authentication.DTO.Incoming;

public class RegistrationRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
