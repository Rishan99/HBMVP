using HouseBrokerMVP.Business.DTO;
using HouseBrokerMVP.Business.Services;
using HouseBrokerMVP.Core.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HouseBrokerMVP.API.Controllers
{
    [Route("api/[controller]")]
    //[AllowAnonymous]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var result = await _authService.LoginUser(model);
            return Ok(result);

        }

        [Route("register-broker")]
        [HttpPost]
        public async Task<IActionResult> RegisterBroker(RegisterUserDto registerDto)
        {
            var user = await _authService.RegisterBroker(registerDto);
            return Ok(user.UserName);
        }

        [Route("change-password")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto data)
        {
            await _authService.ChangePassword(data);
            return Ok("Password has been changed successfully");

        }
        [Route("me")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMyDetails()
        {
            var data = await _authService.GetMyDetails();
            return Ok(data);
        }
    }
}
