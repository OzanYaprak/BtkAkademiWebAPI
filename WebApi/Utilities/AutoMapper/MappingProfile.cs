﻿using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace WebApi.Utilities.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BookDTOForUpdate, Book>().ReverseMap();
            CreateMap<BookDTOForInsertion, Book>().ReverseMap();
            CreateMap<BookDTO, Book>().ReverseMap();
        }
    }
}