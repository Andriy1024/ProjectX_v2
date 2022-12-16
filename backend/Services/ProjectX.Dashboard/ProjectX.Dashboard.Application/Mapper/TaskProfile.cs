using AutoMapper;
using ProjectX.Dashboard.Application.Contracts;
using ProjectX.Dashboard.Domain.Entities;

namespace ProjectX.Dashboard.Application.Mapper;

public class TaskProfile : Profile
{
    public TaskProfile()
    {
        CreateMap<TaskEntity, TaskContarct>();
    }
}