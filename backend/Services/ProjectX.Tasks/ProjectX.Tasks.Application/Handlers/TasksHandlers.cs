using AutoMapper;
using MediatR;
using ProjectX.Persistence;
using ProjectX.Tasks.Application;
using ProjectX.Tasks.Application.Contracts;
using ProjectX.Tasks.Domain.Entities;

namespace ProjectX.Tasks.Application.Handlers.Tasks;

public sealed class TasksHandlers :
    IQueryHandler<TasksQuery, IEnumerable<TaskContarct>>,
    ICommandHandler<CreateTaskCommand, TaskContarct>,
    ICommandHandler<UpdateTaskCommand, TaskContarct>,
    ICommandHandler<DeleteTaskCommand>
{
    private readonly IRepository<TaskEntity> _repository;
    private readonly IMapper _mapper;

    public TasksHandlers(IRepository<TaskEntity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResultOf<IEnumerable<TaskContarct>>> Handle(TasksQuery query, CancellationToken cancellationToken)
    {
        var tasks = await _repository.GetAsync(cancellationToken: cancellationToken);

        var result = _mapper.Map<TaskContarct[]>(tasks);

        return result;
    }

    public async Task<ResultOf<TaskContarct>> Handle(CreateTaskCommand command, CancellationToken cancellationToken)
    {
        var task = TaskEntity.Create(command.Name, command.Description);

        await _repository.InsertAsync(task, cancellationToken);

        await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return _mapper.Map<TaskContarct>(task);
    }

    public async Task<ResultOf<TaskContarct>> Handle(UpdateTaskCommand command, CancellationToken cancellationToken)
    {
        var taskResult = await _repository.FirstOrDefaultAsync(t => t.Id == command.Id);

        if (taskResult.IsFailed)
        {
            return taskResult.Error!;
        }

        taskResult.Data!.Update(command.Name, command.Description);

        await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return _mapper.Map<TaskContarct>(taskResult.Data);
    }

    public async Task<ResultOf<Unit>> Handle(DeleteTaskCommand command, CancellationToken cancellationToken)
    {
        var taskResult = await _repository.FirstOrDefaultAsync(t => t.Id == command.Id, cancellationToken);

        if (taskResult.IsFailed)
        {
            return taskResult.Error!;
        }

        taskResult.Data!.Remove();

        _repository.Remove(taskResult.Data);

        await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return ResultOf<Unit>.Unit;
    }
}