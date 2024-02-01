using AutoMapper;
using Library.Core.DTOs;
using Library.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CryptoCurrencyEntitie, CryptoCurrencyDTO>().ReverseMap();
            // ... other mappings
        }
    }
}
