using AspNetCore.WebApi.Dtos;
using AspNetCore.WebApi.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.WebApi
{
    public class MapperProfile : Profile
    {

        public MapperProfile()
        {
            CreateMap<AppointmentItem, AppointmentItemDto>().ReverseMap();
            CreateMap<AppointmentItem, AppointmentUpdateStatusDto>().ReverseMap();
            CreateMap<AppointmentItem, AppointmentCreateDto>().ReverseMap();
        }
    }
}
