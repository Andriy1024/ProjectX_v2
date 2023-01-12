using ProjectX.Core.Setup;

namespace ProjectX.RabbitMq.Configuration
{
    public class RabbitMqConnectionConfiguration : IApplicationConfig
    {
        public string UserName { get; set; } = "guest";

        public string Password { get; set; } = "guest";

        public string VirtualHost { get; set; } = "/";

        public string HostName { get; set; } = "localhost";

        public string Port { get; set; } = "5672";
    }
}
