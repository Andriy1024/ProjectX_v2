using Microsoft.AspNetCore.Mvc;

namespace ProjectX.Tasks.API.SeedWork;

/// <summary>
/// Internal server result, contains 500 status code.
/// </summary>
public class InternalServerErrorObjectResult : ObjectResult
{
    public InternalServerErrorObjectResult(object error) :
        base(error)
    {
        StatusCode = StatusCodes.Status500InternalServerError;
    }
}

/// <summary>
/// Forbidden result, contains 403 status code.
/// </summary>
public class ForbiddenObjectResult : ObjectResult
{
    public ForbiddenObjectResult(object error) :
        base(error)
    {
        StatusCode = StatusCodes.Status403Forbidden;
    }
}
