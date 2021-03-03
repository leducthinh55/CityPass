using AutoMapper;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.ViewModels;

namespace WebAPI.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() : base()
        {
            CreateMap<AttractionVM, Attraction>();
            CreateMap<AttractionCM, Attraction>();
            CreateMap<Attraction, AttractionVM>().ForMember(des => des.City,
                act => act.MapFrom(src => src.City.Name));
        }
    }
}
