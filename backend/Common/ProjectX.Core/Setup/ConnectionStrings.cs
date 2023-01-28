using ProjectX.Core.Setup;
using System.ComponentModel.DataAnnotations;

namespace ProjectX.Core.Setup;

public class ConnectionStrings : IApplicationConfig
{
    [Required]
    public required string DbConnection { get; set; }
}