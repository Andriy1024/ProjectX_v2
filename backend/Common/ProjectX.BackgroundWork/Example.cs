namespace ProjectX.BackgroundWork;

public class TestPayload : IBackgroundWorkPayload<TestTypes>
{
    public static TestTypes GetWorkType() => TestTypes.One;
}

public abstract class TestTypes : BackgroundWorkTypes<TestTypes>
{
    public static readonly TestTypes One = new TypeOne();

    public class TypeOne : TestTypes
    {
        public override IRunBackgroundWorkCommand GetPayload(BackdroundWorkEntity<TestTypes> work)
        {
            var command = new RunBackgroundWorkCommand<TestTypes, TestPayload>()
            {
                Work = work,
                Payload = new TestPayload()
            };

            return command;
        }
    }
}