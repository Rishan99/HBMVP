using HouseBrokerMVP.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerMVP.Core.Repositories
{
    public interface IAuthRepository
    {
        Task<bool> Login(string username, string password);
        Task<ApplicationUser> Register(ApplicationUser user, string password);
        Task<ApplicationUser> GetUserById(string userId);
        Task<List<string>> GetRolesByUserName(string userName);
        Task ChangePassword(ApplicationUser user, string oldPassword, string newPassword);
        IQueryable<IdentityRole> GetAllRoles();
        Task<ApplicationUser?> GetUserByUsername(string username);
        Task<bool> AddUserToRole(ApplicationUser user, string role);
        Task<string> AddRole(string role);
    }
}
