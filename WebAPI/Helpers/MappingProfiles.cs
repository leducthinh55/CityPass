﻿using AutoMapper;
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
                act => act.MapFrom(src => src.Atrraction.Name))
                .ForMember(des => des.IsNew,
                act => act.MapFrom(src => src.CreateAt.AddMonths(1) > DateTime.Now))
                .ForMember(des => des.NumberOfVisiter,
                act => act.MapFrom(src => src.Tickets.Count))
                .ForMember(des => des.UrlImage,
                act => act.MapFrom(src => src.UrlImage != null ? src.UrlImage.Split(new char[] { ';' })[0] : ""));

            CreateMap<Pass, PassVM>()
                .ForMember(des => des.Collections,
                src => src.Ignore());

            CreateMap<PassCM, Pass>()
                .ForMember(des => des.Collections,
                src => src.Ignore())
                .ForMember(des => des.CreateAt,
                act => act.MapFrom(_ => DateTime.Now));

            CreateMap<PassUM, Pass>()
                .ForMember(des => des.Collections,
                src => src.Ignore())
                .ForMember(des => des.CreateAt,
                act => act.MapFrom(_ => DateTime.Now));

            CreateMap<Collection, CollectionVM>()
                .ForMember(des => des.TicketTypes,
                src => src.Ignore());

            CreateMap<UserPass, UserPassVM>();
            CreateMap<UserPassCM, UserPass>();

            CreateMap<UserPass, UserPassExpireVM>()
                .ForMember(des => des.UrlImage,
                act => act.MapFrom(_ => _.Pass.UrlImage))
                .ForMember(des => des.Name,
                act => act.MapFrom(_ => _.Pass.Name));

            CreateMap<Ticket, TicketVM>()
                .ForMember(des => des.TicketTypeName,
                act => act.MapFrom(_ => _.TicketType.Name));

            CreateMap<TicketType, TicketTypeDetailVM>()
                .ForMember(des => des.UrlImage,
                src => src.Ignore())
                .ForMember(des => des.UrlImages,
                act => act.MapFrom(_ => _.UrlImage.Split(new char[] { ';' })));

            CreateMap<TicketType, TicketTypeDetailVM1>()
                .ForMember(des => des.Lat,
                act => act.MapFrom(_ => _.Atrraction.Lat))
                .ForMember(des => des.Lng,
                act => act.MapFrom(_ => _.Atrraction.Lng));
        }
    }
}
