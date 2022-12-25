namespace ProjectX.BackgroundWork;

public interface IBackgroundWorkPayload<TType>
    where TType : BackgroundWorkTypes<TType>
{
    abstract static TType GetWorkType();
}