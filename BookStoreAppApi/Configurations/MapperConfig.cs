using AutoMapper;
using BookStoreAppApi.Data;
using BookStoreAppApi.Models.Author;
using BookStoreAppApi.Models.Book;
using BookStoreAppApi.Models.User;

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

        #region Book Mappings

        CreateMap<BookCreateDto, Book>().ReverseMap();
        CreateMap<BookUpdateDto, Book>().ReverseMap();

        CreateMap<Book, BookReadOnlyDto>()
            .ForMember(q => q.AuthorName, 
                d => 
                    d.MapFrom(map => $"{map.Author!.FirstName} {map.Author.LastName}"))
            .ReverseMap();

        CreateMap<Book, BookDetailsDto>()
            .ForMember(q => q.AuthorName,
                d =>
                    d.MapFrom(map => $"{map.Author!.FirstName} {map.Author.LastName}"))
            .ReverseMap();

        #endregion

        #region Api User mappings

        CreateMap<ApiUser, UserDto>().ReverseMap();

        #endregion

    }
}