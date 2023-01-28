using ProjectX.Core.Setup;
using System.ComponentModel.DataAnnotations;

namespace ProjectX.FileStorage.Database.Setup;

public class MongoConfig : IApplicationConfig
{
    [Required]
    public string ConnectionString { get; set; }
    
    [Required]
    public string DatabaseName { get; set; }

    [Required]
    public string[] Collections { get; set; }
}