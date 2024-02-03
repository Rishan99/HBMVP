using AutoMapper;
using HouseBrokerMVP.Business.DTO;
using HouseBrokerMVP.Core.Entities;
using HouseBrokerMVP.Core.Enum;
using HouseBrokerMVP.Core.Model;
using HouseBrokerMVP.Core.Repositories;
using HouseBrokerMVP.Core.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    public interface IAuthService
    {
        Task<Object> LoginUser(LoginDto loginDto);
        Task<ApplicationUser> RegisterBroker(RegisterAdminDto model);
        Task RegisterCustomer(RegisterUserDto model);
        Task ChangePassword(ChangePasswordDto passwordDto);
        Task<dynamic> GetMyDetails();
    }
    public class AuthService : IAuthService
    {
        private readonly JwtConfig _jwtModel;
        private readonly IAuthRepository _authRepository;
        private readonly IHttpContextAccessor _accessor;
        private IMapper _mapper;

        public AuthService(IAuthRepository authRepository, JwtConfig jwtModel, IHttpContextAccessor accessor, IMapper mapper)
        {
            _authRepository = authRepository;
            _jwtModel = jwtModel;
            _accessor = accessor;
            _mapper = mapper;
        }

        public async Task<Object> LoginUser(LoginDto loginDto)
        {

            ApplicationUser? user = await _authRepository.GetUserByUsername(loginDto.EmailAddress);
            if (user is null)
            {
                throw new Exception($"User with username {loginDto.EmailAddress} doesnt exists");
            }
            var status = await _authRepository.Login(loginDto.EmailAddress, loginDto.Password);
            if (!status)
            {

                throw new Exception("User name or password in incorrect");
            }
            var token = await GenerateJwtToken(user);
            dynamic? loggedInUser = await GetUserInformation(user.UserName);
            if (loggedInUser == null) throw new Exception("Cannnot Log in, Please try again");
            return new
            {
                UserName = user.UserName!,
                AccessToken = token,
                ExpiresIn = Convert.ToInt32(_jwtModel.ExpireDays) * 24 * 1000,
                User = loggedInUser
            };
        }


        public async Task<ApplicationUser> RegisterBroker(RegisterAdminDto model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                throw new Exception("Password and confirm password do not match");
            }
            using TransactionScope trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                ApplicationUser? user = await _authRepository.GetUserByUsername(model.EmailAddress);

                if (user is not null)
                {
                    throw new Exception("User with the same email address already exists. Please use a different email address");
                }

                var toRegister = new ApplicationUser()
                {
                    UserName = model.EmailAddress,
                    Email = model.EmailAddress,
                    PhoneNumber = model.PhoneNumber,
                };
                user = await _authRepository.Register(toRegister, model.Password);
                await _authRepository.AddUserToRole(user, nameof(RoleEnum.Broker));

                trans.Complete();
                return user;
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                trans.Dispose();
            }
        }

        public async Task RegisterCustomer(RegisterUserDto model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                throw new Exception("Password and confirm password do not match");
            }
            using TransactionScope trans = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            {
                try
                {
                    ApplicationUser? user = await _authRepository.GetUserByUsername(model.EmailAddress);
                    if (user is not null)
                    {
                        throw new Exception("User with same email address already exists, Please use different email address");
                    }
                    var toRegister = new ApplicationUser()
                    {
                        UserName = model.EmailAddress,
                        Email = model.EmailAddress,
                        PhoneNumber = model.PhoneNumber,
                    };
                    user = await _authRepository.Register(toRegister, model.Password);
                    await _authRepository.AddUserToRole(user, nameof(RoleEnum.HouseSeeker));
                    trans.Complete();
                }
                catch (Exception e)
                {
                    throw;
                }
                finally
                {
                    trans.Dispose();
                }
            }
        }

        public async Task ChangePassword(ChangePasswordDto passwordDto)
        {
            var userName = GeneralUtility.GetLoggedInUsername(_accessor);
            var user = await _authRepository.GetUserByUsername(userName);
            if (user is null) throw new Exception("Cannot find user");
            await _authRepository.ChangePassword(user, passwordDto.OldPassword, passwordDto.NewPassword);
        }

        public async Task<dynamic> GetMyDetails()
        {
            var userName = GeneralUtility.GetLoggedInUsername(_accessor);
            return await GetUserInformation(userName);
        }

        private async Task<UserDto> GetUserInformation(string userName)
        {
            var user = await _authRepository.GetUserByUsername(userName);
            if (user == null) throw new Exception("Cannot find user");
            return _mapper.Map<UserDto>(user);
        }

        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {

            var theRole = await _authRepository.GetRolesByUserName(user.UserName);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, string.Join(",",theRole)),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtModel.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_jwtModel.ExpireDays));
            var token = new JwtSecurityToken(
                _jwtModel.Issuer,
               _jwtModel.Issuer,
                claims,
                expires: expires,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token).ToString();
        }



    }
}
