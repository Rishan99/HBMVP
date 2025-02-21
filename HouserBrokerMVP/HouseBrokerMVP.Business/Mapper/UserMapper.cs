using AutoMapper;
using HouseBrokerMVP.Business.DTO;
using HouseBrokerMVP.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerMVP.Business.Mapper
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<ApplicationUser, UserDto>();
        }

    }
}
