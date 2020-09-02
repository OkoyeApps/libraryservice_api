using AutoMapper;
using Library.Domain.Dtos;
using Library.Domain.Entities;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.Profiles
{
    public class BookProfiles : Profile
    {
        public BookProfiles()
        {
            CreateMap<BookDto, Book>().ForMember(dest => dest.Author, src => src.MapFrom(x => ObjectId.Parse(x.Author)))
                .ForMember(dest => dest.Total_Copies_Available, src => src.MapFrom(x => x.Total_Copies));
        }
    }
}
