using AutoMapper;
using ProjectX.Dashboard.Application.Contracts;

namespace ProjectX.Dashboard.Application.Mapper;

public class DashboardProfile : Profile
{
    public DashboardProfile()
    {
        CreateMap<TaskEntity, TaskContarct>();
        CreateMap<NoteEntity, NoteContarct>();
        CreateMap<BookmarkEntity, BookmarkContarct>();
    }
}