using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace HouseBrokerMVP.Business.Services
{
    public class CoreService : ICoreService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CoreService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetLoggedInUserName()
        {
            var user = _httpContextAccessor.HttpContext.User ?? throw new Exception("User not found, Please try again");
            var userName = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
            if (userName is null)
                throw new Exception("Please Login to continue");
            return userName.Value;
        }
    }
}
