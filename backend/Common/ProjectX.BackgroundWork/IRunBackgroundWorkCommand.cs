using MediatR;

namespace ProjectX.BackgroundWork;

public interface IRunBackgroundWorkCommand : IRequest<Unit>
{

}

public class RunBackgroundWorkCommand<TType, TPayload> : IRunBackgroundWorkCommand
    where TType : BackgroundWorkTypes<TType>
    where TPayload : IBackgroundWorkPayload<TType>
{
    public required BackdroundWorkEntity<TType> Work { get; init; }

    public required TPayload Payload { get; init; }
}