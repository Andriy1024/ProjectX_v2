using AutoMapper;
using ProjectX.Tasks.Application.Contracts;
using ProjectX.Tasks.Domain.Entities;

namespace ProjectX.Tasks.Application.Mapper;

public class TaskProfile : Profile
{
    public TaskProfile()
    {
        CreateMap<TaskEntity, TaskContarct>();
    }
}