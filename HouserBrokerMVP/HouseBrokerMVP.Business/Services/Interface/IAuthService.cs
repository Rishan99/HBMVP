using HouseBrokerMVP.Business.DTO;
using HouseBrokerMVP.Core.Entities;

namespace HouseBrokerMVP.Business.Services
{
    public interface IAuthService
    {
        Task<Object> LoginUser(LoginDto loginDto);
        Task<ApplicationUser> RegisterBroker(RegisterUserDto model);
        Task ChangePassword(ChangePasswordDto passwordDto);
        Task<dynamic> GetMyDetails();
    }
}
