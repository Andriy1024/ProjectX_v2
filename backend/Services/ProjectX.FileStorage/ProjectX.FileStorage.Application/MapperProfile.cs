using AutoMapper;
using ProjectX.FileStorage.Persistence.Database.Documents;

namespace ProjectX.FileStorage.Application;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<FileDocument, FileDto>();
    }
}