﻿namespace ProjectX.Core.Setup;

public class BaseOptions : IApplicationConfig
{
    public string ApiName { get; set; }

    public string IdentityUrl { get; set; }

    public string ExternalIdentityUrl { get; set; }   
}
