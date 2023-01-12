using ProjectX.Core.Setup;

namespace ProjectX.Core.Setup;

public class ConnectionStrings : IApplicationConfig
{
    public string DbConnection { get; set; }
}
