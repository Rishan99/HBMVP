using AutoMapper;
using HouseBrokerMVP.Business.DTO;
using HouseBrokerMVP.Core.Entities;
using HouseBrokerMVP.Core.Enum;
using HouseBrokerMVP.Core.Model;
using HouseBrokerMVP.Core.Repositories;
using HouseBrokerMVP.Core.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace HouseBrokerMVP.Business.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleListDto>> GetRoleList();
    }
    public class RoleService : IRoleService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IHttpContextAccessor _accessor;
        private IMapper _mapper;

        public RoleService(IAuthRepository authRepository, IHttpContextAccessor accessor, IMapper mapper)
        {
            _authRepository = authRepository;

            _accessor = accessor;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleListDto>> GetRoleList()
        {
            var data = await _authRepository.GetAllRoles().ToListAsync();
            return _mapper.Map<IEnumerable<RoleListDto>>(data);
        }

    }
}
