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

public interface IBackgroundWorkPayload<TType>
    where TType : BackgroundWorkTypes<TType>
{
   abstract static TType GetWorkType();
}

public abstract class BackgroundWorkTypes<T> where T : BackgroundWorkTypes<T>
{
    public abstract RunBackgroundWorkCommand<T> GetPayload(BackdroundWorkEntity<T> work);
}

public class RunBackgroundWorkCommand<TType>
    where TType : BackgroundWorkTypes<TType>
{
    public required BackdroundWorkEntity<TType> Work { get; init; }

    public required IBackgroundWorkPayload<TType> Payload { get; init; }
}

public class TestPayload : IBackgroundWorkPayload<TestTypes>
{
    public static TestTypes GetWorkType() => TestTypes.One;
}

public abstract class TestTypes : BackgroundWorkTypes<TestTypes> 
{
    public static readonly TestTypes One = new TypeOne();

    public class TypeOne : TestTypes
    {
        public override RunBackgroundWorkCommand<TestTypes> GetPayload(BackdroundWorkEntity<TestTypes> work)
        {
            var command = new RunBackgroundWorkCommand<TestTypes>()
            {
                Work = work,
                Payload = new TestPayload()
            };

            return command;
        }
    }
}