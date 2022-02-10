using AutoMapper;
using BookStoreAppApi.Data;
using BookStoreAppApi.Models.Author;

namespace BookStoreAppApi.Configurations;

public class MapperConfig : Profile
{
    public MapperConfig()
    {

        #region Author Mappings

        CreateMap<AuthorCreateDto, Author>().ReverseMap();
        CreateMap<AuthorUpdateDto, Author>().ReverseMap();
        CreateMap<AuthorReadOnlyDto, Author>().ReverseMap();    
        
        #endregion


    }
}