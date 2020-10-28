using AutoMapper;
using LibraryAPI.Data;
using LibraryAPI.Models.Books;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Profiles
{
    public class BooksProfile : Profile
    {
        public BooksProfile()
        {
            CreateMap<Book, GetBookDetailsResponse>();
            CreateMap<Book, GetBooksResponseItem>();
            CreateMap<PostBookRequest, Book>()
                .ForMember(dest => dest.AddedToInventory, opt => opt.MapFrom((s) => DateTime.Now))
                .ForMember(dest => dest.IsInInventory, opt => opt.MapFrom((s) => true))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
               
        }
    }
}
