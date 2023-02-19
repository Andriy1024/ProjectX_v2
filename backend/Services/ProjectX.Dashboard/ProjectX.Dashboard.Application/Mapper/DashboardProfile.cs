using ProjectX.Dashboard.Application.Contracts;
using ProjectX.Realtime.Messages;

namespace ProjectX.Dashboard.Application.Mapper;

public class DashboardProfile : Profile
{
    public DashboardProfile()
    {
        CreateMap<TaskEntity, TaskContarct>();
        CreateMap<NoteEntity, NoteContarct>();
        CreateMap<BookmarkEntity, BookmarkContarct>();

        CreateMap<TaskEntity, TaskCreated>();
        CreateMap<TaskEntity, TaskUpdated>();
        CreateMap<TaskEntity, TaskDeleted>();

        CreateMap<NoteEntity, NoteCreated>();
        CreateMap<NoteEntity, NoteUpdated>();
        CreateMap<NoteEntity, NoteDeleted>();

        CreateMap<BookmarkEntity, BookmarkCreated>();
        CreateMap<BookmarkEntity, BookmarkUpdated>();
        CreateMap<BookmarkEntity, BookmarkDeleted>();
    }
}