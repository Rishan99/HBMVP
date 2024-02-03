using HouseBrokerMVP.Core.Context;
using HouseBrokerMVP.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerMVP.Core.Repositories
{

    public interface IPropertyReposiotry
    {
        IQueryable<Property> GetList(bool TrackEntity);
        Task<Property> Add(Property service);
        Task Delete(int id);
        Task<Property> Update(Property service);
        IQueryable<Property> GetById(int id);
    }
}
