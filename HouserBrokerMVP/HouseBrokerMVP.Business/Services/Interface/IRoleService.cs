using HouseBrokerMVP.Business.DTO;

namespace HouseBrokerMVP.Business.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleListDto>> GetRoleList();
    }

}
