using AutoMapper;
using HouseBrokerMVP.Business.DTO;
using HouseBrokerMVP.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HouseBrokerMVP.Business.Services
{
    public class RoleService : IRoleService
    {
        private readonly IAuthRepository _authRepository;
        private IMapper _mapper;

        public RoleService(IAuthRepository authRepository, IMapper mapper)
        {
            _authRepository = authRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleListDto>> GetRoleList()
        {
            var data = await _authRepository.GetAllRoles().ToListAsync();
            return _mapper.Map<IEnumerable<RoleListDto>>(data);
        }
    }
}
