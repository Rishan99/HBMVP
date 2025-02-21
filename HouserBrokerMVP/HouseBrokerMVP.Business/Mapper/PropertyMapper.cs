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
    public class PropertyMapper : Profile
    {
        public PropertyMapper()
        {
            CreateMap<PropertyImage, string>().ConvertUsing(x=>x.ImageName);
            CreateMap<Property, PropertyListDto>().ForMember(x=>x.Images,y=>y.MapFrom(z=>z.PropertyImages));
            CreateMap<PropertyInsertDto, Property>();
            CreateMap<PropertyUpdateDto, Property>();
            CreateMap<PropertyImage, PropertyImageListDto>();
            
        }
    }
}
