using ProjectX.Core.Setup;

namespace ProjectX.FileStorage.Persistence.Database.Setup;

public class MongoOptions : IApplicationConfig
{
    public string DatabaseName { get; set; }

    public string[] Collections { get; set; }
}