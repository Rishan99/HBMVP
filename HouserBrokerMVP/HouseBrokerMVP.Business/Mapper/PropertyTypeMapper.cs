using AutoMapper;
using HouseBrokerMVP.Business.DTO;
using HouseBrokerMVP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerMVP.Business.Mapper
{
    public class PropertyTypeMapper : Profile
    {
        public PropertyTypeMapper()
        {
            CreateMap<PropertyType, PropertyTypeListDto>();
            CreateMap<PropertyTypeInsertDto, PropertyType>();
            CreateMap<PropertyTypeUpdateDto, PropertyType>();
        }
    }
}
