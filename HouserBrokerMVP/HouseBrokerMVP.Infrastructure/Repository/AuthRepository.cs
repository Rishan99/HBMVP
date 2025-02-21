using HouseBrokerMVP.Core.Context;
using HouseBrokerMVP.Core.Entities;
using HouseBrokerMVP.Core.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerMVP.Infrastructure.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext authDbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthRepository(UserManager<ApplicationUser> userManager, AppDbContext authDbContext, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            this.authDbContext = authDbContext;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }


        public async Task<string> AddRole(string roleName)
        {
            IdentityResult result = await _roleManager.CreateAsync(new IdentityRole()
            {
                Name = $"{roleName}",
                NormalizedName = roleName.ToString(),
            });
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.FirstOrDefault()?.Description);
            }
            return roleName;
        }

        public async Task<bool> AddUserToRole(ApplicationUser user, string role)
        {
            IdentityResult addUserToRole = await _userManager.AddToRoleAsync(user, role);
            if (addUserToRole.Succeeded)
            {
                return addUserToRole.Succeeded;
            }
            throw new Exception(addUserToRole.Errors.FirstOrDefault()?.Description);

        }

        public async Task ChangePassword(ApplicationUser user, string oldPassword, string newPassword)
        {
            IdentityResult identityResult = await _userManager.ChangePasswordAsync(user: user,
                 oldPassword, newPassword);

            if (!identityResult.Succeeded)
            {
                throw new Exception(identityResult.Errors.FirstOrDefault()?.Description);
            }
        }

        public IQueryable<IdentityRole> GetAllRoles()
        {
            return _roleManager.Roles;
        }

        public async Task<List<string>> GetRolesByUserName(string userName)
        {
            var user = await GetUserByUsername(userName);
            var result = await _userManager.GetRolesAsync(user);
            return result.ToList();
        }

        public async Task<ApplicationUser> GetUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                throw new Exception("Invalid User");
            }
            return user;
        }

        public async Task<ApplicationUser?> GetUserByUsername(string username)
        {
            return await _userManager.FindByEmailAsync(username);
        }

        public async Task<bool> Login(string username, string password)
        {
            SignInResult result = await _signInManager.PasswordSignInAsync(username, password, true, false);
            return result.Succeeded;
        }

        public async Task<ApplicationUser> Register(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password: password);
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.FirstOrDefault()?.Description);
            }
            return user;
        }
    }
}
