using System.Diagnostics.CodeAnalysis;

namespace ProjectX.BackgroundWork;

public class BackdroundWorkEntity<TType>
    where TType : BackgroundWorkTypes<TType>
{
    public required Guid Id { get; init; }

    public required TType Type { get; init; }

    [StringSyntax(StringSyntaxAttribute.Json)]
    public required string Payload { get; init; }
}