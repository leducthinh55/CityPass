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
            CreateMap<Attraction, AttractionVM>()
                .ForMember(des => des.City,
                act => act.MapFrom(src => src.City.Name))
                .ForMember(des => des.Category,
                act => act.MapFrom(src => src.Category.Name));

            CreateMap<TicketType, TicketTypeVM>()
                .ForMember(des => des.City,
                act => act.MapFrom(src => src.Atrraction.City.Name))
                .ForMember(des => des.Atrraction,
                act => act.MapFrom(src => src.Atrraction.Name));
        }
    }
}
