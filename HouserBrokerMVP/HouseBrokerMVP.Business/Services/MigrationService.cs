using HouseBrokerMVP.Core.Entities;
using HouseBrokerMVP.Core.Enum;
using HouseBrokerMVP.Core.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerMVP.Business.Services
{
    public class MigrationService
    {
        private readonly IAuthRepository _authRepository;


        public MigrationService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task Run()
        {
            await AddDefaultRole();
            await AddSuperAdmin();
        }


        private async Task AddSuperAdmin()
        {
            var alreadyExistsUser = await _authRepository.GetUserByUsername("superadmin");
            if (alreadyExistsUser is not null) return;
            var user = await _authRepository.Register(new ApplicationUser()
            {
                Email = "superadmin",
                UserName = "superadmin",
                EmailConfirmed = true
            }, "Admin@123#");
            await _authRepository.AddUserToRole(user, nameof(RoleEnum.SuperAdmin));
        }
        private async Task AddDefaultRole()
        {
            var rolesExists = await _authRepository.GetAllRoles().AnyAsync();
            if (rolesExists) return;
            await _authRepository.AddRole(nameof(RoleEnum.SuperAdmin));
            await _authRepository.AddRole(nameof(RoleEnum.HouseSeeker));
            await _authRepository.AddRole(nameof(RoleEnum.Broker));
        }
    }
}
