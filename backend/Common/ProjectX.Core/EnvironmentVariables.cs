namespace ProjectX.Core;

public static class EnvironmentVariables
{
    public static string JAEGER_URI { get; }

    public static string SEQ_URI { get; }

    static EnvironmentVariables() 
    {
        JAEGER_URI = Environment.GetEnvironmentVariable("JAEGER_URI") ?? "http://localhost:6831";

        SEQ_URI = Environment.GetEnvironmentVariable("SEQ_URI") ?? "http://localhost:5341";
    }
}