namespace ProjectX.Core;

public static class EnvironmentVariables
{
    public static string JAEGER_HOST { get; }

    public static int JAEGER_PORT { get; }

    public static string SEQ_URI { get; }

    static EnvironmentVariables() 
    {
        JAEGER_HOST = Environment.GetEnvironmentVariable("JAEGER_HOST") ?? "localhost";

        JAEGER_PORT = int.Parse(Environment.GetEnvironmentVariable("JAEGER_PORT") ?? "6831");

        SEQ_URI = Environment.GetEnvironmentVariable("SEQ_URI") ?? "http://localhost:5341";
    }
}