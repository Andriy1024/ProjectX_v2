namespace ProjectX.BackgroundWork;

public abstract class BackgroundWorkTypes<T> where T : BackgroundWorkTypes<T>
{
    public abstract IRunBackgroundWorkCommand GetPayload(BackdroundWorkEntity<T> work);
}