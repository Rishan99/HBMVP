using HouseBrokerMVP.Business.DTO;
using HouseBrokerMVP.Business.Services;
using HouseBrokerMVP.Core.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HouseBrokerMVP.API.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
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
            try
            {
                var result = await _authService.LoginUser(model);
                return Ok(result);
            }
            catch (Exception ex)

            {
                return BadRequest(ex.Message);
            }
        }

        [Route("register-broker")]
        [HttpPost]
        public async Task<IActionResult> RegisterBroker(RegisterUserDto registerDto)
        {
            try
            {
                var user = await _authService.RegisterBroker(registerDto);
                return Ok(user.UserName);
            }
            catch (Exception ex)

            {
                return BadRequest(ex.Message);
            }
        }
        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterUserDto registerDto)
        {
            try
            {
                 await _authService.RegisterCustomer(registerDto);
                return Ok("Registered Successfully");
            }
            catch (Exception ex)

            {
                return BadRequest(ex.Message);
            }
        }
        [Route("change-password")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto data)
        {
            try
            {
                if (string.IsNullOrEmpty(data.OldPassword))
                {
                    throw new Exception("Old Password cannot be empty");
                }
                if (string.IsNullOrEmpty(data.NewPassword))
                {
                    throw new Exception("New Password and Confirm Password donot match");
                }
                await _authService.ChangePassword(data);
                return Ok("Password has been changed successfully");
            }
            catch (Exception ex)

            {
                return BadRequest(ex.Message);
            }
        }
        [Route("me")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMyDetails()
        {
            try
            {
                var data = await _authService.GetMyDetails();
                return Ok(data);
            }
            catch (Exception ex)

            {
                return BadRequest(ex.Message);
            }
        }



    }
}
